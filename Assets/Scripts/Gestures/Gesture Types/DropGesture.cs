using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGesture : ComponentGesture
{
    public StereoRail_AudioManager.DropColor colorOfDrop;
    public float minTriggerSpeed = 0;

    private int windupCounter = 0;
    private const int WindupsTillDrop2 = 8;
    private const int WindupsTillDrop3 = 16;

    private StereoRail_AudioManager audioManager;
    private PanelManager parentManager;

    private void OnNewMeasure(MusicState state)
    {
        //windupCounter++;
        //Debug.Log("windup counter is at: " + windupCounter);
    }

    private void Start()
    {
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
    }

    public override void ExecuteEvent()
    {
        if (audioManager == null)
        {
            audioManager = StereoRail_AudioManager.Instance;
        }

        if (parentManager == null)
        {
            GameObject myParent = transform.parent.gameObject;
            parentManager = myParent.GetComponent<PanelManager>();
        }

        // Trigger the drop and fade out this panel's manager
        audioManager.TriggerDrop(colorOfDrop, audioManager.nextDropLength);
        //parentManager.FadeOutPanelHolder(gameObject);
        parentManager.DeleteAllPanels();
        Debug.Log("Sent Drop Trigger, color is: " + colorOfDrop);
    }

    /*
    private int GetNextDropLength()
    {
        if (windupCounter >= WindupsTillDrop3)
        {
            return 32;
        }
        else if (windupCounter >= WindupsTillDrop2)
        {
            return 16;
        }
        else
        {
            return 8;
        }
    }
    */

    public override bool MeetsTriggerSpeed(Vector3 velocity)
    {
        return velocity.magnitude >= minTriggerSpeed;
    }
}
