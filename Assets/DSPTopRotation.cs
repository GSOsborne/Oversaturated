using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSPTopRotation : MonoBehaviour
{
    Transform trans;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustSystemRotation(float rtpcValue)
    {
        StopAllCoroutines();
        float currentY = trans.rotation.y;
        trans.rotation = trans.rotation * Quaternion.Euler(0, (rtpcValue - 50f) / 40f, 0);

    }

    public void ReturnToZero()
    {
        //StartCoroutine(ReturnToZeroRoutine());
        //we may not even need this at all, just let it sit I guess.
    }


}
