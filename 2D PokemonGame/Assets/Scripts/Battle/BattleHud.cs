using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text statusText;
    [SerializeField] HPBar hpBar;
    [SerializeField] GameObject expBar;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color slpColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;

    Pokemon _pokemon;

    Dictionary<StatusConditionID, Color> statusColors;
    public void SetData(Pokemon pokemon)
    {
        if(_pokemon != null)
        {
            _pokemon.OnStatusChanged -= SetStatusText;
            _pokemon.OnHPChanged -= UpdateHP;

        }

        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        SetLevel();
        hpBar.SetHP((float)pokemon.HP / pokemon.MaxHp);
        SetExp();

        statusColors = new Dictionary<StatusConditionID, Color>()
        {
            { StatusConditionID.psn , psnColor},
            { StatusConditionID.brn , brnColor},
            { StatusConditionID.slp , slpColor},
            { StatusConditionID.par , parColor},
            { StatusConditionID.frz , frzColor},
        };
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
        _pokemon.OnHPChanged += UpdateHP;
    }

    void SetStatusText()
    {
        if(_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[_pokemon.Status.Id];
        }
    }

    public void SetLevel()
    {
        levelText.text = "Lvl " + _pokemon.Level;
    }

    public void SetExp()
    {
        if (expBar == null)
            return;

        float normalizedExp = _pokemon.GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1f, 1f);
    }

    public IEnumerator SetExpSmooth(bool reset = false)
    {
        if (expBar == null)
            yield break;

        if (reset)
            expBar.transform.localScale = new Vector3(0, 1, 1);
        float normalizedExp = _pokemon.GetNormalizedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }

  

    public void UpdateHP()
    {
        StartCoroutine(UpdateHPAsync());
    }

    public IEnumerator UpdateHPAsync()
    {
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHp);
    }

    public IEnumerator WaitForHPUpdate()
    {
        yield return new WaitUntil(() => hpBar.IsUpdating == false);
    }

    public void ClearData()
    {
        if (_pokemon != null)
        {
            _pokemon.OnStatusChanged -= SetStatusText;
            _pokemon.OnHPChanged -= UpdateHP;

        }
    }
}
