using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] GameObject choiceBox;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    [SerializeField] Text yesText;
    [SerializeField] Text noText;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        for(int i=0;i<dialog.Length;i++)
        {
            dialogText.text += dialog[i];
            yield return new WaitForSeconds(1.0f / lettersPerSecond);
        }
        yield return new WaitForSeconds(1.0f);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.gameObject.SetActive(enabled);
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void EnableChoiceBox(bool enabled)
    {
        choiceBox.gameObject.SetActive(enabled);
    }


    public bool IsChoiceBoxEnabled => choiceBox.activeSelf;

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveTexts.Count; i++)
        {
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.Name;
            else
                moveTexts[i].text = "-";
        }
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for(int i=0; i<actionTexts.Count;i++)
        {
            if (i == selectedAction)
                actionTexts[i].color = GlobalSettings.i.HighlightedColor;
            else
                actionTexts[i].color = Color.black;
        }
    }


    public void UpdateChoiceBox(bool selected)
    {
        if(selected)
        {
            yesText.color = GlobalSettings.i.HighlightedColor;
            noText.color = Color.black;
        }
        else
        {
            yesText.color = Color.black;
            noText.color = GlobalSettings.i.HighlightedColor;
        }
    }
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for(int i=0;i<moveTexts.Count;i++)
        {
            if( i== selectedMove)
                moveTexts[i].color = GlobalSettings.i.HighlightedColor;
            else
                moveTexts[i].color = Color.black;
        }

        ppText.text = $"{move.PP}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();

        if (move.PP == 0)
            ppText.color = Color.red;
        else
            ppText.color = Color.black;
    }
}
