using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioreactiveDropAnim : MonoBehaviour
{
    public abstract void TriggerDropAnimation(StereoRail_AudioManager.DropColor dropColor, int dropLength);
}
