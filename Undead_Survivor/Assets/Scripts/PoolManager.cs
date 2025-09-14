using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] polls;

    private void Awake()
    {
        polls = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < polls.Length; i++)
        {
            polls[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int idx)
    {
        GameObject select = null;

        foreach (GameObject item in polls[idx])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }


        if(select == null)
        {
            select = Instantiate(prefabs[idx], gameObject.transform);
            polls[idx].Add(select);
        }

        return select;
    }
}
