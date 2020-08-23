using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class SharedMatColorOnDrop : MonoBehaviour
{
    public Color ogColor, purpleColor, blueColor, greenColor, yellowColor, orangeColor, redColor;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial.SetColor("_Color", ogColor);
        StereoRail_AudioManager.TriggerDropEvent += DropColorChange;
    }

    void DropColorChange(DropColor givenColor, int dropLength)
    {
        switch (givenColor)
        {
            case DropColor.Purple:
                rend.sharedMaterial.SetColor("_Color", purpleColor);
                break;
            case DropColor.Blue:
                rend.sharedMaterial.SetColor("_Color", blueColor);
                break;
            case DropColor.Green:
                rend.sharedMaterial.SetColor("_Color", greenColor);
                break;
            case DropColor.Yellow:
                rend.sharedMaterial.SetColor("_Color", yellowColor);
                break;
            case DropColor.Orange:
                rend.sharedMaterial.SetColor("_Color", orangeColor);
                break;
            case DropColor.Red:
                rend.sharedMaterial.SetColor("_Color", redColor);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
