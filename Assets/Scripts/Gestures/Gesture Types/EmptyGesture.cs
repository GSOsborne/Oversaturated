using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A mere platitude! Represents the lack of a gesture. 
/// </summary>
public class EmptyGesture : IGestureType
{
    public void ExecuteEvent()
    {
        return;
    }

    public bool MeetsTriggerSpeed(Vector3 velocity)
    {
        return true;
    }
}
