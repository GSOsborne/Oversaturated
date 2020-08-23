using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentRiser : MonoBehaviour
{
    public AK.Wwise.Event startLevelEvent;
    // Start is called before the first frame update
    bool firstMeasureOfRiser;

    void Start()
    {
        firstMeasureOfRiser = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSong()
    {
        Debug.Log("Starting the song now!");
        AkSoundEngine.PostEvent("StopSong", gameObject);
        startLevelEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBar, DelayMeasureCheck);
        AkSoundEngine.PostEvent("RiserStart", gameObject);

    }

    void DelayMeasureCheck(object in_cookie, AkCallbackType in_type, object in_info)
    {

        //measureLength = AkMusicSyncCallbackInfo;

        StartCoroutine(DelayMeasureCheckCoroutine());
    }

    IEnumerator DelayMeasureCheckCoroutine()
    {
        //Debug.Log("measure delay activated!");
        yield return new WaitForSeconds(.7f);
        EveryMeasureCheck();
    }

    void EveryMeasureCheck()
    {
        AkSoundEngine.PostEvent("WindupTrigger", gameObject);
    }

    public void StopSong()
    {
        Debug.Log("Stopping the song now.");
        AkSoundEngine.PostEvent("StopSong", gameObject);
        firstMeasureOfRiser = true;
    }
}
