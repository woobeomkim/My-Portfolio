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
                    yield return DialogManager.i.ShowDialogText($"아이템을 사용할수없다!");
                    gc.StateMachine.Pop();
                    yield break;
                }
            }

            var usedItem = inventory.UseItem(item, partyScreen.SelectedMember);
            if (usedItem != null)
            {
                ItemUsed = true;
                if (usedItem is RecoveryItem)
                    yield return DialogManager.i.ShowDialogText($"{usedItem.Name}을 사용했다!");

            }
            else
            {
                if (inventoryUI.SelectedCategory == (int)ItemCategory.Items)
                    yield return DialogManager.i.ShowDialogText($"아이템을 사용할수없다!");
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
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(이)가 {tmItem.Move.Name}을 이미 알고있다!");
            yield break;
        }

        if (!tmItem.CanBeTaugth(pokemon))
        {
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(이)가 {tmItem.Move.Name}을 배울 수 없다!");
            yield break;
        }

        if (pokemon.Moves.Count < PokemonBase.MaxNumOfMoves)
        {
            pokemon.LearnMove(tmItem.Move);
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(이)가 {tmItem.Move.Name}을 배웠다!");
        }
        else
        {
            yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(이)가 {tmItem.Move.Name}을 배우려고한다!");
            yield return DialogManager.i.ShowDialogText($"그러나 {PokemonBase.MaxNumOfMoves}개 이상의 기술을 배울수없다.");
            yield return DialogManager.i.ShowDialogText($"잊으려는 기술을 고르세요!", true, false);

            MoveToForgetState.i.NewMove = tmItem.Move;
            MoveToForgetState.i.CurrentMoves = pokemon.Moves.Select(m => m.Base).ToList();
            yield return gc.StateMachine.PushAndWait(MoveToForgetState.i);

            int moveIndex = MoveToForgetState.i.Selection;
            if (moveIndex == PokemonBase.MaxNumOfMoves || moveIndex == -1)
            {
                // don't learn the new move
                yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(이)가 {tmItem.Move.Name}을 배우지 않았다!");
            }
            else
            {
                //forget selected move and learn new move
                var selectedMove = pokemon.Moves[moveIndex].Base;
                yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(이)가 {selectedMove.Name}을 잊고 {tmItem.Move.Name}을 배웠다!");

                pokemon.Moves[moveIndex] = new Move(tmItem.Move);
            }

        }
    }
}
