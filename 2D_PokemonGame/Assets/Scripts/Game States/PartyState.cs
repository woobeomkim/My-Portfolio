using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PartyState : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;

    public Pokemon SelectedPokemon { get; private set; }

    bool IsSwitchingPosition;
    int selectedIndexForSwitching = 0;

    public static PartyState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    PokemonParty playerParty;

    private void Start()
    {
        playerParty = PlayerController.i.GetComponent<PokemonParty>();
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        SelectedPokemon = null;
        partyScreen.gameObject.SetActive(true);
        partyScreen.OnSelected += OnPokemonSelected;
        partyScreen.OnBack += OnBack;

        partyScreen.SetPartyData();
    }

    public override void Execute()
    {
        partyScreen.HandleUpdate();
    }

    public override void Exit()
    {
        partyScreen.gameObject.SetActive(false);
        partyScreen.OnSelected -= OnPokemonSelected;
        partyScreen.OnBack -= OnBack;
    }

    void OnPokemonSelected(int selection)
    {
        SelectedPokemon = partyScreen.SelectedMember;

        StartCoroutine(PokemonSeletecAction(selection));
    }

    IEnumerator PokemonSeletecAction(int selectedPokemonIndex)
    {
        var prevState = gc.StateMachine.GetPrevState();

        if (prevState == InventoryState.i)
        {
            // Use Item
            StartCoroutine(GoToUseItemState());
        }
        else if (prevState == BattleState.i)
        {
            var battleState = prevState as BattleState;


            DynamicMenuState.i.MenuItems = new List<string>() { "Shift", "Summary", "Cancel" };
            yield return gc.StateMachine.PushAndWait(DynamicMenuState.i);
            if (DynamicMenuState.i.SelectedItem == 0)
            {
                if (SelectedPokemon.HP <= 0)
                {
                    partyScreen.SetMessageText("기절한 포켓몬을 꺼낼수는 없습니다!");
                    yield break;
                }

                if (battleState.BattleSystem.PlayerUnits.Any(u => u.Pokemon == SelectedPokemon))
                {
                    partyScreen.SetMessageText("이 포켓몬은 이미 배틀중입니다!");
                    yield break;
                }
                if(battleState.BattleSystem.UnitCount > 1 && battleState.BattleSystem.IsPokemonSelectedToShift(SelectedPokemon))
                {
                    partyScreen.SetMessageText("이 포켓몬은 이미 내보냈습니다!");
                    yield break;
                }
            }
            else if (DynamicMenuState.i.SelectedItem == 1)
            {
                // TODO : Open summarty Screen
                SummaryState.i.SelectedPokemonIndex = selectedPokemonIndex;
                yield return gc.StateMachine.PushAndWait(SummaryState.i);
                // Debug.Log($"Selected Pokemon index {selection}");
            }
            else
            {
                yield break;
            }

            gc.StateMachine.Pop();
        }
        else
        {
            if(IsSwitchingPosition)
            {
                if(selectedIndexForSwitching == selectedPokemonIndex)
                {
                    partyScreen.SetMessageText("같은 포켓몬으로 바꿀수는 없습니다!");
                    yield break;
                }

                IsSwitchingPosition = false;

                var tmpPokemon = playerParty.Pokemons[selectedIndexForSwitching];
                playerParty.Pokemons[selectedIndexForSwitching] = playerParty.Pokemons[selectedPokemonIndex];
                playerParty.Pokemons[selectedPokemonIndex] = tmpPokemon;

                playerParty.PartyUpdated();

                yield break;
            }

            DynamicMenuState.i.MenuItems = new List<string>() { "Summary", "Switch Position", "Cancel" };
            yield return gc.StateMachine.PushAndWait(DynamicMenuState.i);
            if(DynamicMenuState.i.SelectedItem == 0)
            {
                // TODO : Open summarty Screen
                SummaryState.i.SelectedPokemonIndex = selectedPokemonIndex;
                yield return gc.StateMachine.PushAndWait(SummaryState.i);
                // Debug.Log($"Selected Pokemon index {selection}");
            }
            else if(DynamicMenuState.i.SelectedItem == 1)
            {
                // Switch Position
                IsSwitchingPosition = true;
                selectedIndexForSwitching = selectedPokemonIndex;
                partyScreen.SetMessageText("바꿀 포켓몬을 고르세요!");
            }
            else
            {
                yield break;
            }
            
        }
    }

    IEnumerator GoToUseItemState()
    {
        yield return gc.StateMachine.PushAndWait(UseItemState.i);
        gc.StateMachine.Pop();
    }

    void OnBack()
    {
        SelectedPokemon = null;

        var prevState = gc.StateMachine.GetPrevState();
        if(prevState == BattleState.i)
        {
            var battleState = prevState as BattleState;
            if (battleState.BattleSystem.PlayerUnits.Any(u => u.Pokemon != null && u.Pokemon.HP <= 0)) 
            {
                partyScreen.SetMessageText("포켓몬을 골라 계속 진행해야합니다.");
                return;
            }
        }

        gc.StateMachine.Pop();
    }
}
