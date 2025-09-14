using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public AudioClip audioClip1;
    public AudioClip audioClip2;

    int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        //AudioSource audio = GetComponent<AudioSource>();
        //audio.PlayOneShot(audioClip);

        i++;

        if(i % 2 == 0)
        Managers.Sound.Play(audioClip1,Define.Sound.Bgm);
        else
        Managers.Sound.Play(audioClip2, Define.Sound.Bgm);

    }
}
