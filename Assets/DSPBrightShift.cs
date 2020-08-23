using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class DSPBrightShift : MonoBehaviour
{
    Camera cam;
    Color dropColor;
    public ChangeCameraColor changeCam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        changeCam = GetComponent<ChangeCameraColor>();
        StereoRail_AudioManager.TriggerDropEvent += SetDropColor;
    }

    void SetDropColor(DropColor dColor, int dropLength)
    {
        switch (dColor)
        {
            case DropColor.Yellow:
                dropColor = changeCam.whenYellow;
                break;
            case DropColor.Red:
                dropColor = changeCam.whenRed;
                break;
            case DropColor.Purple:
                dropColor = changeCam.whenPurple;
                break;
            case DropColor.Blue:
                dropColor = changeCam.whenBlue;
                break;
            case DropColor.Green:
                dropColor = changeCam.whenGreen;
                break;
            case DropColor.Orange:
                dropColor = changeCam.whenOrange;
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustBackgroundBrightness(float rtpcValue)
    {
        StopAllCoroutines();
        float colorAdjust = (rtpcValue + 50)/100;
        cam.backgroundColor = new Color(Mathf.Clamp(dropColor.r * colorAdjust, 0f,1f), Mathf.Clamp(dropColor.g * colorAdjust, 0f, 1f), Mathf.Clamp(dropColor.b * colorAdjust, 0f, 1f),1f);
    }

    public void ReturnToDropColor()
    {
        StartCoroutine(ReturnToDropColorRoutine());
    }

    IEnumerator ReturnToDropColorRoutine()
    {
        Color currentColor = cam.backgroundColor;
        for (float i = 0; i <= 10; i++)
        {
            cam.backgroundColor = Color.Lerp(currentColor, dropColor, i / 10);
            yield return null;
        }
    }
}
