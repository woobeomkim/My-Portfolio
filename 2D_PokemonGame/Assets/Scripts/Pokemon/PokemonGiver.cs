using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonGiver : MonoBehaviour,ISavable
{
    [SerializeField] Pokemon pokemonToGive;
    [SerializeField] Dialog dialog;

    bool used = false;

    public IEnumerator GivePokemon(PlayerController player)
    {
        yield return DialogManager.i.ShowDialog(dialog);

        pokemonToGive.Init();

        player.GetComponent<PokemonParty>().AddPokemon(pokemonToGive);

        used = true;

        AudioManager.i.PlaySfx(AudioID.PokemonObtain, true);

        string dialogText = $"{player.Name}(��)�� {pokemonToGive.Base.Name}�� �޾Ҵ�!";
       
        yield return DialogManager.i.ShowDialogText(dialogText);
    }

    public bool CanBeGiven()
    {
        return pokemonToGive != null && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}
