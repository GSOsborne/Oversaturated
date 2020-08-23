using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class ToyBoxManager : MonoBehaviour
{
    public GameObject[] boxes;
    private bool haveWeFadedOut;

    // Start is called before the first frame update
    void Start()
    {
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.StopSongEvent += TurnOffToyBoxes;
        StereoRail_AudioManager.TriggerDropEvent += TurnToysOn;
        TurnOffToyBoxes();
    }

    void TurnToysOn(DropColor dColor, int dLength)
    {
        if (dLength >= 16)
        {
            if (boxes != null)
            {
                foreach (GameObject box in boxes)
                {
                    DSPBox dspBoxScript = box.GetComponent<DSPBox>();
                    if (dspBoxScript != null)
                    {
                        dspBoxScript.dspEnabled = true;
                    }
                }

            }
        }
    }

    private void Update()
    {
        bool allPanelsNull = true;
        foreach (GameObject box in boxes)
        {
            if (box)
            {
                allPanelsNull = false;
                break;
            }
        }

        if (allPanelsNull)
        {

            //Debug.Log("No ToyBoxes contained within holder " + gameObject + "; destroying holder");
            //Destroy(gameObject);
        }
    }

    public void TurnOffToyBoxes()
    {
        if (boxes != null)
        {
            foreach (GameObject box in boxes)
            {
                DSPBox dspBoxScript = box.GetComponent<DSPBox>();
                if (dspBoxScript != null)
                {
                    dspBoxScript.dspEnabled = false;
                }

            }

        }

    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
        StereoRail_AudioManager.StopSongEvent -= TurnOffToyBoxes;
    }


    void OnNewMeasure(MusicState currentState)
    {
        switch (currentState)
        {
            case MusicState.Groove:
                if (!haveWeFadedOut)
                {
                    TurnOffToyBoxes();
                    haveWeFadedOut = true;
                }
                break;
            case MusicState.Drop:
                haveWeFadedOut = false;
                break;
        }
    }
    /*
    void FadeOutPanelHolder()
    {
        Debug.Log("Starting fade out process!");
        foreach (GameObject box in boxes)
        {
                ColorFader currentFader = box.GetComponent<ColorFader>();
            if(currentFader)
            {
                StartCoroutine(currentFader.FadeOut());
                Debug.Log("Fading out the ToyBox " + gameObject);
            }
            else
            {
                Debug.Log("Couldn't find that damn color fader on " + box.name);
            }
        }
    }
    */
}
