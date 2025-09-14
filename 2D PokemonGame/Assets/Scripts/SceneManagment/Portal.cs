using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, iPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestnationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        player.Character.Animator.IsMoving = false;
        StartCoroutine(SwitchScene());
    }

    Fader fader;
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        GameController.i.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var destPos = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPos.SpawnPoint.position);

        yield return fader.FadeOut(0.5f);
        GameController.i.PauseGame(false);
        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;

    public bool TriggerRepeatedly => false;
}

public enum DestnationIdentifier
{
    A,B,C,D,E
}