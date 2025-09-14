using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttableTree : MonoBehaviour, Interactable
{
    public IEnumerator Interact(Transform initiator)
    {
        yield return DialogManager.i.ShowDialogText($"이 나무는 왠지 자를수 있을것 같다...");

        var pokemonWithCut = initiator.GetComponent<PokemonParty>().Pokemons.FirstOrDefault(p => p.Moves.Any(m => m.Base.Name == "Cut"));
    
        if(pokemonWithCut != null)
        {
            int selectedChoice = 0;
            yield return DialogManager.i.ShowDialogText($"{pokemonWithCut.Base.Name}(이)가 자르기를 사용하시겠습니까?",
                choices: new List<string>() { "Yes", "No" },
                onChoiceSelected: (selection) => selectedChoice = selection);

            if(selectedChoice == 0)
            {
                //Yes
                yield return DialogManager.i.ShowDialogText($"{pokemonWithCut.Base.Name}(이)가 자르기를 사용하였습니다");
                gameObject.SetActive(false);
            }
            
        }
    }
}
