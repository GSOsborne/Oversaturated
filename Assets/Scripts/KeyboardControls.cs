using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    public StereoRail_AudioManager am;
    bool canYouStartSong;
    // Start is called before the first frame update
    void Start()
    {
        canYouStartSong = true;
        am = StereoRail_AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            if (canYouStartSong)
            {
                am.DebugStartPaperclipSwarm();
                canYouStartSong = false;
            }
        }
        if (Input.GetKey("p"))
        {
            StereoRail_AudioManager.StopThisSong();
            canYouStartSong = true;
        }
        if (Input.GetKey("w"))
        {
            am.TriggerWindup();
        }
        if (Input.GetKey("d"))
        {
            am.TriggerDrop(StereoRail_AudioManager.DropColor.Blue, 32);
        }
        if (Input.GetKey("f"))
        {
            am.TriggerDrop(StereoRail_AudioManager.DropColor.Green, 32);
        }
        if (Input.GetKey("g"))
        {
            am.TriggerDrop(StereoRail_AudioManager.DropColor.Orange, 32);
        }
        if (Input.GetKey("h"))
        {
            am.TriggerDrop(StereoRail_AudioManager.DropColor.Purple, 32);
        }
        if (Input.GetKey("j"))
        {
            am.TriggerDrop(StereoRail_AudioManager.DropColor.Red, 32);
        }
        if (Input.GetKey("k"))
        {
            am.TriggerDrop(StereoRail_AudioManager.DropColor.Yellow, 32);
        }
    }
}
