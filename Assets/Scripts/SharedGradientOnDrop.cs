using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaktion;
using static StereoRail_AudioManager;

public class SharedGradientOnDrop : MonoBehaviour
{

    public Gradient ogGrad, purpleGrad, blueGrad, greenGrad, yellowGrad, orangeGrad, redGrad;
    SharedMaterialGear matGear;
    // Start is called before the first frame update
    void Start()
    {
        matGear = GetComponent<SharedMaterialGear>();
        matGear.colorGradient = ogGrad;
        StereoRail_AudioManager.TriggerDropEvent += DropColorChange;
    }

    void DropColorChange(DropColor givenColor, int dropLength)
    {
        switch (givenColor)
        {
            case DropColor.Purple:
                matGear.colorGradient = purpleGrad;
                break;
            case DropColor.Blue:
                matGear.colorGradient = blueGrad;
                break;
            case DropColor.Green:
                matGear.colorGradient = greenGrad;
                break;
            case DropColor.Yellow:
                matGear.colorGradient = yellowGrad;
                break;
            case DropColor.Orange:
                matGear.colorGradient = orangeGrad;
                break;
            case DropColor.Red:
                matGear.colorGradient = redGrad;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
