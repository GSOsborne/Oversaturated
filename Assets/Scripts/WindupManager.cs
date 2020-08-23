using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindupManager : MonoBehaviour
{
    private StereoRail_AudioManager audioManager;

    private int windupCounter = 0;
    bool dropLengthHasBeenReset;
    bool firstMeasureOfRiser;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = StereoRail_AudioManager.Instance;
        if (!audioManager)
        {
            throw new System.Exception("No StereoRail_AudioManager instance found by WindupManager");
        }
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.StopSongEvent += ResetFirstMeasureBool;
        dropLengthHasBeenReset = false;
        firstMeasureOfRiser = true;
        
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
        StereoRail_AudioManager.StopSongEvent -= ResetFirstMeasureBool;
    }

    void ResetFirstMeasureBool()
    {
        windupCounter = 0;
        firstMeasureOfRiser = true;
    }

    private void OnNewMeasure(MusicState state)
    {
        switch (state)
        {
            case MusicState.Windup:
                if (windupCounter == 0)
                {
                    StereoRail_AudioManager.Instance.nextDropLength = 8;
                }
                if (firstMeasureOfRiser)
                {
                    AkSoundEngine.PostEvent("RiserStart", this.gameObject);
                    Debug.Log("Should have started the riser this measure.");
                    firstMeasureOfRiser = false;
                }
                AkSoundEngine.PostEvent("WindupTrigger", this.gameObject);
                //Debug.Log("WindupTrigger Triggered");
                
                windupCounter++;

                //Debug.Log(windupCounter);
                //need to update droplength if the windup has been going on long enough
                if (windupCounter == 8)
                {
                    StereoRail_AudioManager.Instance.nextDropLength = 16;
                    Debug.Log("Set Next Drop Length to " + StereoRail_AudioManager.Instance.nextDropLength);
                    dropLengthHasBeenReset = false;
                }
                else if (windupCounter == 16)
                {
                    StereoRail_AudioManager.Instance.nextDropLength = 32;
                    Debug.Log("Set Next Drop Length to " + StereoRail_AudioManager.Instance.nextDropLength);
                }

                break;
            case MusicState.Filler:
                //removing this reset so that you can continue streak even after failing to keep riser going
                //windupCounter = 0;
                windupCounter++;
                firstMeasureOfRiser = true;
                break;
            case MusicState.Drop:
                if (!dropLengthHasBeenReset)
                {
                    windupCounter = 0;
                    StereoRail_AudioManager.Instance.nextDropLength = 8;
                    Debug.Log("Set Next Drop Length to " + StereoRail_AudioManager.Instance.nextDropLength);
                    dropLengthHasBeenReset = true;
                }

                firstMeasureOfRiser = true;
                break;
            case MusicState.Groove:
                windupCounter = 0;
                firstMeasureOfRiser = true;
                break;
        }
        audioManager.theWindupCounter = windupCounter;
        //Debug.Log(audioManager.theWindupCounter);
    }


    [System.Obsolete("This does absolutely zilch")]
    void PlayTheNextRiser()
    {
       
        /*
          if (!areWeInTheMiddleOfRiser){
            //riserSource.clip = firstHalfRisers[whichRiser];
            //riserSource.Play();
            if (whichRiser == firstHalfRisers.Length)
            {
                whichRiser = 0;
            }
            areWeInTheMiddleOfRiser = true;
        }
        else if (areWeInTheMiddleOfRiser){
            riserSource.clip = secondHalfRisers[whichRiser];
            riserSource.Play();
            whichRiser++;
            if (whichRiser == firstHalfRisers.Length)
            {
                whichRiser = 0;
            }
        }

    */

    }
}
