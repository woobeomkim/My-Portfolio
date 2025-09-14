using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Achive { UnlockPotato, UnlockBean}

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    public Achive[] achives;
    WaitForSecondsRealtime wait;
    private void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))
            Init();
    }

    private void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }

    }

    private void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    private void LateUpdate()
    {
        foreach (Achive achive in achives)
            CheckAchive(achive);
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        switch(achive)
        {
            case Achive.UnlockPotato:
                if(GameManager.i.isLive)
                    isAchive = GameManager.i.kill >= 10;
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.i.gameTime == GameManager.i.maxGameTime;
                break;
        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) 
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;

                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRotuin());
        }

    }

    IEnumerator NoticeRotuin()
    {
        uiNotice.gameObject.SetActive(true);
        AudioManager.i.PlaySfx(Sfx.LevelUp);

        yield return wait;
        uiNotice.gameObject.SetActive(false);
    }
}
