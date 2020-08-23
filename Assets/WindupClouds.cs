using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class WindupClouds : MonoBehaviour
{
    ParticleSystem pSys;
    bool systemStarted;
    // Start is called before the first frame update
    void Start()
    {
        pSys = GetComponent<ParticleSystem>();
        StereoRail_AudioManager.NewMeasureEvent += DecideCloudFate;
    }

    void DecideCloudFate(MusicState givenState)
    {
        if (givenState == MusicState.Windup)
        {
            if (!systemStarted)
            {
                pSys.Play();
                systemStarted = true;
            }
        }
        else if (givenState == MusicState.Drop)
        {
            pSys.Stop();
            systemStarted = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
