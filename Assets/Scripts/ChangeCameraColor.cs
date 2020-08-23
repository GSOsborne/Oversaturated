using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class ChangeCameraColor : MonoBehaviour
{
    Camera cam;
    //public float fadeSpeed;
    public Color whenOrange, whenYellow, whenGreen, whenBlue, whenPurple, whenRed;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.backgroundColor = whenPurple;
        StereoRail_AudioManager.TriggerDropEvent += ChangeBackground;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.TriggerDropEvent -= ChangeBackground;
    }

    void ChangeBackground(DropColor givenColor, int dropLength)
    {
        switch (givenColor)
        {
            case DropColor.Yellow:
                //cam.backgroundColor = whenYellow;
                StartCoroutine(FadeToGivenColor(whenYellow));
                break;
            case DropColor.Red:
                StartCoroutine(FadeToGivenColor(whenRed));
                break;
            case DropColor.Purple:
                StartCoroutine(FadeToGivenColor(whenPurple));
                break;
            case DropColor.Blue:
                StartCoroutine(FadeToGivenColor(whenBlue));
                break;
            case DropColor.Green:
                StartCoroutine(FadeToGivenColor(whenGreen));
                break;
            case DropColor.Orange:
                StartCoroutine(FadeToGivenColor(whenOrange));
                break;
        }
    }

    IEnumerator FadeToGivenColor(Color givenColor)
    {
        Debug.Log("Trying to get to " + givenColor);
        Color startColor = cam.backgroundColor;
        for (float i = 0; i < 10; i++)
        {
            cam.backgroundColor = Color.Lerp(startColor, givenColor, i/10);
            //Debug.Log("current cam color is: " + cam.backgroundColor);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
