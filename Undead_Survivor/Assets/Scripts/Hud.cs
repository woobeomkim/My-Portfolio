using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public enum InfoType { Exp,Level,Kill,Time,Health}

public class Hud : MonoBehaviour
{
    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.i.exp;
                float nextExp = GameManager.i.nextExp[Mathf.Min(GameManager.i.level, GameManager.i.nextExp.Length - 1)];
                mySlider.value = curExp / nextExp;
                break;
            case InfoType.Level:
                myText.text = $"Lv.{GameManager.i.level}";
                break;
            case InfoType.Kill:
                myText.text = $"kill : {GameManager.i.kill}";
                break;
            case InfoType.Time:
                float remainTime = GameManager.i.maxGameTime - GameManager.i.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = $"{min:D2}:{sec:D2}";
                break;
            case InfoType.Health:
                float curHealth = GameManager.i.health;
                float maxHealth = GameManager.i.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
