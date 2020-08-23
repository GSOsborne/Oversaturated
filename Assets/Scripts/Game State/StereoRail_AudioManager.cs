using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AK.Wwise;

public class StereoRail_AudioManager : MonoBehaviour {

    public enum SongName
    {
        NA, PaperclipSwarm
    }

    public enum DropColor
    {
        Red, Blue, Green, Orange, Yellow, Purple
    }

    public static event System.Action<MusicState> NewMeasureEvent;

    public static StereoRail_AudioManager Instance { get; private set; }
    public static event System.Action<SongName> StartSongEvent;
    public static event System.Action StopSongEvent;
    // Color of drop, length of drop
    public static event System.Action<DropColor, int> TriggerDropEvent;
    public static event System.Action WindupGestureRecieved;
    public static event System.Action DropGestureRecieved;


    public AK.Wwise.Event startLevelEvent;

    public float measureLength;
    public float measureOffsetTime;
    //windup leeway time
    public float windupForgivenessTime;
    bool canAddChainedWindup;
    public bool addChainedWindup;
    public bool tutorialPreventingDrop;
    public bool tutorialActive;

    public int theWindupCounter;

    //public AnimationParcels animationParcels;

    //public float timer;
    public MusicState currentState;
    public MusicState nextState;

    public Text currentStateText;
    public Text nextStateText;

    //public int dropCounter;
    public int nextDropLength = 8;

    public DropColor colorOfDrop;

    public GameObject dropSelectPrefab;
    public GameObject cameraParent;

    int measuringTempo = 2;

