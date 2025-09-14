using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Move
{
    MoveBase moveBase;
    int pp;

    public MoveBase Base => moveBase;

    public int PP { get { return pp; } set { pp = value; } }
    public Move(MoveBase moveBase)
    {
        this.moveBase = moveBase;
        pp = moveBase.PP;
    }

    public Move(MoveSaveData saveData)
    {
        moveBase = MoveDB.GetObjectByName(saveData.name);
        PP = saveData.pp;
    }

    public MoveSaveData GetSaveData()
    {
        var saveData = new MoveSaveData()
        {
            name = moveBase.Name,
            pp = PP
        };

        return saveData;
    }

    public void IncreasePP(int amount)
    {
        pp = Mathf.Clamp(pp + amount, 0, Base.PP);
    }
}


[System.Serializable]
public class MoveSaveData
{
    public string name;
    public int pp;
}