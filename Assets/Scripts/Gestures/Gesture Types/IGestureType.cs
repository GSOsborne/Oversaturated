using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGestureType
{
    void ExecuteEvent();

    bool MeetsTriggerSpeed(Vector3 velocity);
}
