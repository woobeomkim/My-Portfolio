using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable,ISavable
{
    [SerializeField] string name;
    [SerializeField] int battleUnitCount = 1;
    [SerializeField] Sprite sprite;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAtfterBattle;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;

    [SerializeField] AudioClip trainerAppearsClip;

    bool battleLost = false;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
    }

    private void Update()
    {
        character.HandleUpdate();
    }
    public IEnumerator Interact(Transform initiator)
    {
        character.LookTowards(initiator.position);

        if (!battleLost)
        {
            AudioManager.i.PlayMusic(trainerAppearsClip);

            yield return StartCoroutine(DialogManager.i.ShowDialog(dialog));
            GameController.i.StartTrainerBattle(this);
        }
        else
        {
            yield return DialogManager.i.ShowDialog(dialogAtfterBattle);
        }
    }

    public IEnumerator TriggerTriainerBattle(PlayerController player)
    {
        GameController.i.StateMachine.Push(CutsceneState.i);

        AudioManager.i.PlayMusic(trainerAppearsClip);

        // Show Exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        // walk toward player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;

        moveVec = new Vector3(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));
        yield return character.Move(moveVec);

        // start battle
        yield return DialogManager.i.ShowDialog(dialog);

        GameController.i.StateMachine.Pop();

        GameController.i.StartTrainerBattle(this);
    }

    public void BattleLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }

    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;
        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 180f;
        else if (dir == FacingDirection.Left)
            angle = 270f;

        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    public object CaptureState()
    {
        return battleLost;
    }

    public void RestoreState(object state)
    {
        battleLost = (bool)state;

        if (battleLost)
            fov.gameObject.SetActive(false);
    }

    public string Name => name;
    public Sprite Sprite => sprite;
    public int BattleUnitCount => battleUnitCount;
}
