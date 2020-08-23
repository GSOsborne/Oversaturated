using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class InteractionMachine : MonoBehaviour
{
    #region Defining Variables
    //bottom panels - part of the main stage
    [Header ("Interaction Machine Panels (Parents)")]
    [Tooltip ("Populated on Start, no need to plug in prefabs")]
    public GameObject InteractionCenter;
    [Tooltip("Populated on Start, no need to plug in prefabs")]
    public GameObject InteractionLeft;
    [Tooltip("Populated on Start, no need to plug in prefabs")]
    public GameObject InteractionRight;

    //drop selectors
    [Header ("Drop Selectors")]
    public GameObject LeftDropSelect;
    public GameObject CenterDropSelect;
    public GameObject RightDropSelect;

    //DSP Panels
    [Header("DSP Boxes")]
    public GameObject LeftDSP;
    public GameObject CenterDSP;
    public GameObject RightDSP;

    //drop selector desired scales
    private Vector3 leftDropScale;
    private Vector3 centerDropScale;
    private Vector3 rightDropScale;

    //drop selector current scales
    private Vector3 currentDropScaleLeft;
    private Vector3 currentDropScaleCenter;
    private Vector3 currentDropScaleRight;

    //drop selector minimized scales
    private Vector3 minDropScaleLeft;
    private Vector3 minDropScaleCenter;
    private Vector3 minDropScaleRight;

    //animation variables
    [Header ("Animation Variables")]
    public float speed = 2f;
    public float machineHeight = 2f;
    public float dspHeight = 2.2f;

    //panel resting transforms
    private Vector3 restPosLeft;
    private Vector3 restPosCenter;
    private Vector3 restPosRight;

    //panel active offsets
    private Vector3 offset;
    private Vector3 activePosLeft;
    private Vector3 activePosCenter;
    private Vector3 activePosRight;

    //panel current offsets
    private Vector3 currentPosLeft;
    private Vector3 currentPosCenter;
    private Vector3 currentPosRight;

    //DSP current offsets
    private Vector3 dspCurrentPosLeft;
    private Vector3 dspCurrentPosCenter;
    private Vector3 dspCurrentPosRight;

    //DSP active offsets
    private Vector3 dspOffset;
    private Vector3 dspActivePosLeft;
    private Vector3 dspActivePosCenter;
    private Vector3 dspActivePosRight;

    //DSP resting positions
    private Vector3 dspRestPosLeft;
    private Vector3 dspRestPosCenter;
    private Vector3 dspRestPosRight;

    //machine states
    [Header ("Trigger States")]
    public bool isTriggeredLeft = false;
    public bool isTriggeredRight = false;
    public bool isTriggeredCenter = false;

    [Header ("Active (Max Height)")]
    public bool isActiveLeft = false;
    public bool isActiveRight = false;
    public bool isActiveCenter = false;

    [Header ("Return to Floor Position")]
    public bool deactivateLeft = false;
    public bool deactivateRight = false;
    public bool deactivateCenter = false;

    [Header ("Resting (Floor Position)")]
    public bool restingLeft = true;
    public bool restingRight = true;
    public bool restingCenter = true;

    [Header("DSP is Triggered")]
    public bool dspTriggeredLeft = false;
    public bool dspTriggeredCenter = false;
    public bool dspTriggeredRight = false;

    [Header("DSP Active")]
    public bool dspActiveLeft = false;
    public bool dspActiveCenter = false;
    public bool dspActiveRight = false;

    [Header("DSP Return to Rest Position")]
    public bool dspDeactivateLeft = false;
    public bool dspDeactivateCenter = false;
    public bool dspDeactivateRight = false;

    [Header("DSP Resting")]
    public bool dspRestingLeft = true;
    public bool dspRestingCenter = true;
    public bool dspRestingRight = true;
    #endregion

    bool songPlaying;

    private void Start()
    {
        songPlaying = false;
        StereoRail_AudioManager.StartSongEvent += SongIsPlaying;

        StereoRail_AudioManager.NewMeasureEvent += MeasureCheck;
        StereoRail_AudioManager.TriggerDropEvent += DropEventReceived;
        StereoRail_AudioManager.WindupGestureRecieved += BeginInteraction;
        StereoRail_AudioManager.StopSongEvent += ResetItAll;

        CenterDropSelect.SetActive(false);
        LeftDropSelect.SetActive(false);
        RightDropSelect.SetActive(false);

        #region Define Interaction Panels
        //find the Left Interaction Panel
        if (InteractionLeft == null)
        { 
        InteractionLeft = GameObject.FindWithTag("InteractionMachineLeft");
            restPosLeft = InteractionLeft.transform.position;
            offset = new Vector3(0, machineHeight, 0);
            activePosLeft = new Vector3(restPosLeft.x, restPosLeft.y + offset.y, restPosLeft.z);
        }

        //DSP offset calculation - Left
        dspRestPosLeft = LeftDSP.transform.position;
        dspOffset = new Vector3(0, dspHeight, 0);
        dspActivePosLeft = new Vector3(dspRestPosLeft.x, dspRestPosLeft.y + dspOffset.y, dspRestPosLeft.z);

        //find the Center Interaction Panel
        if (InteractionCenter == null)
        {
            InteractionCenter = GameObject.FindWithTag("InteractionMachineCenter");
            restPosCenter = InteractionCenter.transform.position;
            offset = new Vector3(0, machineHeight, 0);
            activePosCenter = new Vector3 (restPosCenter.x, restPosCenter.y + offset.y, restPosCenter.z);
        }

        //DSP offset calculation - Center
        dspRestPosCenter = CenterDSP.transform.position;
        dspOffset = new Vector3(0, dspHeight, 0);
        dspActivePosCenter = new Vector3(dspRestPosCenter.x, dspRestPosCenter.y + dspOffset.y, dspRestPosCenter.z);

        //find the Right Interaction Panel
        if (InteractionRight == null)
        {
            InteractionRight = GameObject.FindWithTag("InteractionMachineRight");
            restPosRight = InteractionRight.transform.position;
            offset = new Vector3(0, machineHeight, 0);
            activePosRight = new Vector3 (restPosRight.x, restPosRight.y + offset.y, restPosRight.z);
        }

        //DSP offset calculation - Right
        dspRestPosRight = RightDSP.transform.position;
        dspOffset = new Vector3(0, dspHeight, 0);
        dspActivePosRight = new Vector3(dspRestPosRight.x, dspRestPosRight.y + dspOffset.y, dspRestPosRight.z);
        #endregion

        #region Set Starting States
        isTriggeredLeft = false;
        isTriggeredRight = false;
        isTriggeredCenter = false;
        isActiveLeft = false;
        isActiveRight = false;
        isActiveCenter = false;
        deactivateLeft = false;
        deactivateRight = false;
        deactivateCenter = false;
        restingLeft = true;
        restingRight = true;
        restingCenter = true;
        dspActiveCenter = false;
        dspActiveLeft = false;
        dspActiveRight = false;
        dspRestingLeft = true;
        dspRestingCenter = true;
        dspRestingRight = true;
        dspDeactivateLeft = false;
        dspDeactivateCenter = false;
        dspDeactivateRight = false;
        dspTriggeredLeft = false;
        dspTriggeredCenter = false;
        dspTriggeredRight = false;
        #endregion

        #region Drop Select Starting States
        leftDropScale = LeftDropSelect.transform.localScale;
        minDropScaleLeft = leftDropScale / 100;
        currentDropScaleLeft = minDropScaleLeft;
        LeftDropSelect.SetActive(false);

        centerDropScale = new Vector3(50,50,50);
        minDropScaleCenter = centerDropScale / 100;
        //CenterSelection.transform.localScale = minScaleCenter;
        currentDropScaleCenter = CenterDropSelect.transform.localScale;
        CenterDropSelect.SetActive(false);

        rightDropScale = RightDropSelect.transform.localScale;
        minDropScaleRight = rightDropScale / 100;
        currentDropScaleRight = minDropScaleRight;
        RightDropSelect.SetActive(false);
        #endregion
    }

    void SongIsPlaying(SongName sName)
    {
        songPlaying = true;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= MeasureCheck;
        StereoRail_AudioManager.TriggerDropEvent -= DropEventReceived;
        StereoRail_AudioManager.WindupGestureRecieved -= BeginInteraction;
        StereoRail_AudioManager.StopSongEvent -= ResetItAll;
    }

    public void Update()
    {
        //dear god this code is atrocious. I (Travis) will hopefully be able to clean it up in the near future
        #region Center Panel Behaviors
        //all behaviors for the Center Panel

        //trigger the center panel from rest
        if (isTriggeredCenter == true)
        {
            restingCenter = false;
            isActiveCenter = false;
            deactivateCenter = false;
            currentPosCenter = InteractionCenter.transform.position;
            InteractionCenter.transform.position = Vector3.Lerp(currentPosCenter, activePosCenter, speed * Time.deltaTime);
            //Greg decided fuck it, see what happens if you just turn it on early, and was kinda ok with it tbh
            //CenterDropSelect.SetActive(true);
        }

        // set center panel to active, spawn drop selector
        if (isTriggeredCenter == true && activePosCenter.y - currentPosCenter.y <= .01)
        {
            isActiveCenter = true;
            restingCenter = false;
            deactivateCenter = false;
            isTriggeredCenter = false;
            currentPosCenter = activePosCenter;

            //scale and activate center selector
            //having issue scaling uniformally in the same way we did translation. Will do a research spike
            //CenterDropSelect.SetActive(true);
            CenterDropSelect.SetActive(false);
            //currentScaleCenter = minScaleCenter;
            //currentScaleCenter = CenterSelection.transform.localScale;
            //CenterSelection.transform.localScale = Vector3.Lerp(currentScaleCenter, currentScaleCenter * 100, speed * Time.deltaTime);
        }

        #region DSP Box - Center Panel
        //trigger center panel DSP box animation
        if (isActiveCenter == true && dspTriggeredCenter == true)
        {
            dspRestingCenter = false;
            dspActiveCenter = false;
            dspDeactivateCenter = false;
            dspCurrentPosCenter = CenterDSP.transform.position;
            //InteractionCenter.transform.position = Vector3.Lerp(currentPosCenter, activePosCenter, speed * Time.deltaTime);
            CenterDSP.transform.position = Vector3.Lerp(dspCurrentPosCenter, dspActivePosCenter, speed * Time.deltaTime);
        }

        //set central dsp to active
        if (dspTriggeredCenter == true && dspActivePosCenter.y - dspCurrentPosCenter.y <= .01)
        {
            dspActiveCenter = true;
            dspRestingCenter = false;
            dspDeactivateCenter = false;
            dspTriggeredCenter = false;
            dspCurrentPosCenter = dspActivePosCenter;
        }

        //deactivate center dsp, return to resting position
        if (dspDeactivateCenter == true)
        {
            dspRestingCenter = false;
            dspActiveCenter = false;
            dspTriggeredCenter = false;
            dspCurrentPosCenter = CenterDSP.transform.position;
            CenterDSP.transform.position = Vector3.Lerp(dspCurrentPosCenter, dspRestPosCenter, speed * Time.deltaTime);
        }

        //reset Center DSP to resting
        if (dspDeactivateCenter == true && Mathf.Abs(dspRestPosCenter.y - dspCurrentPosCenter.y) <= .01)
        {
            dspActiveCenter = false;
            dspTriggeredCenter = false;
            dspRestingCenter = true;
            dspDeactivateCenter = false;
            dspCurrentPosCenter = dspRestPosCenter;
        }
        #endregion
        // deactivate center panel, return to rest position
        if (deactivateCenter == true)
        {
            isActiveCenter = false;
            restingCenter = false;
            isTriggeredCenter = false;
            dspActiveCenter = false;
            dspTriggeredCenter = false;
            dspRestingCenter = false;
            dspDeactivateCenter = true;
            CenterDropSelect.SetActive(false);
            currentPosCenter = InteractionCenter.transform.position;
            InteractionCenter.transform.position = Vector3.Lerp(currentPosCenter, restPosCenter, speed * Time.deltaTime);
        }

        // reset panel to resting
        if (deactivateCenter == true && Mathf.Abs(restPosCenter.y - currentPosCenter.y) <= .01)
        {
            isActiveCenter = false;
            isTriggeredCenter = false;
            restingCenter = true;
            deactivateCenter = false;
            currentPosCenter = restPosCenter;
        }

        #endregion

        #region Left Panel Behaviors

        //trigger left pillar
        if (isTriggeredLeft == true)
        {
            restingLeft = false;
            isActiveLeft = false;
            deactivateLeft = false;
            currentPosLeft = InteractionLeft.transform.position;
            InteractionLeft.transform.position = Vector3.Lerp(currentPosLeft, activePosLeft, speed * Time.deltaTime);
            //Greg decided fuck it, see what happens if you just turn it on early, and was kinda ok with it tbh
            //LeftDropSelect.SetActive(true);
        }

        //set left pillar to active
        if (isTriggeredLeft == true && Mathf.Abs(activePosLeft.y - currentPosLeft.y) <= .01)
        {
            isActiveLeft = true;
            isTriggeredLeft = false;
            restingLeft = false;
            deactivateLeft = false;
            currentPosLeft = activePosLeft;
            //LeftDropSelect.SetActive(true);
            LeftDropSelect.SetActive(false);
        }

        #region DSP Box - Left Panel
        //trigger left panel DSP box animation
        if (isActiveLeft == true && dspTriggeredLeft == true)
        {
            dspRestingLeft = false;
            dspActiveLeft = false;
            dspDeactivateLeft = false;
            dspCurrentPosLeft = LeftDSP.transform.position;
            //InteractionCenter.transform.position = Vector3.Lerp(currentPosCenter, activePosCenter, speed * Time.deltaTime);
            LeftDSP.transform.position = Vector3.Lerp(dspCurrentPosLeft, dspActivePosLeft, speed * Time.deltaTime);
        }

        //set left dsp to active
        if (dspTriggeredLeft == true && dspActivePosLeft.y - dspCurrentPosLeft.y <= .01)
        {
            dspActiveLeft = true;
            dspRestingLeft = false;
            dspDeactivateLeft = false;
            dspTriggeredLeft = false;
            dspCurrentPosLeft = dspActivePosLeft;
        }

        //deactivate left dsp, return to resting position
        if (dspDeactivateLeft == true)
        {
            dspRestingLeft = false;
            dspActiveLeft = false;
            dspTriggeredLeft = false;
            dspCurrentPosLeft = LeftDSP.transform.position;
            LeftDSP.transform.position = Vector3.Lerp(dspCurrentPosLeft, dspRestPosLeft, speed * Time.deltaTime);
        }

        //reset Left DSP to resting
        if (dspDeactivateLeft == true && Mathf.Abs(dspRestPosLeft.y - dspCurrentPosLeft.y) <= .01)
        {
            dspActiveLeft = false;
            dspTriggeredLeft = false;
            dspRestingLeft = true;
            dspDeactivateLeft = false;
            dspCurrentPosLeft = dspRestPosLeft;
        }
        #endregion

        //deactivate left pillar, start return to rest
        if (deactivateLeft == true)
        {
            isActiveLeft = false;
            restingLeft = false;
            isTriggeredLeft = false;
            dspActiveLeft = false;
            dspTriggeredLeft = false;
            dspRestingLeft = false;
            dspDeactivateLeft = true;
            LeftDropSelect.SetActive(false);
            currentPosLeft = InteractionLeft.transform.position;
            InteractionLeft.transform.position = Vector3.Lerp(currentPosLeft, restPosLeft, speed * Time.deltaTime);
        }

        // reset left pillar to resting
        if (deactivateLeft == true && Mathf.Abs(restPosLeft.y - currentPosLeft.y) <= .01)
        {
            isActiveLeft = false;
            isTriggeredLeft = false;
            restingLeft = true;
            deactivateLeft = false;
            currentPosLeft = restPosLeft;
        }

        #endregion

        #region Right Panel Behaviors
        if (isTriggeredRight == true)
        {
            restingRight = false;
            isActiveRight = false;
            deactivateRight = false;
            currentPosRight = InteractionRight.transform.position;
            InteractionRight.transform.position = Vector3.Lerp(currentPosRight, activePosRight, speed * Time.deltaTime);

            //RightDropSelect.SetActive(true);
        }

        //set right pillar to active
        if (isTriggeredRight == true && Mathf.Abs(activePosRight.y - currentPosRight.y) <= .01)
        {
            isActiveRight = true;
            isTriggeredRight = false;
            restingRight = false;
            deactivateRight = false;
            currentPosRight = activePosLeft;
            //RightDropSelect.SetActive(true);
            RightDropSelect.SetActive(false);
        }

        #region DSP Box - Right Panel
        //trigger right panel DSP box animation
        if (isActiveRight == true && dspTriggeredRight == true)
        {
            dspRestingRight = false;
            dspActiveRight = false;
            dspDeactivateRight = false;
            dspCurrentPosRight = RightDSP.transform.position;
            //InteractionCenter.transform.position = Vector3.Lerp(currentPosCenter, activePosCenter, speed * Time.deltaTime);
            RightDSP.transform.position = Vector3.Lerp(dspCurrentPosRight, dspActivePosRight, speed * Time.deltaTime);
        }

        //set Right dsp to active
        if (dspTriggeredRight == true && dspActivePosRight.y - dspCurrentPosRight.y <= .01)
        {
            dspActiveRight = true;
            dspRestingRight = false;
            dspDeactivateRight = false;
            dspTriggeredRight = false;
            dspCurrentPosRight = dspActivePosRight;
        }

        //deactivate Right dsp, return to resting position
        if (dspDeactivateRight == true)
        {
            dspRestingRight = false;
            dspActiveRight = false;
            dspTriggeredRight = false;
            dspCurrentPosRight = RightDSP.transform.position;
            RightDSP.transform.position = Vector3.Lerp(dspCurrentPosRight, dspRestPosRight, speed * Time.deltaTime);
        }

        //reset Right DSP to resting
        if (dspDeactivateRight == true && Mathf.Abs(dspRestPosRight.y - dspCurrentPosRight.y) <= .01)
        {
            dspActiveRight = false;
            dspTriggeredRight = false;
            dspRestingRight = true;
            dspDeactivateRight = false;
            dspCurrentPosRight = dspRestPosRight;
        }
        #endregion

        //deactivate right pillar, start return to rest
        if (deactivateRight == true)
        {
            isActiveRight = false;
            restingRight = false;
            isTriggeredRight = false;
            dspActiveRight = false;
            dspTriggeredRight = false;
            dspRestingRight = false;
            dspDeactivateRight = true;
            RightDropSelect.SetActive(false);
            currentPosRight = InteractionRight.transform.position;
            InteractionRight.transform.position = Vector3.Lerp(currentPosRight, restPosRight, speed * Time.deltaTime);
        }

        // reset right pillar to resting
        if (deactivateRight == true && Mathf.Abs(restPosRight.y - currentPosRight.y) <= .01)
        {
            isActiveRight = false;
            isTriggeredRight = false;
            restingRight = true;
            deactivateRight = false;
            currentPosRight = restPosLeft;
        }

        #endregion
    }

    void MeasureCheck(MusicState mState)
    {
        switch (mState)
        {
            case MusicState.Drop:
                CenterDropSelect.SetActive(false);
                LeftDropSelect.SetActive(false);
                RightDropSelect.SetActive(false);
                break;
            case MusicState.Windup:

                break;
            case MusicState.Groove:
                deactivateCenter = true;
                deactivateLeft = true;
                deactivateRight = true;

                break;
            case MusicState.Filler:

                break;
        }
    }

    void DropEventReceived(DropColor dropColor, int dropLength)
    {
        if (songPlaying)
        {
            if (dropLength >= 16)
            {
                dspTriggeredCenter = true;
                dspTriggeredLeft = true;
                dspTriggeredRight = true;
            }
            else
            {
                /*
                 * Was thinking of having the pillars disappear if there's no dsp box, decided against it to keep the triggers where they are in peace
                deactivateCenter = true;
                deactivateLeft = true;
                deactivateRight = true;
                */
            }

            CenterDropSelect.SetActive(false);
            LeftDropSelect.SetActive(false);
            RightDropSelect.SetActive(false);
        }
    }

    public void BeginInteraction()
    {
        if (!StereoRail_AudioManager.Instance.tutorialPreventingDrop && songPlaying)
        {
            //Debug.Log("Beginning that interaction.");
            machineHeight = Camera.main.transform.position.y*.7f;
            dspHeight = machineHeight + .7f;
            //reset all heights for new height
            offset = new Vector3(0, machineHeight, 0);
            activePosLeft = new Vector3(restPosLeft.x, restPosLeft.y + offset.y, restPosLeft.z);

            dspOffset = new Vector3(0, dspHeight, 0);
            dspActivePosLeft = new Vector3(dspRestPosLeft.x, dspRestPosLeft.y + dspOffset.y, dspRestPosLeft.z);

            offset = new Vector3(0, machineHeight, 0);
            activePosCenter = new Vector3(restPosCenter.x, restPosCenter.y + offset.y, restPosCenter.z);

            dspOffset = new Vector3(0, dspHeight, 0);
            dspActivePosCenter = new Vector3(dspRestPosCenter.x, dspRestPosCenter.y + dspOffset.y, dspRestPosCenter.z);

            offset = new Vector3(0, machineHeight, 0);
            activePosRight = new Vector3(restPosRight.x, restPosRight.y + offset.y, restPosRight.z);

            dspOffset = new Vector3(0, dspHeight, 0);
            dspActivePosRight = new Vector3(dspRestPosRight.x, dspRestPosRight.y + dspOffset.y, dspRestPosRight.z);


            //Debug.Log("Camera height is right now at: " + machineHeight);
            isTriggeredCenter = true;
            isTriggeredLeft = true;
            isTriggeredRight = true;
        }
    }

    void ResetItAll()
    {
        songPlaying = false;
        deactivateCenter = true;
        deactivateLeft = true;
        deactivateRight = true;
    }
}