    // TODO: Make all of these function on an event-based system
    // Objects for the Drops, Windups, Fillers, and Grooves
    public DropManager dropManager;
    public GrooveManager grooveManager;
    public WindupManager windupManager;
    public FillerManager fillerManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

    }
    // Use this for initialization
    void Start ()
    {
        StartSongEvent += ChooseSongToStart;
        StopSongEvent += StoppingSongMusic;
   

        measuringTempo = 2;
        //measureLength = 0;

        //timer = 0f + offsetMeasureLength;
        addChainedWindup = false;
        tutorialPreventingDrop = false;
        tutorialActive = false;
        theWindupCounter = 0;

    }

    private void OnDestroy()
    {
        StartSongEvent -= ChooseSongToStart;
        StopSongEvent -= StoppingSongMusic;
    }

    public static void StopThisSong()
    {
        StopSongEvent?.Invoke();
    }

    public void DropGestureRecievedCall()
    {
        DropGestureRecieved?.Invoke();
        Debug.Log("DropGestureRecieved!");
    }

    void StoppingSongMusic()
    {
        AkSoundEngine.PostEvent("StopSong", gameObject);
    }

    public static void StartASong(SongName whichSongName)
    {
        StartSongEvent?.Invoke(whichSongName);
    }

    //this one is just to help using canvas for debugging
    //so we don't have to trigger the shape for the music to play
    public void DebugStartPaperclipSwarm()
    {
        StartSongEvent?.Invoke(SongName.PaperclipSwarm);
    }


    private void ChooseSongToStart(SongName song)
    {
        switch (song)
        {
            case SongName.PaperclipSwarm:
                startLevelEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBar, DelayMeasureCheck);
                break;
            default:
                break;

        }

        currentState = MusicState.Groove;
        currentStateText.text = "Current State: Groove";
        nextState = MusicState.Groove;
        nextStateText.text = "Next State: Groove";

        //startLevelEvent.Post(this.gameObject, (uint)AkCallbackType.AK_MusicSyncBar, DelayMeasureCheck);
        grooveManager.GrooveMeasure();
        Debug.Log("Started Level Music ideally");
    }

    //-----------THE BUSINESS---------------

    void DelayMeasureCheck(object in_cookie, AkCallbackType in_type, object in_info)
    {

        //measureLength = AkMusicSyncCallbackInfo;

        StartCoroutine(DelayMeasureCheckCoroutine());
    } 

    IEnumerator MeasureLengthCoroutine()
    {
        while (measuringTempo == 1)
        {
            measureLength = measureLength + .01f;
            yield return new WaitForSeconds(.01f);
        }
 
    }

    IEnumerator DelayMeasureCheckCoroutine()
    {
        //Debug.Log("measure delay activated!");
        yield return new WaitForSeconds(measureLength - measureOffsetTime);
        EveryMeasureCheck();
    }

    void EveryMeasureCheck()
    {
        //Debug.Log("MeasureCheck!");
        //This is the stuff that happens if the state is changing
        if (nextState != currentState)
        {
            //Debug.Log("Transitioning!");

            switch(nextState)
            {
                case MusicState.Windup:
                    {
                        currentState = MusicState.Windup;

                        //and do a random animation
                        //animationParcels.RandomAnimationSelect();

                        currentStateText.text = "Current State: Windup";

                        if (addChainedWindup)
                        {
                            nextState = MusicState.Windup;
                            nextStateText.text = "Next State: Windup (because of forgiveness)";
                        }
                        else
                        {
                            //resetting the nextState, because windups only last one measure until they need refresh
                            nextState = MusicState.Filler;
                            nextStateText.text = "Next State: Filler";
                        }


                        //stop the groove. This should only trigger the first time the windup is triggered.
                        // TODO: The events are coming for your job, StereoRail!
                        grooveManager.StopTheGroove();
                        //stop the riser transition lead-in
                        //AkSoundEngine.PostEvent("QuitRiserLeadIn", gameObject);

                        //start the timer for the forgiveness windup
                        addChainedWindup = false;
                        StartCoroutine(WindupLeewayCheck());
                        break;
                    }
                case MusicState.Filler:
                    {

                        //play the filler measure
                        //fillerManager.FillerMeasure();

                        //fillers should only ever last one measure
                        //default should be going back to the groove
                        currentState = MusicState.Filler;
                        nextState = MusicState.Groove;
                        //Debug.Log("Playing Filler Measure");
                        currentStateText.text = "Current State: Filler";
                        nextStateText.text = "Next State: Groove";
                        break;
                    }
                case MusicState.Drop:
                    {
                        //Play that drop! Need to figure out a way to make sure it just keeps playing rather than
                        //triggering everytime
                        //Debug.Log("Playing DROP! measure #" + dropCounter);
                        currentStateText.text = "Current State: DROP";
                        currentState = MusicState.Drop;

                        TriggerDropEvent?.Invoke(colorOfDrop, nextDropLength);
                        Debug.Log("Called TriggerDropEvent, color: " + colorOfDrop);

                        // The actual call to the drop object for compartmentalization
                        //dropManager.DropMeasure(dropCounter, currentDropLength, colorOfDrop, animationParcels);
                        break;
                    }
                case MusicState.Groove:
                    {
                        currentState = MusicState.Groove;
                        //Debug.Log("Playing the groove");
                        nextStateText.text = "Next State: Groove";
                        currentStateText.text = "Current State: Groove";

                        // TODO: Events oughtta be here too
                        //call the groove measure!
                        //grooveManager.GrooveMeasure();
                        break;
                    }
                default:
                    {
                        nextState = MusicState.Groove;
                        nextStateText.text = "Next State: Groove";
                        currentStateText.text = "Current State: Groove";
                        Debug.Log("You didn't have a next state");
                        break;
                    }
            }
        }

        // This is what happens if the state hasn't changed. Drop continues, Groove Continues, Windup keeps going up. Filler should only get here if the drop overreaches the charge time into the next measure.
        else {
            //Debug.Log("Nothing's changed, you're still in: " + currentState);

            switch (nextState)
            {
                
                case MusicState.Drop:
                    {
                        //This is all now being handled by the drop Manager
                        //Debug.Log("Still playing the drop, stay tuned and have fun!");
                        
                        break;
                    }
                    
                case MusicState.Windup:
                    {
                        // TODO: Move this as well
                        //animationParcels.RandomAnimationSelect();

                        if (addChainedWindup)
                        {
                            nextState = MusicState.Windup;
                            nextStateText.text = "Next State: Windup (because of forgiveness)";
                            addChainedWindup = false;
                        }
                        else
                        {
                            //resetting the nextState, because windups only last one measure until they need refresh
                            nextState = MusicState.Filler;
                            nextStateText.text = "Next State: Filler";
                        }
                        //start the timer for the forgiveness windup
                        canAddChainedWindup = false;
                        StartCoroutine(WindupLeewayCheck());
                        break;
                    }
                case MusicState.Groove:
                    {
                        // TODO: More events baby!
                        //Do nothing, both nextState and currentState are Groove, just keep messing with DSP.
                        //grooveManager.GrooveMeasure();
                        break;
                    }
                case MusicState.Filler:
                    {
                        currentState = MusicState.Filler;
                        nextState = MusicState.Groove;
                        //Debug.Log("Playing Filler Measure");
                        currentStateText.text = "Current State: Filler";
                        nextStateText.text = "Next State: Groove";

                        Debug.Log("We're chaining fillers together for weird drop selection shenanigans.");
                        break;
                    }
                default:
                    {
                        Debug.Log("Something went wrong, you're not supposed to see this");
                        break;
                    }
            }
        }

        NewMeasureEvent?.Invoke(currentState);
    }

    // Ends the drop, if it's current in progress
    public void EndDrop()
    {
        if (currentState == MusicState.Drop)
        {
            // TODO: Have this set by DropManager
            //time to go back to the groove
            nextState = MusicState.Groove;
            nextStateText.text = "Next State: Groove";
            //Debug.Log("Thats the end of the drop.");
            //gotta reset the drop length, they gotta get a combo again
            nextDropLength = 8;

            Debug.Log("We made it to the EndDrop() method!");
        }
    }

    //---------------END OF THE BUSINESS------------------

    public void TriggerDrop(DropColor color, int dropLength)
    {
        bool canYouDrop;
        Debug.Log("Recieved request to Trigger the Drop!");
        //gotta check if the timing is right, can't trigger the drop in the middle of groove or another drop
        if (!tutorialPreventingDrop)
        {
            switch (currentState)
            {
                case MusicState.Windup:
                case MusicState.Filler:
                    canYouDrop = true;
                    //Debug.Log("Drop should be a-ok to proceed!");
                    break;
                default:
                    canYouDrop = false;
                    Debug.Log("The form is preventing you from dropping!");
                    
                    break;
            }
        }
        else
        {
            canYouDrop = false;

            Debug.Log("The tutorial is preventing you from dropping!");
        }

        //if you can drop, do it, otherwise you cant
        if (canYouDrop)
        {
            nextDropLength = dropLength;

            

            colorOfDrop = color;
            nextState = MusicState.Drop;
            Debug.Log("Drop incoming! Color is: " + color);
            nextStateText.text = "Next State: DROP";

            //and broadcast the reception of the drop gesture
            DropGestureRecieved?.Invoke();
        }
        else{
            Debug.Log("You can't drop it yet!");
        }
    }

    #region

    //all for using with the canvas to debug the music, don't use anywhere else
    public void DebugTriggerDropBlue()
    {
        TriggerDrop(DropColor.Blue, nextDropLength);
    }
    public void DebugTriggerDropRed()
    {
        TriggerDrop(DropColor.Red, nextDropLength);
    }
    public void DebugTriggerDropGreen()
    {
        TriggerDrop(DropColor.Green, nextDropLength);
    }
    public void DebugTriggerDropOrange()
    {
        TriggerDrop(DropColor.Orange, nextDropLength);
    }
    public void DebugTriggerDropYellow()
    {
        TriggerDrop(DropColor.Yellow, nextDropLength);
    }
    public void DebugTriggerDropPurple()
    {
        TriggerDrop(DropColor.Purple, nextDropLength);
    }
    //end of debug drop methods
    #endregion

    public void TriggerWindup(){
        //Debug.Log("I've recieved a request to windup!");
        bool canYouWindup;
        //gotta check if the timing is right, can't trigger the windup in the middle of drop... or if the drop has already been triggered

        switch (currentState)
        {
            case MusicState.Windup:
            case MusicState.Filler:
            case MusicState.Groove:
                if (nextState == MusicState.Drop)
                {
                    canYouWindup = false;
                }
                else
                {
                    canYouWindup = true;
                }
                break;
            default:
                canYouWindup = false;
                break;
        }

        //alright, lets do it if you can
        if (canYouWindup){
            nextState = MusicState.Windup;
            //Debug.Log("So I've got a Windup Incoming");
            nextStateText.text = "Next State: Windup";
            if (canAddChainedWindup)
            {
                addChainedWindup = true;
            }
            //and invoke the event so everyone knows its been triggered
            WindupGestureRecieved?.Invoke();            
        }
        else{
            Debug.Log("But unfortunately you can't windup right now");
        }
    }

    public void TriggerFiller()
    {
        nextState = MusicState.Filler;
        Debug.Log("Next state will be filler!");
    }
    public void TriggerGroove()
    {
        nextState = MusicState.Groove;
        Debug.Log("Going back to the groove.");
    }

    public void TriggerDropStinger()
    {
        Debug.Log("Stinger triggered!");
        AkSoundEngine.PostEvent("DropStinger", gameObject);
    }

    public void SetGrooveIntro()
    {
        AkSoundEngine.SetState("GrooveType", "Intro");
    }

    public void SetGrooveFlow()
    {
        AkSoundEngine.SetState("GrooveType", "Flow");
    }

    IEnumerator WindupLeewayCheck()
    {
        yield return new WaitForSeconds(measureLength - windupForgivenessTime);
        if (!tutorialActive)
        {
        canAddChainedWindup = true;
        }
    }
}
