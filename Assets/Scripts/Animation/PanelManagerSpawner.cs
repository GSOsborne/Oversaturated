using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerSpawner : MonoBehaviour
{
    public GameObject dropPanelPrefab;
    public GameObject interactionMachineLocation;
    private bool windupActive = false;
    private StereoRail_AudioManager audioManager;

    private static readonly float DistanceFromPlayer = .8f;
    private static readonly float PlayerHeight = .2f;

    // Start is called before the first frame update
    private void Start()
    {
        audioManager = StereoRail_AudioManager.Instance;
        if (!audioManager)
        {
            throw new System.Exception("No StereoRail_AudioManager instance found by PanelManagerSpawner");
        }
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
    }

    private void OnNewMeasure(MusicState state)
    {
        switch (state)
        {
            case MusicState.Drop:
                windupActive = false;
                break;
            case MusicState.Windup:
                if (!windupActive)
                {
                    if(audioManager.tutorialPreventingDrop == false)
                    {
                        SpawnDropSelector();
                        windupActive = true;
                    }
                }
                break;
            case MusicState.Groove:
                if (windupActive)
                {
                    windupActive = false;
                }
                break;
        }
    }

    private void SpawnDropSelector()
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
        
        Vector3 quaternionAdjustment = new Vector3(adjustment[0], 0f , adjustment[2]);
        Quaternion facePlayer = Quaternion.FromToRotation(dropPanelPrefab.transform.forward, quaternionAdjustment * -1f); // gonna try to change adjustment to basePosition to see if it works.

        Instantiate(dropPanelPrefab, basePosition + adjustment, facePlayer);
        */

        float cameraHeightAdjustment = Camera.main.transform.position.y - PlayerHeight;
        Debug.Log(cameraHeightAdjustment);
        Instantiate(dropPanelPrefab, new Vector3(interactionMachineLocation.transform.position.x ,cameraHeightAdjustment, interactionMachineLocation.transform.position.z), interactionMachineLocation.transform.rotation);
    }
}
