using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuyingState : State<GameController>
{
    [SerializeField] Vector2 shopCameraOffset;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] ShopUI shopUI;
    [SerializeField] WalletUI walletUI;
    [SerializeField] CountSelectorUI countSelectorUI;

    public static ShopBuyingState i { get; private set; }
    bool browseItems = false;


    private void Awake()
    {
        i = this;
    }

    public List<ItemBase> AvailableItems { get; set; }


    Inventory inventory;
    private void Start()
    {
        inventory = Inventory.GetInventory();
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        browseItems = false;
        StartCoroutine(StartBuyingState());
    }

    public override void Execute()
    {
        if(browseItems)
            shopUI.HandleUpdate();
    }

    IEnumerator StartBuyingState()
    {
        yield return GameController.i.MoveCamera(shopCameraOffset);
        walletUI.Show();
        shopUI.Show(AvailableItems, (item) => StartCoroutine(BuyItem(item)),
            () => StartCoroutine(OnBackFromBuying()));

        browseItems = true;
    }

    IEnumerator BuyItem(ItemBase item)
    {
        browseItems = false;

        yield return DialogManager.i.ShowDialogText("몇개를 구매할거야 ?",
            waitForInput: false, autoClose: false);

        int countToBuy = 1;
        yield return countSelectorUI.ShowSelector(100, item.Price,
            (selectedCount) => { countToBuy = selectedCount; });

        DialogManager.i.CloseDialog();

        float totalPrice = item.Price * countToBuy;

        if (Wallet.i.HasMoney(totalPrice))
        {
            int selectedChoice = 0;
            yield return DialogManager.i.ShowDialogText($"{totalPrice} $ 가격인데 구매할거야?",
                waitForInput: false,
                choices: new List<string>() { "Yes", "No" },
                onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

            if (selectedChoice == 0)
            {
                // Yes
                inventory.AddItem(item, countToBuy);
                Wallet.i.TakeMoney(totalPrice);
                yield return DialogManager.i.ShowDialogText($"구매해줘서 고마워~");
            }
        }
        else
        {
            yield return DialogManager.i.ShowDialogText($"돈이 충분하지가 않아!");
        }

        browseItems = true;
    }

    IEnumerator OnBackFromBuying()
    {
        yield return GameController.i.MoveCamera(-shopCameraOffset);
        shopUI.Close();
        walletUI.Close();
        gc.StateMachine.Pop();
    }
}
