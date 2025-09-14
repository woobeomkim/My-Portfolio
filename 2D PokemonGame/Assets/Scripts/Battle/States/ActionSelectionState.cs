using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionSelectionState : State<BattleSystem>
{
    [SerializeField] ActionSelectionUI selectionUI;

    public static ActionSelectionState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    BattleSystem bs;
    public override void Enter(BattleSystem owner)
    {
        bs = owner;

        selectionUI.gameObject.SetActive(true);
        selectionUI.OnSelected += OnActionSelected;

        bs.DialogBox.EnableActionSelector(true);

        bs.DialogBox.SetDialog($"{bs.UnitInSelection.Pokemon.Base.Name}�� �ൿ�� ������");
    }

    public override void Execute()
    {
        selectionUI.HandleUpdate();
    }

    public override void Exit()
    {
        selectionUI.gameObject.SetActive(false);
        selectionUI.OnSelected -= OnActionSelected;
    }

    void OnActionSelected(int selection)
    {
        if(selection == 0)
        {
            // Fight
            MoveSelectionState.i.Moves = bs.UnitInSelection.Pokemon.Moves;
            bs.StateMachine.ChangeState(MoveSelectionState.i);
        }
        else if(selection == 1)
        {
            StartCoroutine(GoToInventoryState());

        }
        else if (selection == 2)
        {
            StartCoroutine(GoToPartyState());
        }
        else if (selection == 3)
        {
            bs.AddBattleAction(new BattleAction()
            {
                Type = BattleActionType.Run
            });

        }
    }

    IEnumerator GoToPartyState()
    {
        yield return GameController.i.StateMachine.PushAndWait(PartyState.i);
        var selectedPokemon = PartyState.i.SelectedPokemon;
    
        if(selectedPokemon != null)
        {
            bs.AddBattleAction(new BattleAction()
            {
               Type = BattleActionType.SwitchPokemon,
               SelectedPokemon = selectedPokemon,
            });
        }
    }

    IEnumerator GoToInventoryState()
    {
        yield return GameController.i.StateMachine.PushAndWait(InventoryState.i);
        var seletedItem = InventoryState.i.SelectedItem;
        if (seletedItem != null)
        {
            bs.AddBattleAction(new BattleAction()
            {
                Type = BattleActionType.UseItem,
                SelectedItem = seletedItem
            });
        }
    }
}
