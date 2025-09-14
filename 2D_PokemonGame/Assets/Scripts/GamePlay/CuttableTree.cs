using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableTree : MonoBehaviour, Interactable
{
    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogManager.i.ShowDialogText($"�� ������ ���� �ڸ��� ������ ����...");

        var pokemonWithCut = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Cut"));
    
        if(pokemonWithCut != null)
        {
            int selectedChoice = 0;
            yield return DialogManager.i.ShowDialogText($"{pokemonWithCut.Base.Name}(��)�� �ڸ��⸦ ����Ͻðڽ��ϱ�?",
                choices: new List<string>() { "Yes", "No" },
                onChoiceSelected: (selection) => selectedChoice = selection);

            if(selectedChoice == 0)
            {
                //Yes
                yield return DialogManager.i.ShowDialogText($"{pokemonWithCut.Base.Name}(��)�� �ڸ��⸦ ����Ͽ����ϴ�");
                gameObject.SetActive(false);
            }
            
        }
    }
}
