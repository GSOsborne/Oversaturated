using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaktion;
using static StereoRail_AudioManager;

public class MiscellanySpawner : MonoBehaviour
{
    Animator animator;
    Spawner spawner;
    public DropColor dropColor;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spawner = GetComponent<Spawner>();
        spawner.enabled = false;
        //animator.Play("Base Layer");
        StereoRail_AudioManager.TriggerDropEvent += BrieflyTurnOnSpawner;
    }

    void BrieflyTurnOnSpawner(DropColor givenDropColor, int dropLength)
    {
        if (givenDropColor == dropColor)
        {
            spawner.enabled = true;
            StartCoroutine(BurstForABit());
        }

    }

    IEnumerator BurstForABit()
    {
        yield return new WaitForSeconds(3f);
        spawner.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
