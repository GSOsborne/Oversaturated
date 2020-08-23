using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class SharedMatOnDrop : MonoBehaviour
{
    public float startMinIntensity, startMaxIntensity, finalMinIntensity, finalMaxIntensity, incrementValue;
    float currentMinIntensity, currentMaxIntensity;
    public Color ogColor, purpleColor, blueColor, greenColor, yellowColor, orangeColor, redColor;
    Renderer rend;
    bool firstGrooveMeasurePassed;
    Color currentDropColor;
    // Start is called before the first frame update
    void Start()
    {
        currentDropColor = ogColor;
        rend = GetComponent<Renderer>();
        rend.sharedMaterial.SetColor("_EmissionColor", ogColor*startMinIntensity);
        ResetMinMaxIntensity(SongName.PaperclipSwarm);
        StereoRail_AudioManager.TriggerDropEvent += DropColorChange;
        StereoRail_AudioManager.StartSongEvent += ResetMinMaxIntensity;
        StereoRail_AudioManager.NewMeasureEvent += CheckForGroove;
        firstGrooveMeasurePassed = true;
    }

    void DropColorChange(DropColor givenColor, int dropLength)
    {
        IncrementMinMaxIntensity();
        switch (givenColor)
        {
            case DropColor.Purple:
                StartCoroutine(FadeToGivenColor(purpleColor));
                break;
            case DropColor.Blue:
                StartCoroutine(FadeToGivenColor(blueColor));
                break;
            case DropColor.Green:
                StartCoroutine(FadeToGivenColor(greenColor));
                break;
            case DropColor.Yellow:
                StartCoroutine(FadeToGivenColor(yellowColor));
                break;
            case DropColor.Orange:
                StartCoroutine(FadeToGivenColor(orangeColor));
                break;
            case DropColor.Red:
                StartCoroutine(FadeToGivenColor(redColor));
                break;
        }

    }

    void ResetMinMaxIntensity(SongName songName)
    {
        currentMinIntensity = startMinIntensity;
        currentMaxIntensity = startMaxIntensity;
        rend.sharedMaterial.SetColor("_EmissionColor", currentDropColor * currentMinIntensity);
    }

    void IncrementMinMaxIntensity()
    {
        currentMinIntensity = Mathf.Min(currentMinIntensity + incrementValue, finalMinIntensity);
        currentMaxIntensity = Mathf.Min(currentMaxIntensity + incrementValue, finalMaxIntensity);
    }

    IEnumerator FadeToGivenColor(Color givenColor)
    {
        Debug.Log("Trying to get to " + givenColor);
        currentDropColor = givenColor;
        Color startColor = rend.sharedMaterial.GetColor("_EmissionColor");
        for (float i = 0; i <= 10; i++)
        {
            rend.sharedMaterial.SetColor("_EmissionColor", Color.Lerp(startColor, givenColor, i / 10) * (currentMinIntensity + i/10*(currentMaxIntensity-currentMinIntensity)));
            //Debug.Log("current cam color is: " + cam.backgroundColor);
            Debug.Log("Fading In: " + (currentMinIntensity + i / 10 * (currentMaxIntensity - currentMinIntensity)));
            yield return null;
        }
    }

    void CheckForGroove(MusicState givenState)
    {
        if (givenState == MusicState.Groove)
        {
            if (!firstGrooveMeasurePassed)
            {
                StartCoroutine(DropIntensityOnGroove());
                firstGrooveMeasurePassed = true;
            }
        }
        else if (givenState == MusicState.Drop)
        {
            firstGrooveMeasurePassed = false;
        }
    }

    IEnumerator DropIntensityOnGroove()
    {
        //Color startColor = rend.sharedMaterial.GetColor("_EmissionColor");
        for (float i = 0; i <= 10; i++)
        {
            rend.sharedMaterial.SetColor("_EmissionColor", currentDropColor * (currentMaxIntensity - i / 10 * (currentMaxIntensity - currentMinIntensity)));
            //Debug.Log("current cam color is: " + cam.backgroundColor);
            Debug.Log("Fading Out: " + (currentMaxIntensity - i / 10 * (currentMaxIntensity - currentMinIntensity)));
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
