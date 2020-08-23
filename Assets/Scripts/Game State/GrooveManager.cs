using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class GrooveManager : MonoBehaviour
{

    bool startOfGroove;

    public bool mediumDropCounter;

    // Start is called before the first frame update
    void Start()
    {
        //listen to new measure event and also drop event
        NewMeasureEvent += OnNewMeasure;
        TriggerDropEvent += DropCalled;

        mediumDropCounter = false;
    }

    private void OnDestroy()
    {
        TriggerDropEvent -= DropCalled;
        NewMeasureEvent -= OnNewMeasure;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnNewMeasure(MusicState state)
    {
        switch (state)
        {
            case MusicState.Windup:
            case MusicState.Filler:
            case MusicState.Drop:
                startOfGroove = true;
                break;
            case MusicState.Groove:
                if (startOfGroove)
                {
                    AkSoundEngine.PostEvent("GruvPlay", gameObject);
                    Debug.Log("Should be starting the groove now.");
                }
            
                startOfGroove = false;
                break;
        }
    }

    private void DropCalled(DropColor color, int length)
    {
        if (length == 8) //short drop should not be followed by intro
        {
            AkSoundEngine.SetState("GrooveType", "Flow");
        }
        else if (length == 16) //this is the medium drop, every other medium drop should be followed by intro
        {
            if (!mediumDropCounter)
            {
                mediumDropCounter = true;
                AkSoundEngine.SetState("GrooveType", "Flow");
            }
            else
            {
                AkSoundEngine.SetState("GrooveType", "Intro");
                mediumDropCounter = false;
            }
        }
        else if (length == 32) //this is the long drop, should always be followed by intro
        {
            AkSoundEngine.SetState("GrooveType", "Intro");
            mediumDropCounter = false;
        }
    }


    //this stuff obsolete, left over from unity audio integration
    public void GrooveMeasure(){

        AkSoundEngine.PostEvent("GruvPlay", this.gameObject);


    }

    void PlayNextGroove(){
       
        /*
          grooveSource.clip = grooves[whichGroove];
        whichGroove++;
        if (whichGroove == grooves.Length){
            whichGroove = 0;
        }
        grooveSource.Play();

*/

    }

    public void StopTheGroove(){

        /*
        grooveSource.Stop();
        whichGroove++;
        if (whichGroove == grooves.Length)
        {
            whichGroove = 0;
        }
        measureCounter = 0;
        */      

    }
}
