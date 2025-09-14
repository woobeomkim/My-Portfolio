using GDE.GenericSelectionUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MoveToForgetSelectionUI : SelectionUI<TextSlot>
{
    [SerializeField] List<Text> moveTexts;

    //int currentSelection = 0;
    public void setMoveData(List<MoveBase> currentMoves, MoveBase newMove)
    {
        for(int i=0;i<currentMoves.Count;i++)
        {
            moveTexts[i].text = currentMoves[i].Name;
        }
        moveTexts[currentMoves.Count].text = newMove.Name;

        SetItems(moveTexts.Select(m => m.GetComponent<TextSlot>()).ToList());
    }

    //public void HandleMoveSelection(Action<int> onSelected)
    //{
    //    if (Input.GetKeyDown(KeyCode.DownArrow))
    //        ++currentSelection;
    //    else if (Input.GetKeyDown(KeyCode.UpArrow))
    //        --currentSelection;

    //    currentSelection = Mathf.Clamp(currentSelection, 0, PokemonBase.MaxNumOfMoves);

    //    UpdateMoveSelection(currentSelection);

    //    if(Input.GetKeyDown(KeyCode.Z))
    //    {
    //        onSelected?.Invoke(currentSelection);
    //    }

    //}

    //public void UpdateMoveSelection(int selection)
    //{
    //    for(int i=0;i<PokemonBase.MaxNumOfMoves+1;i++)
    //    {
    //        if (i == selection)
    //            moveTexts[i].color = Color.blue;
    //        else
    //            moveTexts[i].color = Color.black;
    //    }
    //}
}
