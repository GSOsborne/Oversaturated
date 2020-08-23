using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class WindupControllerSprinkle : MonoBehaviour
{
    ParticleSystem pSystem;
    public bool checkIfLeftHand;
    // Start is called before the first frame update
    void Start()
    {
        StereoRail_AudioManager.WindupGestureRecieved += SpawnParticles;

        pSystem = GetComponent<ParticleSystem>();
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.WindupGestureRecieved -= SpawnParticles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnParticles()
    {
        if(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude > OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude)
        {
            if (!checkIfLeftHand)
            {
                pSystem.Play(); // because the right hand was faster at that moment so likely triggered it
            }
        }
        else
        {
            if (checkIfLeftHand)
            {
                pSystem.Play(); // cus the left hand was faster and this is the left hand
            }

        }

    }
}
