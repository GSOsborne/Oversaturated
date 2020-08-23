using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    //public AudioClip[] drops;
    //public AudioSource dropSource;

    private StereoRail_AudioManager audioManager;

    private int dropLength;
    private int dropCounter;

    float quickTimer;
    //so I can see which drop is playing


    // Start is called before the first frame update
    void Start()
    {
        audioManager = StereoRail_AudioManager.Instance;
        if (!audioManager)
        {
            throw new System.Exception("No StereoRail_AudioManager instance found by DropManager");
        }

        TriggerDropEvent += InitiateDrop;

        //projectMeasureLength = audioManager.measureLength;
    }

    private void OnDestroy()
    {
        TriggerDropEvent -= InitiateDrop;
        NewMeasureEvent -= OnNewMeasure;
    }
    /*
    [System.Obsolete("No longer supported, as TriggerDropEvent listeners should be used instead")]
    public void DropMeasure(int dropMeasure, int dropLength, DropColor colorOfDrop, AnimationParcels animationParcels){
        if (dropMeasure == 1){
            //ChooseADropAndPlay();
            if (colorOfDrop == DropColor.Red)
            {
                AkSoundEngine.SetState("DropColor", "Red");
            }
            if (colorOfDrop == DropColor.Green)
            {
                AkSoundEngine.SetState("DropColor", "Green");
            }
            if (colorOfDrop == DropColor.Blue)
            {
                AkSoundEngine.SetState("DropColor", "Blue");
                animationParcels.BlueDropStart(dropLength);
            }
            if (colorOfDrop == DropColor.Yellow)
            {
                AkSoundEngine.SetState("DropColor", "Yellow");
            }
            if (colorOfDrop == DropColor.Purple)
            {
                AkSoundEngine.SetState("DropColor", "Purple");
            }
            AkSoundEngine.PostEvent("DropPlay", this.gameObject);


        }
    }
    */

    private void InitiateDrop(DropColor color, int length)
    {

        //Debug.Log("Initiating the Drop!");
        //ChooseADropAndPlay();
        switch (color)
        {
            case DropColor.Red:
                AkSoundEngine.SetState("DropColor", "Red");
                break;
            case DropColor.Green:
                AkSoundEngine.SetState("DropColor", "Green");
                break;
            case DropColor.Blue:
                AkSoundEngine.SetState("DropColor", "Blue");
                break;
            case DropColor.Orange:
                AkSoundEngine.SetState("DropColor", "Orange");
                break;
            case DropColor.Yellow:
                AkSoundEngine.SetState("DropColor", "Yellow");
                break;
            case DropColor.Purple:
                AkSoundEngine.SetState("DropColor", "Purple");

                // TODO: Move animation parcels to some animation manager object or animation event system
                //animationParcels.BlueDropStart(dropLength);
                break;
        }

        AkSoundEngine.PostEvent("DropPlay", gameObject);
        //Debug.Log("Playing the " + color + " drop!");

        dropCounter = 0;
        dropLength = length;
        Debug.Log("This drop should last for " + dropLength + " measures!");
        NewMeasureEvent += OnNewMeasure;
    }

    private void OnNewMeasure(MusicState state)
    {
        if (state == MusicState.Drop)
        {
            dropCounter++;
        }
        if (dropCounter >= dropLength)
        {
            Debug.Log("Ending the drop now after " + dropCounter + " measures. So sad.");
            NewMeasureEvent -= OnNewMeasure;
            dropCounter = 0;
            dropLength = 8;
            audioManager.EndDrop();
        }
    }

    /*
    void ChooseADropAndPlay(){
        //gets called when the drop starts
        whichDrop = Random.Range(0, drops.Length);
        Debug.Log(whichDrop);
        dropSource.clip = drops[whichDrop];
        dropSource.Play();

    }
    */

        /*
    void StopTheDrop(){

        dropSource.Stop();
    }
    */
}
