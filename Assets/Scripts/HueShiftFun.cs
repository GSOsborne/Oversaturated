using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
using static StereoRail_AudioManager;

/*
public class HueShiftFun : MonoBehaviour
{

    public PostProcessVolume volume;
    public float hueShiftSpeed;
    public float saturationShiftSpeed;
    public float startingSaturationValue;
    public float maxSaturationValue;
    private ColorGrading colorGrading;

    bool firstMeasureOfDropPlayed;
    bool firstMeasureOfGroovePlayed;
    int numberOfDropsPlayed;


    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out colorGrading);

        colorGrading.hueShift.value = 0;
        colorGrading.saturation.value = startingSaturationValue;
        numberOfDropsPlayed = 0;

        StereoRail_AudioManager.NewMeasureEvent += StateCheck;
        StereoRail_AudioManager.TriggerDropEvent += DropHueShift;
        StereoRail_AudioManager.StartSongEvent += ResetAllValues;
        
    }

    void ResetAllValues(SongName songName)
    {
        numberOfDropsPlayed = 0;
        DesaturatePostDrop();

    }

    void StateCheck(MusicState mState)
    {
        switch (mState)
        {
            case MusicState.Drop:
                if (!firstMeasureOfDropPlayed)
                {
                    ResaturateForDrop();
                }
                firstMeasureOfGroovePlayed = false;
                firstMeasureOfDropPlayed = true;
                break;
            case MusicState.Groove:
                if (!firstMeasureOfGroovePlayed)
                {
                    DesaturatePostDrop();
                }
                firstMeasureOfGroovePlayed = true;
                firstMeasureOfDropPlayed = false;

                break;
            case MusicState.Windup:
                break;
            case MusicState.Filler:
                break;
        }
    }

    void DropHueShift(DropColor dColor, int dropLength)
    {
        //Debug.Log("Oh Shit, a drop, change color!");
        float targetHueValue = 0f;
        switch (dColor)
        {
            case DropColor.Blue:
                targetHueValue = -34f;
                StartCoroutine(ShiftToDropHue(targetHueValue));
                break;
            case DropColor.Green:
                targetHueValue = -175f;
                StartCoroutine(ShiftToDropHue(targetHueValue));
                break;
            case DropColor.Orange:
                targetHueValue = 114f;
                StartCoroutine(ShiftToDropHue(targetHueValue));
                break;
            case DropColor.Purple:
                targetHueValue = 0f;
                StartCoroutine(ShiftToDropHue(targetHueValue));
                break;
            case DropColor.Red:
                targetHueValue = 80f;
                StartCoroutine(ShiftToDropHue(targetHueValue));
                break;
            case DropColor.Yellow:
                targetHueValue = 139f;
                StartCoroutine(ShiftToDropHue(targetHueValue));
                break;

        }
    }

    IEnumerator ShiftToDropHue(float targetHueValue)
    {
        bool weOughtaBeCyclingDown = true;
        Debug.Log("We made it to the coroutine, target hue value of: " + targetHueValue);
        float currentHueValue = colorGrading.hueShift.value;

        //figuring out whether its closer to go up or down in values
        if (targetHueValue > currentHueValue)
        {
            if (targetHueValue-currentHueValue > 180)
            {
                weOughtaBeCyclingDown = true;
                //Debug.Log("???With target hue " + targetHueValue + " and current hue " + currentHueValue + " we oughta cycle down.");
            }
            else
            {
                weOughtaBeCyclingDown = false;
                //Debug.Log("!!!With target hue " + targetHueValue + " and current hue " + currentHueValue + " we oughta cycle up.");
            }
        }
        else
        {
            if (currentHueValue-targetHueValue > 180)
            {
                weOughtaBeCyclingDown = false;
                //Debug.Log("&&&With target hue " + targetHueValue + " and current hue " + currentHueValue + " we oughta cycle up.");
            }
            else
            {
                weOughtaBeCyclingDown = true;
                //Debug.Log("%%%With target hue " + targetHueValue + " and current hue " + currentHueValue + " we oughta cycle down.");
            }
        }


        while (currentHueValue>(targetHueValue + 10) || currentHueValue < (targetHueValue - 10)) //if we haven't approached the correct hue value
        {
            if (weOughtaBeCyclingDown)
            {
                currentHueValue -= hueShiftSpeed * Time.deltaTime;
            }
            else
            {
                currentHueValue += hueShiftSpeed * Time.deltaTime;
            }

            if (currentHueValue < -180)
            {
                currentHueValue += 360;
            }
            if (currentHueValue > 180)
            {
                currentHueValue -= 360;
            }
            colorGrading.hueShift.value = currentHueValue;
            //Debug.Log("current hue value is " + currentHueValue + " but we're still working on it");
            yield return null;
        }
        while (currentHueValue > (targetHueValue + 1) || currentHueValue < (targetHueValue - 1)) //if we haven't approached the correct hue value
        {
            if (weOughtaBeCyclingDown)
            {
                currentHueValue -= hueShiftSpeed/10 * Time.deltaTime;
            }
            else
            {
                currentHueValue += hueShiftSpeed/10 * Time.deltaTime;
            }

            if (currentHueValue < -180)
            {
                currentHueValue += 360;
            }
            if (currentHueValue > 180)
            {
                currentHueValue -= 360;
            }
            colorGrading.hueShift.value = currentHueValue;
            //Debug.Log("current hue value is " + currentHueValue + " but we're still working on it");
            yield return null;
        }
        //Debug.Log("final hue value should be: " + currentHueValue);
    }

    void ResaturateForDrop()
    {
        StartCoroutine(Resaturate());
    }

    IEnumerator Resaturate()
    {
        float currentSaturation = colorGrading.saturation.value;
        float targetSaturation = Mathf.Min(0 + numberOfDropsPlayed * 2, maxSaturationValue);
        //Debug.Log("Target Saturation is " + targetSaturation);
        while (currentSaturation < targetSaturation ) // change this value if you want to oversaturate
        {
            currentSaturation += saturationShiftSpeed * Time.deltaTime;
            colorGrading.saturation.value = currentSaturation;
            //Debug.Log("Current saturation is at " + currentSaturation);
            yield return null;
        }
        Debug.Log("Final Resaturation is: " + currentSaturation);
    }

    void DesaturatePostDrop()
    {
        float targetSaturation = Mathf.Min(startingSaturationValue + numberOfDropsPlayed * 8, 0f);
        StartCoroutine(DesaturateCoRoutine(targetSaturation));
        numberOfDropsPlayed++;
    }

    IEnumerator DesaturateCoRoutine(float targetSaturation)
    {
        float currentSaturation = colorGrading.saturation.value;
        while (currentSaturation > targetSaturation)
        {
            currentSaturation -= saturationShiftSpeed * Time.deltaTime;
            colorGrading.saturation.value = currentSaturation;
            //Debug.Log("Current desaturation is at " + currentSaturation);
            yield return null;
        }
        Debug.Log("Final Desaturation is: " + currentSaturation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/