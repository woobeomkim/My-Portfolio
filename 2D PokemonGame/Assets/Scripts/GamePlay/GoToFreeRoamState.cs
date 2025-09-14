using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToFreeRoamState : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
        GameController.i.StateMachine.ChangeState(FreeRoamState.i);
    }
}
