using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindupGesture : ComponentGesture
{
    // The minimum speed needed to trigger this gesture
    public float minTriggerSpeed = 0;

    private StereoRail_AudioManager audioManager;

    bool recentlyTriggered = false;

    private void Start()
    {
        recentlyTriggered = false;
    }

    public override void ExecuteEvent()
    {
        if (audioManager == null)
        {
            audioManager = StereoRail_AudioManager.Instance;
        }

        if (!recentlyTriggered)
        {
            audioManager.TriggerWindup();
            StartCoroutine(PreventMachineGun());


            //for debugging machine gun triggering 
            /*
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(Random.Range(-.2f, .2f), Random.Range(.1f, .8f), Random.Range(1f, 1.8f));
            cube.transform.localScale = new Vector3(.1f, .1f, .1f);
            */
        }
        
    }

    public override bool MeetsTriggerSpeed(Vector3 velocity)
    {
        return velocity.magnitude > minTriggerSpeed;
    }

    IEnumerator PreventMachineGun()
    {
        recentlyTriggered = true;
        yield return new WaitForSeconds(.2f);
        recentlyTriggered = false;
    }
}
