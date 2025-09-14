using GDEUtils.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSellingState : State<GameController>
{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] WalletUI walletUI;
    [SerializeField] CountSelectorUI countSelectorUI;

    public static ShopSellingState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    public List<ItemBase> AvailableItems { get; set; }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;

        StartCoroutine(StartSellingState());
    }

    IEnumerator StartSellingState()
    {
        yield return gc.StateMachine.PushAndWait(InventoryState.i);

        var selectedItem = InventoryState.i.SelectedItem;
        if (selectedItem != null)
        {
            yield return SellItem(selectedItem);
            StartCoroutine(StartSellingState());
        }
        else
            gc.StateMachine.Pop();
    }

    IEnumerator SellItem(ItemBase item)
    {
        if (!item.IsSellable)
        {
            yield return DialogManager.i.ShowDialogText($"이 아이템은 팔수 없어!");
            yield break;
        }

        walletUI.Show();

        float sellingPrice = Mathf.Round(item.Price / 2);
        int countToSell = 1;

        int itemCount = inventory.GetItemCount(item);

        if (itemCount > 1)
        {
            yield return DialogManager.i.ShowDialogText($"몇개의 아이템을 팔꺼야?",
                waitForInput: false, autoClose: false);

            yield return countSelectorUI.ShowSelector(itemCount, sellingPrice,
                (selectedCount) => { countToSell = selectedCount; });

            DialogManager.i.CloseDialog();
        }

        sellingPrice = sellingPrice * countToSell;

        int selectedChoice = 0;
        yield return DialogManager.i.ShowDialogText($"이 아이템은 {sellingPrice}원을 줄수있어! 아이템을 팔꺼야?",
            waitForInput: false,
            choices: new List<string>() { "Yes", "No" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            inventory.RemoveItem(item, countToSell);
            // TODO : Add Money
            Wallet.i.AddMoney(sellingPrice);
            yield return DialogManager.i.ShowDialogText($"좋아! {item.Name}을 받았고 {sellingPrice}원을 너에게 줬어!");

        }

        walletUI.Close();
    }
}
