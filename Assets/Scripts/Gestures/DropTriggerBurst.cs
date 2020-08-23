using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class DropTriggerBurst : MonoBehaviour
{
    ParticleSystem pSystem;
    
    public Gradient redGradient;
    public Gradient orangeGradient;
    public Gradient yellowGradient;
    public Gradient greenGradient;
    public Gradient blueGradient;
    
    public Gradient purpleGradient;
    
    public Color redColor;
    public Color orangeColor;
    public Color yellowColor;
    public Color greenColor;
    public Color blueColor;
    
    public Color purpleColor;
    // Start is called before the first frame update
    void Start()
    {
        StereoRail_AudioManager.TriggerDropEvent += SpawnParticles;
        StereoRail_AudioManager.NewMeasureEvent += CheckIfEndOfDrop;

        pSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckIfEndOfDrop(MusicState mState)
    {
        if (mState == MusicState.Groove)
        {
            pSystem.Stop();
        }
    }

    void SpawnParticles(DropColor dropColor, int dropLength)
    {
        ParticleSystem.MainModule main = pSystem.main;
        var psr = GetComponent<ParticleSystemRenderer>();
        Material trailMat = psr.trailMaterial;
        //Debug.Log(trailMat.GetColor("_EmissionColor"));


        switch (dropColor)
        {
            case DropColor.Red:
                main.startColor = new ParticleSystem.MinMaxGradient(redGradient);
                trailMat.SetColor("_EmissionColor", redColor);
                //trailMat.SetColor("_Color", redColor);
                break;
            case DropColor.Orange:
                main.startColor = new ParticleSystem.MinMaxGradient(orangeGradient);
                trailMat.SetColor("_EmissionColor", orangeColor);
                //trailMat.SetColor("_Color", orangeColor);
                //Debug.Log(trailMat.emmisionColor)
                break;
            case DropColor.Yellow:
                main.startColor = new ParticleSystem.MinMaxGradient(yellowGradient);
                trailMat.SetColor("_EmissionColor", yellowColor);
                //trailMat.SetColor("_Color", yellowColor);
                break;
            case DropColor.Green:
                main.startColor = new ParticleSystem.MinMaxGradient(greenGradient);
                trailMat.SetColor("_EmissionColor", greenColor);
                //trailMat.SetColor("_Color", greenColor);
                break;
            case DropColor.Blue:
                main.startColor = new ParticleSystem.MinMaxGradient(blueGradient);
                trailMat.SetColor("_EmissionColor", blueColor);
                //trailMat.SetColor("_Color", blueColor);
                break;
            case DropColor.Purple:
                main.startColor = new ParticleSystem.MinMaxGradient(purpleGradient);
                trailMat.SetColor("_EmissionColor", purpleColor);
                //trailMat.SetColor("_Color", purpleColor);
                break;
        }
        pSystem.Play();
    }
}
