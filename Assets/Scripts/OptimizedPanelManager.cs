using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class OptimizedPanelManager : MonoBehaviour
{
    bool dropSelectionsActive;
    public GameObject[] dropSelections;
    OptimizedDropCharger[] dropSelectionChargers;

    // Start is called before the first frame update
    void Start()
    {
        dropSelections[0].SetActive(false);
        dropSelections[1].SetActive(false);
        dropSelections[2].SetActive(false);
        dropSelectionChargers = new OptimizedDropCharger[3];
        dropSelectionChargers[0] = dropSelections[0].GetComponent<OptimizedDropCharger>();
        dropSelectionChargers[1] = dropSelections[1].GetComponent<OptimizedDropCharger>();
        dropSelectionChargers[2] = dropSelections[2].GetComponent<OptimizedDropCharger>();
        dropSelectionsActive = false;
        StereoRail_AudioManager.WindupGestureRecieved += SummonDropOptions;
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.DropGestureRecieved += TurnOffOptions;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.WindupGestureRecieved -= SummonDropOptions;
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
        StereoRail_AudioManager.DropGestureRecieved -= TurnOffOptions;

    }

    void SummonDropOptions()
    {
        if (!StereoRail_AudioManager.Instance.tutorialPreventingDrop)
        {
            if (!dropSelectionsActive)
            {
                //send a call to turn on the panels
                dropSelections[0].SetActive(true);
                dropSelections[1].SetActive(true);
                dropSelections[2].SetActive(true);
                dropSelectionChargers[0].acceptingNewInputs = true;
                dropSelectionChargers[1].acceptingNewInputs = true;
                dropSelectionChargers[2].acceptingNewInputs = true;
                dropSelectionsActive = true;
            }
        }
    }

    void OnNewMeasure(MusicState mState)
    {
        if(mState == MusicState.Windup)
        {
            if (dropSelectionsActive)
            {
                int panel0, panel1, panel2;
                //randomize drop selection values
                //pick three numbers from 0 to 5
                panel0 = Random.Range(0, 6);
                panel1 = Random.Range(0, 6);
                while (panel1 == panel0)
                {
                    panel1 = Random.Range(0, 6);
                }
                panel2 = Random.Range(0, 6);
                while (panel2 == panel0 || panel2 == panel1)
                {
                    panel2 = Random.Range(0, 6);
                }

                //Debug.Log("Panel1 int: " + panel0);
                //Debug.Log("Panel2 int: " + panel1);
                //Debug.Log("Panel3 int: " + panel2);

                DropColor panel0Col, panel1Col, panel2Col;
                panel0Col = AssignDropColor(panel0);
                panel1Col = AssignDropColor(panel1);
                panel2Col = AssignDropColor(panel2);


                dropSelections[0].GetComponent<OptimizedDropCharger>().ChangeOption(panel0, panel0Col);
                dropSelections[1].GetComponent<OptimizedDropCharger>().ChangeOption(panel1, panel1Col);
                dropSelections[2].GetComponent<OptimizedDropCharger>().ChangeOption(panel2, panel2Col);
            }


        }
        else if(mState == MusicState.Groove)
        {
            TurnOffOptions();
        }
    }

    DropColor AssignDropColor(int colorInt)
    {
        if (colorInt == 0)
        {
            return DropColor.Purple;
        }
        else if(colorInt == 1)
        {
            return DropColor.Blue;
        }
        else if (colorInt == 2)
        {
            return DropColor.Green;
        }
        else if (colorInt == 3)
        {
            return DropColor.Yellow;
        }
        else if (colorInt == 4)
        {
            return DropColor.Orange;
        }
        else if (colorInt == 5)
        {
            return DropColor.Red;
        }
        else
        {
            Debug.Log("Hey, you sent the wrong kind of int!");
            return DropColor.Purple;
        }
    }

    void TurnOffOptions()
    {
        //go through and turn off all three objects and their colliders
        dropSelections[0].SetActive(false);
        dropSelections[1].SetActive(false);
        dropSelections[2].SetActive(false);
        dropSelectionChargers[0].acceptingNewInputs = false;
        dropSelectionChargers[1].acceptingNewInputs = false;
        dropSelectionChargers[2].acceptingNewInputs = false;
        dropSelectionsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
