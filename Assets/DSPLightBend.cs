using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSPLightBend : MonoBehaviour
{
    ParticleSystem pSys;
    public float dividerValue, maxSlantValue;
    // Start is called before the first frame update
    void Start()
    {
        pSys = GetComponent<ParticleSystem>();
        var velocityOverLifetime = pSys.velocityOverLifetime;
        velocityOverLifetime.xMultiplier = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustSystemSlant(float rtpcValue)
    {
        StopAllCoroutines();
        var velocityOverLifetime = pSys.velocityOverLifetime;
        velocityOverLifetime.xMultiplier = (-rtpcValue + 50) / 50 * maxSlantValue;
        
    }

    public void ReturnToNormal()
    {
        StartCoroutine(SlideBackToValue());
    }

    IEnumerator SlideBackToValue()
    {
        var velocityOverLifetime = pSys.velocityOverLifetime;
        while (velocityOverLifetime.xMultiplier > 1 || velocityOverLifetime.xMultiplier < -1)
        {
            velocityOverLifetime.xMultiplier = velocityOverLifetime.xMultiplier / dividerValue;
            yield return null;
        }
    }
}
