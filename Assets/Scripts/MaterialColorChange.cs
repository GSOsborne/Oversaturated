using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class MaterialColorChange : MonoBehaviour
{
    public Color duringPurple, duringRed, duringOrange, duringYellow, duringGreen, duringBlue;
    Renderer colorRenderer;

    // Start is called before the first frame update
    void Start()
    {
        colorRenderer = GetComponent<Renderer>();
        //Debug.Log("Material is " + colorRenderer.material);
        StereoRail_AudioManager.TriggerDropEvent += ChangeMatColorOnDrop;
        ChangeMatColorOnStart();
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.TriggerDropEvent -= ChangeMatColorOnDrop;
    }

    void ChangeMatColorOnStart()
    {
        
        DropColor whichColor = StereoRail_AudioManager.Instance.colorOfDrop;
        Color newColor = Color.black;
        switch (whichColor)
        {
            case DropColor.Purple:
                newColor = duringPurple;
                break;
            case DropColor.Red:
                newColor = duringRed;
                break;
            case DropColor.Orange:
                newColor = duringOrange;
                break;
            case DropColor.Yellow:
                newColor = duringYellow;
                break;
            case DropColor.Green:
                newColor = duringGreen;
                break;
            case DropColor.Blue:
                newColor = duringBlue;
                break;
        }
        colorRenderer.material.SetColor("_Color", newColor);
        //Debug.Log("Set Color to " + newColor);
    }

    void ChangeMatColorOnDrop(DropColor dColor, int dropLength)
    {
        Color newColor = Color.black;
        switch (dColor)
        {
            case DropColor.Purple:
                newColor = duringPurple;
                break;
            case DropColor.Red:
                newColor = duringRed;
                break;
            case DropColor.Orange:
                newColor = duringOrange;
                break;
            case DropColor.Yellow:
                newColor = duringYellow;
                break;
            case DropColor.Green:
                newColor = duringGreen;
                break;
            case DropColor.Blue:
                newColor = duringBlue;
                break;
        }
        colorRenderer.material.SetColor("_Color", newColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
