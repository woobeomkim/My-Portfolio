using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iPlayerTriggerable 
{
    void OnPlayerTriggered(PlayerController player);

    bool TriggerRepeatedly { get; }
}
