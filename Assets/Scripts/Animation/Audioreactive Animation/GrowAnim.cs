using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowAnim : AudioreactiveDropAnim
{
    public Vector3 scaleDelta;
    public float duration;

    private bool scalingUp = false;

    private void Start()
    {
        if (!AllComponentsNonNegative(scaleDelta))
        {
            throw new System.Exception("All components of scaleDelta must be positive!");
        }
        if (duration <= 0f)
        {
            throw new System.Exception("duration must be a positive value");
        }
    }

    public override void TriggerDropAnimation(StereoRail_AudioManager.DropColor dropColor, int dropLength)
    {
        scalingUp = true;
        StartCoroutine(Grow(scaleDelta, duration));
        StartCoroutine(Grow(-1 * scaleDelta, duration));
    }

    private IEnumerator Grow(Vector3 totalChange, float totalTime)
    {
        while (!AllComponentsNonNegative(totalChange) && scalingUp == true)
        {
            yield return null;
        }

        if (!AllComponentsNonNegative(totalChange))
        {
            Debug.Log("Finished waiting for scale-up!");
        }

        Vector3 remainingChange = totalChange;
        Debug.Log("All components non-negative: " + AllComponentsNonNegative(totalChange));
        while (AllComponentsNonNegative(totalChange) ? 
            AllComponentsNonNegative(remainingChange) && remainingChange != Vector3.zero : !AllComponentsNonNegative(remainingChange))
        {
            Vector3 delta = Time.deltaTime * totalChange / totalTime;
            if (delta.magnitude > remainingChange.magnitude)
            {
                delta = remainingChange;
            }
            remainingChange -= delta;
            gameObject.transform.localScale += delta;
            yield return remainingChange;
        }
        scalingUp = false;
        Debug.Log("Finished scaling up!");
    } 

    private bool AllComponentsNonNegative(Vector3 v)
    {
        return v.x >= 0f && v.y >= 0f && v.z >= 0f;
    }
}
