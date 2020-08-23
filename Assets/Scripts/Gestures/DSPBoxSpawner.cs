using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class DSPBoxSpawner : MonoBehaviour
{
    public GameObject dspSpawnerPrefab;
    public GameObject interactionMachineLocation;
    private bool dropActive = false;
    private StereoRail_AudioManager audioManager;
    private bool lengthAppropriate;

    private static readonly float DistanceFromPlayer = .4f;
    private static readonly float PlayerHeight = .4f;

    // Start is called before the first frame update
    private void Start()
    {
        audioManager = StereoRail_AudioManager.Instance;
        if (!audioManager)
        {
            throw new System.Exception("No StereoRail_AudioManager instance found by PanelManagerSpawner");
        }
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.TriggerDropEvent += SetDSPActivityLogic;
        lengthAppropriate = false;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
        StereoRail_AudioManager.TriggerDropEvent -= SetDSPActivityLogic;
    }

    private void OnNewMeasure(MusicState state)
    {
        switch (state)
        {
            case MusicState.Drop:
                if (!dropActive)
                {
                    if (lengthAppropriate)
                    {
                        SpawnDSPBoxes();
                    }
                    dropActive = true;
                }
                break;
            case MusicState.Groove:
            case MusicState.Filler:
            case MusicState.Windup:
                dropActive = false;
                break;
        }
    }

    private void SetDSPActivityLogic(DropColor color, int length)
    {
        // ******** This is where you change the activation length, was 16 before **********
        if (length > 8)
        {
            lengthAppropriate = true;
        }
        else
        {
            lengthAppropriate = false;
        }
    }

    private void SpawnDSPBoxes()
    {
        /*
        Vector3 basePosition = Camera.main.transform.position;
        Quaternion baseRotation = Camera.main.transform.rotation;

        // TODO: Verify this logic all works
        Vector3 adjustment = baseRotation * Vector3.forward;
        adjustment[1] = 0;
        adjustment = adjustment.normalized;
        adjustment *= DistanceFromPlayer;
        adjustment[1] = -1 * PlayerHeight;

        Debug.Log("Adjustment vector is currently: " + adjustment);
        //gonna fiddle a little bit with inputting different vectors

        Vector3 quaternionAdjustment = new Vector3(adjustment[0], 0f, adjustment[2]);
        Quaternion facePlayer = Quaternion.FromToRotation(dspSpawnerPrefab.transform.forward, quaternionAdjustment * -1f); // gonna try to change adjustment to basePosition to see if it works.

        Instantiate(dspSpawnerPrefab, basePosition + adjustment, facePlayer);*/


        /*
        float cameraHeightAdjustment = Camera.main.transform.position.y - PlayerHeight;
        Debug.Log(cameraHeightAdjustment);
        Instantiate(dspSpawnerPrefab, new Vector3(interactionMachineLocation.transform.position.x, cameraHeightAdjustment, interactionMachineLocation.transform.position.z), interactionMachineLocation.transform.rotation);
        */

        Instantiate(dspSpawnerPrefab, interactionMachineLocation.transform);

    }
}

