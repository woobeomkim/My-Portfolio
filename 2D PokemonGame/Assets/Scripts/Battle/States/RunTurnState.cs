using GDEUtils.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class RunTurnState : State<BattleSystem>
{
    public static RunTurnState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    // Input
    public List<BattleAction> Actions { get; set; }

    BattleSystem bs;
    BattleDialogBox dialogBox;
    PartyScreen partyScreen;
    bool isTrainerBattle;
    PokemonParty playerParty;
    PokemonParty trainerParty;


    public override void Enter(BattleSystem owner)
    {
        bs = owner;

        dialogBox = bs.DialogBox;
        partyScreen = bs.PartyScreen;
        isTrainerBattle = bs.IsTrainerBattle;
        playerParty = bs.PlayerParty;
        trainerParty = bs.TrainerParty;

        StartCoroutine(RunTurns());
    }

    IEnumerator RunTurns()
    {
        foreach (var action in Actions)
        {
            if (action.IsInvalid)
                continue;

            if(action.Type == BattleActionType.Move)
            {
                action.User.Pokemon.CurrentMove = action.SelectedMove;

                yield return RunMove(action.User, action.Target, action.SelectedMove);
                yield return RunAfterTurn(action.User);
            }
            else if(action.Type == BattleActionType.SwitchPokemon)
            {
                yield return bs.SwitchPokemon(action.SelectedPokemon, action.User);
            }
            else if (action.Type == BattleActionType.UseItem)
            {
                if (action.SelectedItem is PokeballItem)
                {
                    yield return bs.ThrowPokeball(action.SelectedItem as PokeballItem);
                   
                }
                else
                {
                    // This is handled from item screen , so do nothing and skip to enemy move
                }
            }
            else if (action.Type == BattleActionType.Run)
            {
                dialogBox.EnableActionSelector(false);
                yield return TryToEscape();
            }

            if (bs.IsBattleOver) break;
        }

        if(bs.Field.Weather != null)
        {
            yield return RunWeatherEffects(bs.Field.Weather);
        }

        bs.ClearTurnData();

        if (!bs.IsBattleOver)
            bs.StateMachine.ChangeState(ActionSelectionState.i);
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeMove();
        if (!canRunMove)
        {
            yield return ShowStatusChanged(sourceUnit);
            yield return sourceUnit.Hud.WaitForHPUpdate();
            yield break;
        }

        move.PP--;

        yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name}(이)가 {move.Base.Name}을 사용했다!");

        if (CheckIfMoveHits(move, sourceUnit.Pokemon, targetUnit.Pokemon))
        {
            int hitCount = 0;
            float typeEffectiveness = 1.0f;
            for (int i = 1; i <= move.Base.GetHitTimes(); i++) 
            {
                sourceUnit.PlayAttackAnimation();
                AudioManager.i.PlaySfx(move.Base.Sound);
                yield return new WaitForSeconds(1.0f);

                targetUnit.PlayHitAnimation();
                AudioManager.i.PlaySfx(AudioID.Hit);

                if (move.Base.Category == MoveCategory.Status)
                {
                    yield return RunMoveEffects(move.Base.Effects, sourceUnit, targetUnit, move.Base.Target);
                }
                else
                {
                    float weatherModifier = bs.Field.Weather.OnDamageModify?.Invoke(move) ?? 1f;

                    var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon, weatherModifier);
                    yield return targetUnit.Hud.UpdateHPAsync();
                    yield return ShowDamageDetails(damageDetails);
                    typeEffectiveness = damageDetails.TypeEffectiveness;

                }

                if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnit.Pokemon.HP > 0)
                {
                    foreach (var secondary in move.Base.Secondaries)
                    {
                        var rnd = UnityEngine.Random.Range(1, 101);
                        if (rnd <= secondary.Chance)
                        {
                            yield return RunMoveEffects(secondary, sourceUnit, targetUnit, secondary.Target);
                        }
                    }
                }

                hitCount = i;

                if (targetUnit.Pokemon.HP <= 0)
                    break;
            }

            yield return ShowTypeEffectiveness(typeEffectiveness);

            if (move.Base.IsMultiHitMove)
                yield return dialogBox.TypeDialog($"Hit {hitCount} times!");

            if (targetUnit.Pokemon.HP <= 0)
            {
                yield return HandlePokemonFainted(targetUnit);
            }
        }
        else
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name}의 공격이 빗나갔습니다!");
        }


        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanged(sourceUnit);
        yield return sourceUnit.Hud.WaitForHPUpdate();

        if (sourceUnit.Pokemon.HP <= 0)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.Name}(이)가 쓰러졌다!");
            sourceUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2.0f);

            yield return NextStepsAfterFainting(sourceUnit);
        }
    }

    IEnumerator RunMoveEffects(MoveEffects effects, BattleUnit source, BattleUnit target, MoveTarget moveTarget)
    {
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
                source.Pokemon.ApplyBoosts(effects.Boosts);
            else
                target.Pokemon.ApplyBoosts(effects.Boosts);
        }

        if (effects.Status != StatusConditionID.none)
        {
            target.Pokemon.SetStatus(effects.Status);
        }

        if (effects.VolatileStatus != StatusConditionID.none)
        {
            target.Pokemon.SetVolatileStatus(effects.VolatileStatus);
        }

        if( effects.Weather != WeatherConditionID.none)
        {
            bs.Field.SetWeahter(effects.Weather, 5);
            yield return dialogBox.TypeDialog(bs.Field.Weather.StartMessageByMove ?? bs.Field.Weather.StartMessage);
        }

        yield return ShowStatusChanged(source);
        yield return ShowStatusChanged(target);
    }

    IEnumerator HandlePokemonFainted(BattleUnit faintedUnit)
    {
        yield return dialogBox.TypeDialog($"{faintedUnit.Pokemon.Base.Name}(이)가 쓰러졌다!");
        faintedUnit.PlayFaintAnimation();

        yield return new WaitForSeconds(2.0f);

        if (!faintedUnit.IsPlayerUnit)
        {
            bool battleWon = true;
            if (isTrainerBattle)
                battleWon = trainerParty.GetHealthPokemon() == null;

            if (battleWon)
                AudioManager.i.PlayMusic(bs.BattleVictoryMusic);

            // Exp Gain
            int expYield = faintedUnit.Pokemon.Base.ExpYield;
            int enemyLevel = faintedUnit.Pokemon.Level;
            float trainerBounus = (isTrainerBattle) ? 1.5f : 1.0f;

            int expGain = Mathf.FloorToInt((expYield * enemyLevel * trainerBounus) / 7);
            expGain /= bs.ActivePlayerUnitsCount;

            for (int i = 0; i < bs.ActivePlayerUnitsCount; i++) 
            {
                var playerUnit = bs.PlayerUnits[i];

                playerUnit.Pokemon.Exp += expGain;
                yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}(이)가 {expGain}exp를 얻었다!");
                yield return playerUnit.Hud.SetExpSmooth();
                // Check Level Up
                while (playerUnit.Pokemon.CheckForLevelUp())
                {
                    playerUnit.Hud.SetLevel();
                    yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}(이)가 레벨{playerUnit.Pokemon.Level}가 되었다");

                    //Try to learn a new move
                    var newMove = playerUnit.Pokemon.GetLearnableMoveAtCurrLevel();
                    if (newMove != null)
                    {
                        if (playerUnit.Pokemon.Moves.Count < PokemonBase.MaxNumOfMoves)
                        {
                            playerUnit.Pokemon.LearnMove(newMove.moveBase);
                            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}(이)가 {newMove.moveBase.Name}을 배웠다!");
                            dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);
                        }
                        else
                        {
                            yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}(이)가 {newMove.moveBase.Name}을 배우려고한다");
                            yield return dialogBox.TypeDialog($"하지만 {PokemonBase.MaxNumOfMoves}개 이상의 기술을 배울수가 없다.");
                            yield return dialogBox.TypeDialog($"잊고싶은 기술을 고르세요!");

                            MoveToForgetState.i.CurrentMoves = playerUnit.Pokemon.Moves.Select(m => m.Base).ToList();
                            MoveToForgetState.i.NewMove = newMove.moveBase;
                            yield return GameController.i.StateMachine.PushAndWait(MoveToForgetState.i);

                            int moveIndex = MoveToForgetState.i.Selection;
                            if (moveIndex == PokemonBase.MaxNumOfMoves || moveIndex == -1)
                            {
                                // don't learn the new move
                                yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}(이)가 {newMove.moveBase.Name}을 배우지 않았다!");
                            }
                            else
                            {
                                //forget selected move and learn new move
                                var selectedMove = playerUnit.Pokemon.Moves[moveIndex].Base;
                                yield return dialogBox.TypeDialog($"{playerUnit.Pokemon.Base.Name}(이)가 {selectedMove.Name}을 잊고 {newMove.moveBase.Name}을 배웠다!");

                                playerUnit.Pokemon.Moves[moveIndex] = new Move(newMove.moveBase);
                            }
                        }
                    }
                    yield return playerUnit.Hud.SetExpSmooth(true);

                }
            }
            

            yield return new WaitForSeconds(1.0f);
        }


        yield return NextStepsAfterFainting(faintedUnit);
    }

    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (bs.IsBattleOver) yield break;

        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanged(sourceUnit);
        yield return sourceUnit.Hud.UpdateHPAsync();

        if (sourceUnit.Pokemon.HP <= 0)
        {
            yield return HandlePokemonFainted(sourceUnit);
        }
    }

    IEnumerator RunWeatherEffects(WeatherCondition weather)
    {
        if(bs.Field.WeatherDuration != null)
        {
            if(bs.Field.WeatherDuration > 0)
            {
                --bs.Field.WeatherDuration;
            }
            else
            {
                bs.Field.SetWeahter(WeatherConditionID.none, null);
                if(weather.EndMessage != null)
                    yield return dialogBox.TypeDialog(weather.EndMessage);

                yield break;
            }
        }    

        if (weather.EffectMessage != null) 
            yield return dialogBox.TypeDialog(weather.EffectMessage);

        var units = bs.PlayerUnits.Concat(bs.EnemyUnits);
        
        foreach(var unit in units)
        {
            if (unit.Pokemon == null || unit.Pokemon.HP == 0)
                continue;
            weather.OnWeatherEffect?.Invoke(unit.Pokemon);
            yield return ShowStatusChanged(unit);

            if (unit.Pokemon.HP <= 0)
                yield return HandlePokemonFainted(unit);
        }
    }

    public IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1.0f)
            yield return dialogBox.TypeDialog($"급소를 공격하였다!");

    }

    public IEnumerator ShowTypeEffectiveness(float typeEffectiveness)
    {
        if (typeEffectiveness > 1.0f)
            yield return dialogBox.TypeDialog($"효과가 뛰어난것 같다!");
        else if (typeEffectiveness < 1.0f)
            yield return dialogBox.TypeDialog($"효과가 별로인것 같다!");
    }

    IEnumerator NextStepsAfterFainting(BattleUnit faintedUnit)
    {
        // Remove the action of the fainted
        var actionToRemove = Actions.FirstOrDefault(a => a.User == faintedUnit);
        if (actionToRemove != null)
            actionToRemove.IsInvalid = true;

        if (faintedUnit.IsPlayerUnit)
        {
            var activePokmons = bs.PlayerUnits.Select(u => u.Pokemon).Where(p => p.HP > 0).ToList();

            var nextPokemon = playerParty.GetHealthPokemon(dontInclude: activePokmons);

            if (nextPokemon == null && activePokmons.Count == 0)
            {
                bs.BattleOver(false);

            }
            else if (nextPokemon == null && activePokmons.Count > 0)
            {
                // No new pokemon to send out, but we can continue the battle with the active pokemon
                bs.PlayerUnits.Remove(faintedUnit);
                faintedUnit.Hud.gameObject.SetActive(false);

                // Attacks targeted at the fainted unit should be changed
                var actionsToChange = Actions.Where(a => a.Target == faintedUnit).ToList();
                actionsToChange.ForEach(a => a.Target = bs.PlayerUnits.First());
            }
            else if (nextPokemon != null)
            {
                // Send out next pokemon
                yield return GameController.i.StateMachine.PushAndWait(PartyState.i);
                yield return bs.SwitchPokemon(PartyState.i.SelectedPokemon, faintedUnit);
            }
        }
        else
        {
            if(!isTrainerBattle)
            { 
                bs.BattleOver(true);
                yield break;
            }
            var activePokmons = bs.EnemyUnits.Select(u => u.Pokemon).Where(p => p.HP > 0).ToList();

            var nextPokemon = trainerParty.GetHealthPokemon(dontInclude: activePokmons);

            if (nextPokemon == null && activePokmons.Count == 0)
            {
                bs.BattleOver(true);

            }
            else if (nextPokemon == null && activePokmons.Count > 0)
            {
                // No new pokemon to send out, but we can continue the battle with the active pokemon
                bs.EnemyUnits.Remove(faintedUnit);
                faintedUnit.Hud.gameObject.SetActive(false);

                var actionsToChange = Actions.Where(a => a.Target == faintedUnit).ToList();
                actionsToChange.ForEach(a => a.Target = bs.EnemyUnits.First());
            }
            else if (nextPokemon != null)
            {
                // Send out next pokemon
                if(bs.UnitCount == 1)
                {
                    AboutToUseState.i.NewPokemon = nextPokemon;
                    yield return bs.StateMachine.PushAndWait(AboutToUseState.i);
                }
                else
                {
                    yield return bs.SendNextTrainerPokemon(bs.EnemyUnits.IndexOf(faintedUnit));
                }
            }
        }

    }

    bool CheckIfMoveHits(Move move, Pokemon source, Pokemon target)
    {
        if (move.Base.AlwaysHits)
            return true;

        float moveAccuracy = move.Base.Accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1f, 4f / 3f, 5f / 3f, 2f, 7f / 3f, 8f / 3f, 3f };

        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }

    IEnumerator ShowStatusChanged(BattleUnit unit)
    {
        var pokemon = unit.Pokemon;

        while (pokemon.StatusChanges.Count > 0)
        {
            var statusEvent = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(statusEvent.Message);

            if(statusEvent.Type == StatusEventType.Damage)
            {
                unit.PlayHitAnimation();
                AudioManager.i.PlaySfx(AudioID.Hit);
                yield return unit.Hud.UpdateHPAsync();
            }
        }
    }

    IEnumerator TryToEscape()
    {
        if (isTrainerBattle)
        {
            yield return dialogBox.TypeDialog($"트레이너 배틀중에는 도망갈수없어!");
            yield break;
        }

        ++bs.EscapeAttempts;

        int playerSpeed = bs.PlayerUnits[0].Pokemon.Speed;
        int enemySpeed = bs.EnemyUnits[0].Pokemon.Speed;

        if (enemySpeed < playerSpeed)
        {
            yield return dialogBox.TypeDialog($"안전하게 도망쳤다!");
            bs.BattleOver(true);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * bs.EscapeAttempts;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                yield return dialogBox.TypeDialog($"안전하게 도망쳤다!");
                bs.BattleOver(true);
            }
            else
            {
                yield return dialogBox.TypeDialog($"도망칠수없다!");
            }
        }
    }
}
