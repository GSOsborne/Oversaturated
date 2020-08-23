using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class StingerEntrance : MonoBehaviour
{
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {

        StereoRail_AudioManager.TriggerDropEvent += GrandEntrance;
    }

    void GrandEntrance(DropColor dColor, int dLength)
    {

        anim.Play("StingerEntranceGrowth");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
