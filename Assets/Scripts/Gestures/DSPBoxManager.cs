using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSPBoxManager : MonoBehaviour
{
    public GameObject[] boxes;
    private bool haveWeFadedOut;

    // Start is called before the first frame update
    void Start()
    {
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.StopSongEvent += FadeOutPanelHolder; 
        foreach (GameObject box in boxes)
        {
            ColorFader currentFader = box.GetComponent<ColorFader>();
            if (currentFader)
            {
                StartCoroutine(currentFader.FadeIn());
                Debug.Log("fading in " + box.name);
            }
            else
            {
                Debug.Log("Couldn't find a way to fade in" + box.name);
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
            Debug.Log("No DSPBoxes contained within holder; destroying holder");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
        StereoRail_AudioManager.StopSongEvent -= FadeOutPanelHolder;
    }


    void OnNewMeasure(MusicState currentState)
    {
        switch (currentState)
        {
            case MusicState.Groove:
                if (!haveWeFadedOut)
                {
                    FadeOutPanelHolder();
                    haveWeFadedOut = true;
                }
                break;
            case MusicState.Drop:
                haveWeFadedOut = false;
                break;
        }
    }

    void FadeOutPanelHolder()
    {
        Debug.Log("Starting fade out process!");
        foreach (GameObject box in boxes)
        {
                ColorFader currentFader = box.GetComponent<ColorFader>();
            if(currentFader)
            {
                StartCoroutine(currentFader.FadeOut());
                Debug.Log("Fading out a DSPBox!");
            }
            else
            {
                Debug.Log("Couldn't find that damn color fader on " + box.name);
            }
        }
    }
}
