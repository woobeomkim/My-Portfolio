using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Longgrass : MonoBehaviour, iPlayerTriggerable
{
    public bool TriggerRepeatedly => true;

    public void OnPlayerTriggered(PlayerController player)
    {
        if (UnityEngine.Random.Range(1, 101) <= 10)
        {
            player.Character.Animator.IsMoving = false;
            GameController.i.StartBattle(BattleTrigger.LongGrass);
        }
    }
}
