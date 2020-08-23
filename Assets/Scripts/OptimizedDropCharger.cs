using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class OptimizedDropCharger : MonoBehaviour
{
    public bool acceptingNewInputs;

    public DropColor optionColor;
    public Material[] optionMat;
    Renderer rend;
    public GameObject jellyObject;

    void Start()
    {
        rend = jellyObject.GetComponent<Renderer>();
        StereoRail_AudioManager.DropGestureRecieved += StopAllInputs;
        acceptingNewInputs = true;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.DropGestureRecieved -= StopAllInputs;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller") && acceptingNewInputs)
        {
            StereoRail_AudioManager.Instance.TriggerDrop(optionColor, StereoRail_AudioManager.Instance.nextDropLength);
            acceptingNewInputs = false;
        }
    }

    public void ChangeOption(int matInt , DropColor whichColor)
    {
        //Debug.Log("Woah, changing to " + whichColor + " and material: " + optionMat[matInt]);

        optionColor = whichColor;
        rend.material = optionMat[matInt];
    }

    void StopAllInputs()
    {
        acceptingNewInputs = false;
    }
}
