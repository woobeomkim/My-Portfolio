using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfableWater : MonoBehaviour, Interactable, iPlayerTriggerable
{
    bool isJumpingToWater = false;

    public bool TriggerRepeatedly => true;

    public IEnumerator Interact(Transform initiator)
    {
        var animator = initiator.GetComponent<CharacterAnimator>();
        if (animator.IsSurfing || isJumpingToWater)
            yield break;

        yield return DialogManager.i.ShowDialogText($"�� ���� ��� ���δ�.");

        var pokemonWithSurf = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "�ĵ�Ÿ��"));

        if (pokemonWithSurf != null)
        {
            int selectedChoice = 0;
            yield return DialogManager.i.ShowDialogText($"{pokemonWithSurf.Base.Name}(��)�� �ĵ�Ÿ�⸦ ����Ͻðڽ��ϱ�?",
                choices: new List<string>() { "Yes", "No" },
                onChoiceSelected: (selection) => selectedChoice = selection);

            if (selectedChoice == 0)
            {
                //Yes
                yield return DialogManager.i.ShowDialogText($"{pokemonWithSurf.Base.Name}(��)�� �ĵ�Ÿ�⸦ ����Ͽ����ϴ�");

                var dir = new Vector3(animator.MoveX, animator.MoveY, 0);
                var targetPos = initiator.position + dir;

                isJumpingToWater = true;
                yield return initiator.DOJump(targetPos, 0.3f, 1, 0.5f).WaitForCompletion();
                isJumpingToWater = false;
                animator.IsSurfing = true;
            }

        }
    }

    public void OnPlayerTriggered(PlayerController player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 5)
        {
            GameController.i.StartBattle(BattleTrigger.Water);
        }
    }
}
