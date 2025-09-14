using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryState : State<GameController>
{
    [SerializeField] InventoryUI inventoryUI;

    // Output
    public ItemBase SelectedItem { get; private set; }

    public static InventoryState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;
        SelectedItem = null;

        inventoryUI.gameObject.SetActive(true);
        inventoryUI.OnSelected += OnItemSelected;
        inventoryUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        inventoryUI.HandleUpdate();
    }

    public override void Exit()
    {
        inventoryUI.gameObject.SetActive(false);
        inventoryUI.OnSelected -= OnItemSelected;
        inventoryUI.OnBack -= OnBack;
    }

    void OnItemSelected(int selection)
    {
        SelectedItem = inventoryUI.SelectedItem;

        if (gc.StateMachine.GetPrevState() != ShopSellingState.i)
            StartCoroutine(SelectPokemonAndUseItem());
        else
            gc.StateMachine.Pop();
    }

    void OnBack()
    {
        SelectedItem = null;
        gc.StateMachine.Pop();
    }

    IEnumerator SelectPokemonAndUseItem()
    {
        var prevState = gc.StateMachine.GetPrevState();
        if(prevState == BattleState.i)
        {
            if(!SelectedItem.CanUseInBattle)
            {
                // In Battle
                yield return DialogManager.i.ShowDialogText(" 이 아이템은 전투에서 사용할수 없어!");
                yield break;
            }
        }
        else
        {
            //OutSideBattle
            if (!SelectedItem.CanUseOutsideBattle)
            {
                yield return DialogManager.i.ShowDialogText(" 이 아이템은 전투에서 사용할수 없어!");
                yield break;
            }
        }

        if (SelectedItem is PokeballItem)
        {
            inventory.UseItem(SelectedItem, null);
            gc.StateMachine.Pop();
            yield break;
        }

        if(SelectedItem is TmItem)
        {
            gc.PartyScreen.ShowIfTmIsUsable(SelectedItem as TmItem);
        }

        yield return gc.StateMachine.PushAndWait(PartyState.i);

        if (prevState == BattleState.i)
        {
            if (UseItemState.i.ItemUsed)
                gc.StateMachine.Pop();
        }
    }
}
