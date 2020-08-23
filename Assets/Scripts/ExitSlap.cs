using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSlap : MonoBehaviour
{
    public enum ExitMode { exitSong, exitExperience};
    public ExitMode exitMode = ExitMode.exitExperience;

    public GameObject stopSongText;
    public GameObject quitAppText;

    public float cooldownTime;
    bool cooldownActive;

    // Start is called before the first frame update
    void Start()
    {
        exitMode = ExitMode.exitExperience;
        quitAppText.SetActive(true);
        stopSongText.SetActive(false);

        StereoRail_AudioManager.StopSongEvent += SongHasEnded;
        StereoRail_AudioManager.StartSongEvent += SongHasStarted;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something has tried to enter me *SLURP*");
        if (other.tag == "Controller")
        {
            if (!cooldownActive)
            {
                if (exitMode == ExitMode.exitExperience)
                {
                    ExitTheExperience();
                }
                if (exitMode == ExitMode.exitSong)
                {
                    StopTheSong();
                    StartCoroutine(CoolDown());
                }
            }
        }
    }

    public void StopTheSong()
    {
        //Stop whatever song is playing
        StereoRail_AudioManager.StopThisSong();
        Debug.Log("Should exit the song now!");
    }

    public void ExitTheExperience()
    {
        Debug.Log("Yo, I'm trying to exit this damn experience.");
        // will probably want to make this smoother and less jarring
        Application.Quit();
    }

    public void SongHasStarted(StereoRail_AudioManager.SongName whichSong)
    {
        Debug.Log("Exit object has switched modes to ending the song!");
        quitAppText.SetActive(false);
        stopSongText.SetActive(true);
        exitMode = ExitMode.exitSong;
    }

    public void SongHasEnded()
    {
        Debug.Log("Exit object has switched modes to quitting the game!");
        quitAppText.SetActive(true);
        stopSongText.SetActive(false);
        exitMode = ExitMode.exitExperience;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CoolDown()
    {
        cooldownActive = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldownActive = false;
    }
}
