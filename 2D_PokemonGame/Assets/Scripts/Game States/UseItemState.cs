using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UseItemState : State<GameController>
{
    [SerializeField] PartyScreen partyScreen;
    [SerializeField] InventoryUI inventoryUI;

    // Output
    public bool ItemUsed { get; private set; }

    public static UseItemState i { get; private set; }
    Inventory inventory;
    private void Awake()
    {
        i = this;
        inventory = Inventory.GetInventory();
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;

        ItemUsed = false;

        StartCoroutine(UseItem());
    }

    IEnumerator UseItem()
    {

        var item = inventoryUI.SelectedItem;
        var pokemon = partyScreen.SelectedMember;

        if(item is TmItem)
        {
            yield return HandleTmItems();
        }
        else
        {
            if (item is EvolutionItem)
            {
                var evolution = pokemon.CheckForEvolution(item);
                if (evolution != null)
                {
                    yield return EvolutionState.i.Evolve(pokemon, evolution);
                }
                else
                {
                    yield return DialogManager.i.ShowDialogText($"�������� ����Ҽ�����!");
                    gc.StateMachine.Pop();
                    yield break;
                }
            }

            var usedItem = inventory.UseItem(item, partyScreen.SelectedMember);
            if (usedItem != null)
            {
                ItemUsed = true;
                if (usedItem is RecoveryItem)
                    yield return DialogManager.i.ShowDialogText($"{usedItem.Name}�� ����ߴ�!");

            }
            else
            {
                if (inventoryUI.SelectedCategory == (int)ItemCategory.Items)
                    yield return DialogManager.i.ShowDialogText($"�������� ����Ҽ�����!");
            }

        }
        gc.StateMachine.Pop();
    }

    IEnumerator HandleTmItems()
    {
        var tmItem = inventoryUI.SelectedItem as TmItem;

        if (tmItem == null)
            yield break;

        var pokemon = partyScreen.SelectedMember;

        if (pokemon.HasMove(tmItem.Move))
        {
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(��)�� {tmItem.Move.Name}�� �̹� �˰��ִ�!");
            yield break;
        }

        if (!tmItem.CanBeTaugth(pokemon))
        {
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(��)�� {tmItem.Move.Name}�� ��� �� ����!");
            yield break;
        }

        if (pokemon.Moves.Count < PokemonBase.MaxNumOfMoves)
        {
            pokemon.LearnMove(tmItem.Move);
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(��)�� {tmItem.Move.Name}�� �����!");
        }
        else
        {
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(��)�� {tmItem.Move.Name}�� �������Ѵ�!");
            yield return DialogManager.i.ShowDialogText($"�׷��� {PokemonBase.MaxNumOfMoves}�� �̻��� ����� ��������.");
            yield return DialogManager.i.ShowDialogText($"�������� ����� ������!", true, false);

            MoveToForgetState.i.NewMove = tmItem.Move;
            MoveToForgetState.i.CurrentMoves = pokemon.Moves.Select(m => m.Base).ToList();
            yield return gc.StateMachine.PushAndWait(MoveToForgetState.i);

            int moveIndex = MoveToForgetState.i.Selection;
            if (moveIndex == PokemonBase.MaxNumOfMoves || moveIndex == -1)
            {
                // don't learn the new move
                yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(��)�� {tmItem.Move.Name}�� ����� �ʾҴ�!");
            }
            else
            {
                //forget selected move and learn new move
                var selectedMove = pokemon.Moves[moveIndex].Base;
                yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(��)�� {selectedMove.Name}�� �ذ� {tmItem.Move.Name}�� �����!");

                pokemon.Moves[moveIndex] = new Move(tmItem.Move);
            }

        }
    }
}
