using GDE.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : SelectionUI<TextSlot>
{
    [SerializeField] Text messageText;
    
    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;
    PokemonParty party;

    //int selection;
    //public Pokemon SelectedMember => pokemons[selection];
    public Pokemon SelectedMember => pokemons[selectedItem];

    /// <summary>
    /// 파티스크린은 액션셀렉션, 러닝턴,어바웃투유즈 세곳에서 불려질수있다
    /// </summary>
    //public BattleState? CalledFrom { get; set; }
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);
        SetSelectionSettings(SelectionType.Grid, 2);
        party = PokemonParty.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    public void SetPartyData()
    {
        pokemons = party.Pokemons;

        ClearItems();

        for(int i=0;i<memberSlots.Length;i++)
        {
            if (i < pokemons.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].Init(pokemons[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        var textSlots = memberSlots.Select(m => m.GetComponent<TextSlot>());
        SetItems(textSlots.Take(pokemons.Count).ToList());
        //UpdateMemberSelection(selection);

        messageText.text = "Choose a Pokemon";
    }

    //public void HandleUpdate(Action onSelected, Action onBack)
    //{
    //    var prevSelection = selection;

    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //        selection += 2;
    //    else if (Input.GetKeyDown(KeyCode.UpArrow))
    //        selection -= 2;
    //    else if (Input.GetKeyDown(KeyCode.RightArrow))
    //        selection += 1;
    //    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        selection -= 1;

    //    selection = Mathf.Clamp(selection, 0, pokemons.Count - 1);

    //    if(selection != prevSelection)
    //        UpdateMemberSelection(selection);

    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        onSelected?.Invoke();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        onBack?.Invoke();
    //    }
    //}

    //public void UpdateMemberSelection(int selectedMember)
    //{
    //    for(int i=0;i<pokemons.Count;i++)
    //    {
    //        if (i == selectedMember)
    //            memberSlots[i].SetSelected(true);
    //        else
    //            memberSlots[i].SetSelected(false);
    //    }
    //}

    public void ShowIfTmIsUsable(TmItem tmItem)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            string message = tmItem.CanBeTaugth(pokemons[i]) ? "배울 수 있습니다!" : "배울 수 없습니다!";
            memberSlots[i].SetMessage(message);
        }
    }

    public void ClearMemberSlotMessage()
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            memberSlots[i].SetMessage("");
        }
    }
    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
