using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentGesture : MonoBehaviour, IGestureType
{

    public bool GesturesEnabled { get; set; } = true;
    public abstract void ExecuteEvent();
    public abstract bool MeetsTriggerSpeed(Vector3 velocity);
}
