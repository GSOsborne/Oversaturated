using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class FillerManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        NewMeasureEvent += OnNewMeasure;
    }

    private void OnDestroy()
    {
        NewMeasureEvent -= OnNewMeasure;
    }

    private void OnNewMeasure(MusicState state)
    {
        switch (state)
        {
            case MusicState.Windup:
            case MusicState.Groove:
            case MusicState.Drop:
                break;
            case MusicState.Filler:
                AkSoundEngine.PostEvent("FillerMeasure", gameObject);
                AkSoundEngine.SetState("GrooveType", "Flow");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //obsolete, used for unity audio integration
    /*
    public void FillerMeasure(){
        //fillerSource.clip = fillers[Random.Range(0, fillers.Length)];
        //fillerSource.Play();

        AkSoundEngine.PostEvent("FillerMeasure", this.gameObject);
    }
    */
}
