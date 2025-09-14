using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectAction : CutsceneAction
{
    [SerializeField] GameObject go;

    public override IEnumerator Play()
    {
        go.gameObject.SetActive(true);
        yield break;
    }
}
