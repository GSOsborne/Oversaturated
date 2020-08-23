using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class BatteryCharge : MonoBehaviour
{
    StereoRail_AudioManager audMan;
    int windupCount;
    int dropCount;
    public GameObject[] levels;
    // Start is called before the first frame update
    void Start()
    {
        audMan = StereoRail_AudioManager.Instance;
        StereoRail_AudioManager.NewMeasureEvent += Counter;
        windupCount = 0;
        dropCount = 0;
        foreach (GameObject level in levels)
        {
            level.SetActive(false);
        }
        if (audMan.theWindupCounter <= 16)
        {
            for (int level = 0; level < audMan.theWindupCounter; level++)
            {
                levels[level].SetActive(true);
            }
        }
        else
        {
            for (int level = 0; level < 16; level++)
            {
                levels[level].SetActive(true);
            }
        }

    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= Counter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Counter(MusicState currentState)
    {
        if(currentState == MusicState.Groove)
        {
            windupCount = 0;
            dropCount = 0;
            foreach (GameObject level in levels)
            {
                level.SetActive(false);
            }
        }
        else if (currentState == MusicState.Drop)
        {
            //Something cute for the battery exploding or whatever
            if(dropCount == 0 && gameObject.activeSelf)
            {

                //StartCoroutine(Flashing());
            }
            dropCount++;

        }
        else
        {
            dropCount = 0;
            windupCount = audMan.theWindupCounter;
            //Debug.Log("windup count is: " + windupCount);
            if (windupCount <= 16 && windupCount > 0)
            {
                levels[windupCount - 1].SetActive(true);
            }
        }
    }

    IEnumerator Flashing()
    {
        Debug.Log("windup count is: " + windupCount);
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(.1f);
            for (int level=0; level < windupCount; level++)
            {
                levels[level].SetActive(true);
            }
            yield return new WaitForSeconds(.1f);
            for (int level = 0; level < windupCount; level++)
            {
                levels[level].SetActive(false);
            }
        }
        
    }
}
