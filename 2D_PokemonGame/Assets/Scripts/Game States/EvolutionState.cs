using GDEUtils.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionState : State<GameController>
{
    [SerializeField] GameObject evolutionUI;
    [SerializeField] Image pokemonImage;

    [SerializeField] AudioClip evolutionMusic;

    public static EvolutionState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    public IEnumerator Evolve(Pokemon pokemon, Evolution evolution)
    {
        var gc = GameController.i;

        gc.StateMachine.Push(this);

        evolutionUI.SetActive(true);

        AudioManager.i.PlayMusic(evolutionMusic);

        pokemonImage.sprite = pokemon.Base.FrontSprite;
        yield return DialogManager.i.ShowDialogText($"{pokemon.Base.Name}(��)�� ��ȭ�ϰ��ִ����̴�...");

        var oldPokemon = pokemon.Base;
        pokemon.Evolve(evolution);

        pokemonImage.sprite = evolution.EvolvesInto.FrontSprite;
        yield return DialogManager.i.ShowDialogText($"{oldPokemon.Name}(��)�� {pokemon.Base.Name}���� ��ȭ�߽��ϴ�!");

        evolutionUI.gameObject.SetActive(false);

        gc.PartyScreen.SetPartyData();
        AudioManager.i.PlayMusic(gc.CurrentScene.SceneMusic, fade: true);

        gc.StateMachine.Pop();
    }
}
