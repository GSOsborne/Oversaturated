using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class StingerExit : MonoBehaviour
{
    Animation anim;

    bool firstMeasureOfGroovePlayed;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        StereoRail_AudioManager.NewMeasureEvent += CloseOnGroove;
        firstMeasureOfGroovePlayed = false;
    }

    void CloseOnGroove(MusicState mState)
    {
        if (mState == MusicState.Groove)
        {
            if (!firstMeasureOfGroovePlayed)
            {
                anim.Play("StingerExit");
                firstMeasureOfGroovePlayed = true;
            }
        }
        else if (mState == MusicState.Drop)
        {
            firstMeasureOfGroovePlayed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
