using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuState : State<GameController>
{
    public static ShopMenuState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    // Input
    public List<ItemBase> AvailableItems { get; set; }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        StartCoroutine(StartMenuState());
    }

    IEnumerator StartMenuState()
    {

        int selectedChoice = 0;
        yield return DialogManager.i.ShowDialogText($"필요한 물품 있으세요 ?",
            waitForInput: false,
            choices: new List<string>() { "Buy", "Sell", "Quit" },
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            ShopBuyingState.i.AvailableItems = AvailableItems;
            yield return gc.StateMachine.PushAndWait(ShopBuyingState.i);
        }
        else if (selectedChoice == 1)
        {
            yield return gc.StateMachine.PushAndWait(ShopSellingState.i);
        }
        else if (selectedChoice == 2)
        {
           
        }
        gc.StateMachine.Pop();
    }
}
