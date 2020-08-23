using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class GestureTutorial : MonoBehaviour
{
    public GameObject firstWindupEncouragementObject;
    public GameObject windupComboEncouragementObject;
    public GameObject dropSelectEncouragementObject;
    public InteractionMachine interactionMachine;
    public OptimizedPanelManager opPanMan;
    //public GameObject finalRemindersObject;

    bool stageOnePassed;
    bool windupComboStarted;
    bool windupComboOngoing;
    int windupCounter;
    public int chargeRequirement;

    // Start is called before the first frame update
    void Start()
    {
        firstWindupEncouragementObject.SetActive(false);
        dropSelectEncouragementObject.SetActive(false);
        windupComboEncouragementObject.SetActive(false);
        //finalRemindersObject.SetActive(false);

        stageOnePassed = false;
        windupComboStarted = false;
        windupComboOngoing = false;

        StartSongEvent += TutorialBehaviour;
        StopSongEvent += EndPSTutorial;
        StopSongEvent += DeleteReminderObject;


    }

    private void OnDestroy()
    {
        StartSongEvent -= TutorialBehaviour;
        NewMeasureEvent -= PaperclipSwarmTutorial;
        StopSongEvent -= EndPSTutorial;
        StopSongEvent -= DeleteReminderObject;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TutorialBehaviour(SongName songName)
    {
        switch (songName)
        {
            case SongName.PaperclipSwarm:
                stageOnePassed = false;
                windupComboOngoing = false;
                NewMeasureEvent += PaperclipSwarmTutorial;
                StereoRail_AudioManager.Instance.tutorialPreventingDrop = true;
                StereoRail_AudioManager.Instance.tutorialActive = true;
                break;
        }
    }

    void PaperclipSwarmTutorial(MusicState currentState)
    {
        if (currentState == MusicState.Windup || currentState == MusicState.Filler)
        {
            windupCounter++;
            //Debug.Log("Tutorial Windup Counter at: " + windupCounter);
            stageOnePassed = true;
            if(windupCounter >= chargeRequirement - 1)
            {
                StereoRail_AudioManager.Instance.tutorialPreventingDrop = false;

            }
            if (windupCounter >= chargeRequirement)
            {
                windupComboOngoing = true;
                //Debug.Log("You should be allowed to drop now, tutorial should let you!");
            }
            windupComboStarted = true;

        }
        else
        {
            windupCounter = 0;
            windupComboStarted = false;
            windupComboOngoing = false;
        }


        if (!stageOnePassed)
        {
            firstWindupEncouragementObject.SetActive(true);
            windupComboEncouragementObject.SetActive(false);
            dropSelectEncouragementObject.SetActive(false);
        }
        else if (windupComboOngoing)
        {
            
            firstWindupEncouragementObject.SetActive(false);
            windupComboEncouragementObject.SetActive(false);
            dropSelectEncouragementObject.SetActive(true);
            //Debug.Log("Hey, we're gonna go ahead and just start the interaction manually.");
            interactionMachine.BeginInteraction();
            opPanMan.SummonDropOptions();
            

        }
        else
        {
            if (currentState != MusicState.Drop)
            {
                firstWindupEncouragementObject.SetActive(false);
                dropSelectEncouragementObject.SetActive(false);
                windupComboEncouragementObject.SetActive(true);
            }

            if (!windupComboStarted && currentState == MusicState.Groove)
            {
                StereoRail_AudioManager.Instance.tutorialPreventingDrop = true;
            }
            
        }


        if (currentState == MusicState.Drop)
        {
            //finalRemindersObject.SetActive(true);
            EndPSTutorial();
        }
    }

    public void EndPSTutorial()
    {

        //Debug.Log("That's the end of the tutorial!");

        NewMeasureEvent -= PaperclipSwarmTutorial;
        StereoRail_AudioManager.Instance.tutorialActive = false;
        StereoRail_AudioManager.Instance.tutorialPreventingDrop = false;

        firstWindupEncouragementObject.SetActive(false);
        dropSelectEncouragementObject.SetActive(false);
        windupComboEncouragementObject.SetActive(false);

    }

    void DeleteReminderObject()
    {
        //finalRemindersObject.SetActive(false);
    }
}
