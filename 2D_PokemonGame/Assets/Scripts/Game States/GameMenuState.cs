using GDEUtils.StateMachine;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuState : State<GameController>
{
    [SerializeField] MenuController menuController;

     public static GameMenuState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;
        menuController.gameObject.SetActive(true);
        menuController.OnSelected += OnMenuItemSelected;
        menuController.OnBack += OnBack;
    }

    public override void Execute()
    {
        menuController.HandleUpdate();
    }

    public override void Exit()
    {
        menuController.gameObject.SetActive(false);
        menuController.OnSelected -= OnMenuItemSelected;
        menuController.OnBack -= OnBack;
    }

    void OnMenuItemSelected(int selection)
    {
        if (selection == 0)
            gc.StateMachine.Push(PartyState.i);
        else if (selection == 1)
            gc.StateMachine.Push(InventoryState.i);
        else if (selection == 2)
            StartCoroutine(SaveSelected());
        else if (selection == 3)
            StartCoroutine(LoadSelected());
        else if (selection == 4)
            gc.StateMachine.Push(StorageState.i);
    }

    IEnumerator SaveSelected()
    {
        yield return Fader.i.FadeIn(0.5f);
        SavingSystem.i.Save("saveSlot1");
        yield return Fader.i.FadeOut(0.5f);
    }

    IEnumerator LoadSelected()
    {
        yield return Fader.i.FadeIn(0.5f);
        SavingSystem.i.Load("saveSlot1");
        yield return Fader.i.FadeOut(0.5f);
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
