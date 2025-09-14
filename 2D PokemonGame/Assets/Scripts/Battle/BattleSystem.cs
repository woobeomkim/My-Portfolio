using DG.Tweening;
using GDEUtils.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum BattleTrigger { LongGrass,Water}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnitSingle;
    [SerializeField] BattleUnit enemyUnitSingle;
    [SerializeField] List<BattleUnit> playerUnitsMulti;
    [SerializeField] List<BattleUnit> enemyUnitsMulti;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] Image playerImage;
    [SerializeField] Image trainerImage;
    [SerializeField] GameObject pokeballSprite;
    [SerializeField] MoveToForgetSelectionUI moveSelectionUI;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] GameObject singleBattleElements;
    [SerializeField] GameObject multiBattleElements;

    [Header("Audio")]
    [SerializeField] AudioClip wildBattleMusic;
    [SerializeField] AudioClip trainerBattleMusic;
    [SerializeField] AudioClip battleVictoryMusic;

    [Header("Background Images")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Sprite grassBackground; 
    [SerializeField] Sprite waterBackground;

    List<BattleUnit> playerUnits;
    List<BattleUnit> enemyUnits;

    int unitCount = 1;
    int unitInSelectionIndex = 0;

    public StateMachine<BattleSystem> StateMachine { get; private set; }

    public Action<bool> OnBattleOver;

    List<BattleAction> battleActions;

    public bool IsBattleOver { get; private set; }



    public PokemonParty PlayerParty{get; private set;}
    public PokemonParty TrainerParty{get; private set;}
    public Pokemon WildPokemon { get; private set; }

    public bool IsTrainerBattle { get; private set; } = false;
    PlayerController player;
    public  TrainerController Trainer { get; private set; }

    public BattleField Field { get; private set; }
    public int EscapeAttempts { get; set; }

    BattleTrigger battleTrigger;
    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon,
        BattleTrigger trigger = BattleTrigger.LongGrass, WeatherConditionID weather = WeatherConditionID.none)
    {
        this.PlayerParty = playerParty;
        this.WildPokemon = wildPokemon;
        this.unitCount = 1;

        player = playerParty.GetComponent<PlayerController>();
        IsTrainerBattle = false;

        battleTrigger = trigger;

        AudioManager.i.PlayMusic(wildBattleMusic);

        StartCoroutine(SetUpBattle(weather));
    }

    public void StartTrainerBattle(PokemonParty playerParty, PokemonParty trainerParty,
        BattleTrigger trigger = BattleTrigger.LongGrass , WeatherConditionID weather = WeatherConditionID.none, int unitCount = 1)
    {
        this.PlayerParty = playerParty;
        this.TrainerParty = trainerParty;
        this.unitCount = unitCount;

        IsTrainerBattle = true;
        player = playerParty.GetComponent<PlayerController>();
        Trainer = trainerParty.GetComponent<TrainerController>();

        battleTrigger = trigger;

        AudioManager.i.PlayMusic(trainerBattleMusic);

        StartCoroutine(SetUpBattle(weather));
    }

    IEnumerator SetUpBattle(WeatherConditionID weather)
    {
        singleBattleElements.SetActive(unitCount == 1);
        multiBattleElements.SetActive(unitCount > 1);

        if(unitCount == 1)
        {
            playerUnits = new List<BattleUnit>() { playerUnitSingle };
            enemyUnits = new List<BattleUnit>() { enemyUnitSingle };
        }
        else if (unitCount > 1)
        {
            playerUnits = playerUnitsMulti.GetRange(0, playerUnitsMulti.Count);
            enemyUnits = enemyUnitsMulti.GetRange(0, enemyUnitsMulti.Count);
        }
        StateMachine = new StateMachine<BattleSystem>(this);
        battleActions = new List<BattleAction>();

        for (int i = 0; i < unitCount; i++)
        {
            playerUnits[i].Clear();
            enemyUnits[i].Clear();
        }

        backgroundImage.sprite = (battleTrigger == BattleTrigger.LongGrass) ? grassBackground : waterBackground;
        if(!IsTrainerBattle)
        {
            // WildPokemon Battle
            playerUnits[0].SetUp(PlayerParty.GetHealthPokemon());
            enemyUnits[0].SetUp(WildPokemon);

            dialogBox.SetMoveNames(playerUnits[0].Pokemon.Moves);
            yield return dialogBox.TypeDialog($"야생의 {enemyUnits[0].Pokemon.Base.Name}(이)가 나타났다!");
        }
        else
        {
            for (int i = 0; i < unitCount; i++)
            {
                playerUnits[i].gameObject.SetActive(false);
                enemyUnits[i].gameObject.SetActive(false);
            }


            playerImage.gameObject.SetActive(true);
            trainerImage.gameObject.SetActive(true);

            playerImage.sprite = player.Sprite;
            trainerImage.sprite = Trainer.Sprite;

            yield return dialogBox.TypeDialog($"{Trainer.Name}(이)가 배틀을 걸어왔다!");

            // 트레이너 포켓몬
            trainerImage.gameObject.SetActive(false);
            var enemyPokemons = TrainerParty.GetHealthPokemons(unitCount);

            for (int i = 0; i < enemyPokemons.Count; i++)
            {
                enemyUnits[i].gameObject.SetActive(true);
                enemyUnits[i].SetUp(enemyPokemons[i]);
            }

            var pokemonNames = String.Join(" and ", enemyPokemons.Select(p => p.Base.Name));
            yield return dialogBox.TypeDialog($"{Trainer.Name}(이)가 {pokemonNames}을 내보냈다!");

            // 플레이어 포켓몬
            playerImage.gameObject.SetActive(false);
            var playerPokemons = PlayerParty.GetHealthPokemons(unitCount);

            for (int i = 0; i < playerPokemons.Count; i++)
            {
                playerUnits[i].gameObject.SetActive(true);
                playerUnits[i].SetUp(playerPokemons[i]);
            }

            pokemonNames = String.Join(" and ", playerPokemons.Select(p => p.Base.Name));
            yield return dialogBox.TypeDialog($"가라! {pokemonNames}");
        }

        Field = new BattleField();
        if(weather != WeatherConditionID.none)
        {
            Field.SetWeahter(weather);
            yield return dialogBox.TypeDialog(Field.Weather.StartMessage);
        }

        IsBattleOver = false;
        EscapeAttempts = 0;
        partyScreen.Init();
        unitInSelectionIndex = 0;

        StateMachine.ChangeState(ActionSelectionState.i);
    }

   
    public void BattleOver(bool won)
    {
        IsBattleOver = true;
        PlayerParty.Pokemons.ForEach(p => p.OnBattleOver());

        playerUnits.ForEach(u => u.Hud.ClearData());
        enemyUnits.ForEach(u => u.Hud.ClearData());

        OnBattleOver(won);
    }


    public void HandleUpdate()
    {
        StateMachine.Execute();
    }

    public void AddBattleAction(BattleAction battleAction)
    {
        battleAction.User = UnitInSelection;
        battleActions.Add(battleAction);

        if(battleActions.Count == ActivePlayerUnitsCount)
        {
            
            // Add enemy actions
            foreach(var enemyUnit in enemyUnits)
            {
                battleActions.Add(new BattleAction()
                {
                    Type = BattleActionType.Move,
                    SelectedMove = enemyUnit.Pokemon.GetRandomMove(),
                    User = enemyUnit,
                    Target = playerUnits[UnityEngine.Random.Range(0, ActivePlayerUnitsCount)]
                });
            }
            // Sort the actions by it's priority and speed
            battleActions = battleActions.OrderByDescending(a => a.Priority).ThenByDescending(a => a.User.Pokemon.Base.Speed).ToList();

            // Run Turns
            RunTurnState.i.Actions = battleActions;
            StateMachine.ChangeState(RunTurnState.i);
        }
        else
        {
            // select another action
            ++unitInSelectionIndex;
            StateMachine.ChangeState(ActionSelectionState.i);
        }
    }

    public void ClearTurnData()
    {
        battleActions = new List<BattleAction>();
        unitInSelectionIndex = 0;
    }

    public IEnumerator SwitchPokemon(Pokemon newPokemon, BattleUnit unitToSwitch)
    {
        if (unitToSwitch.Pokemon.HP > 0)
        {
            yield return dialogBox.TypeDialog($"돌아와 {unitToSwitch.Pokemon.Base.Name}");
            unitToSwitch.PlayFaintAnimation();
            yield return new WaitForSeconds(2.0f);
        }
        unitToSwitch.SetUp(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);
        yield return dialogBox.TypeDialog($"Go {newPokemon.Base.Name}!");
    }

    public IEnumerator SendNextTrainerPokemon(int faintedUnitIndex = 0)
    {
        var activePokmons = EnemyUnits.Select(u => u.Pokemon).Where(p => p.HP > 0).ToList();

        var nextPokemon = TrainerParty.GetHealthPokemon(dontInclude: activePokmons);
        enemyUnits[faintedUnitIndex].SetUp(nextPokemon);
        yield return dialogBox.TypeDialog($"{Trainer.Name}(이)가 {nextPokemon.Base.Name}을 내보냈다!");
    }

    public IEnumerator ThrowPokeball(PokeballItem pokeballItem)
    {
        if(IsTrainerBattle)
        {
            yield return dialogBox.TypeDialog($"트레이너 포켓몬을 훔치는건 안돼!");
            yield break;
        }

        yield return dialogBox.TypeDialog($"{player.Name}(이)가 {pokeballItem.Name}을 사용했다!");

        var playerUnit = playerUnits[0];
        var enemyUnit = enemyUnits[0];

        var pokeballObj = Instantiate(pokeballSprite, playerUnit.transform.position, Quaternion.identity);
        var pokeball = pokeballObj.GetComponent<SpriteRenderer>();
        pokeball.sprite = pokeballItem.Icon;

        // Animations
        yield return pokeball.transform.DOJump(enemyUnit.transform.position + new Vector3(0, 2), 2f, 1, 1f).WaitForCompletion();
        yield return enemyUnit.PlayCaptureAnimation();
        yield return pokeball.transform.DOLocalMoveY(enemyUnit.transform.position.y - 1.8f, 0.5f).WaitForCompletion();

        int shakeCount = TryToCatchPokemon(enemyUnit.Pokemon, pokeballItem);

        for (int i = 0; i < Mathf.Min(shakeCount,3); i++) 
        {
            yield return new WaitForSeconds(0.5f);
            yield return pokeball.transform.DOPunchRotation(new Vector3(0, 0, 10f), 0.8f).WaitForCompletion();
        }

        if(shakeCount == 4)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}을 잡았다!");
            yield return pokeball.DOFade(0, 1.5f).WaitForCompletion();

            PlayerParty.AddPokemon(enemyUnit.Pokemon);
            yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}을 파티에 추가하였습니다!");
            

            Destroy(pokeball);
            BattleOver(true);
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            pokeball.DOFade(0, 0.2f);
            yield return enemyUnit.PlayBreakOutAnimation();

            if(shakeCount<2)
                yield return dialogBox.TypeDialog($"{enemyUnit.Pokemon.Base.Name}(이)가 튀어나왔다!");
            else
                yield return dialogBox.TypeDialog($"아깝다! 거의다 잡았는데!");


            Destroy(pokeball);
        }
    }

    int TryToCatchPokemon(Pokemon pokemon, PokeballItem pokeballItem)
    {
        float a = (3 * pokemon.MaxHp - 2 * pokemon.HP) * pokemon.Base.CatchRate * pokeballItem.CatchRateModifier * StatusConditionsDB.GetStatusBonus(pokemon.Status) / (3 * pokemon.MaxHp);

        if (a >= 255)
            return 4;

        float b = 1048560 / Mathf.Sqrt(Mathf.Sqrt(16711600 / a));

        int shakeCount = 0;
        while (shakeCount < 4) 
        {
            if (UnityEngine.Random.Range(0, 65535) >= b)
                break;
         
            ++shakeCount;
        }

        return shakeCount;
    }

    public bool IsPokemonSelectedToShift(Pokemon pokemon)
    {
        return battleActions.Any(a => a.Type == BattleActionType.SwitchPokemon && a.SelectedPokemon == pokemon);
    }
    public BattleDialogBox DialogBox => dialogBox;

    public List<BattleUnit> PlayerUnits => playerUnits;
    public List<BattleUnit> EnemyUnits => enemyUnits;
    public int UnitCount => unitCount;
    public int ActivePlayerUnitsCount => playerUnits.Count(u => u.Pokemon != null && u.Pokemon.HP > 0);
    public int ActiveEnemyUnistCount => enemyUnits.Count(u => u.Pokemon != null && u.Pokemon.HP > 0);
    public BattleUnit UnitInSelection => playerUnits[unitInSelectionIndex];
    public PartyScreen PartyScreen => partyScreen;

    public AudioClip BattleVictoryMusic => battleVictoryMusic;


}
