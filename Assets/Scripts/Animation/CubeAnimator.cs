using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAnimator : MonoBehaviour
{
    Animator upDownCubeAnimator;

    public float maxOffset = .2f;
    public float minOffset = 0f;

    public float animationMeasureLength;

    public GameObject pulseControlObject;
    Animator pulseAnimator;

    // Start is called before the first frame update
    void Start()
    {
        upDownCubeAnimator = GetComponent<Animator>();
        upDownCubeAnimator.SetFloat("CycleOffset", Random.Range(minOffset, maxOffset));
        //upDownCubeAnimator.Play("UpDown", 0, 0.25f);

        pulseControlObject = this.transform.Find("PulseHolder").gameObject;
        pulseAnimator = pulseControlObject.GetComponent<Animator>();


        //replace this when implemented with audio
        animationMeasureLength = .75f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PulseOnce()
    {
        pulseAnimator.SetTrigger("SendPulse");
        //Debug.Log("Pulse Sent!");
    }

    public void RotateOnce()
    {
        pulseAnimator.SetTrigger("RotateABit");
        //Debug.Log("Rotation Sent!");
    }

    public void FasterForABit()
    {
        //upDownCubeAnimator.SetTrigger("FasterPulse");
        StartCoroutine("FasterUpDown");
        //Debug.Log("Faster UpDown Sent!");
    }

    IEnumerator FasterUpDown()
    {
        float originalSpeed = upDownCubeAnimator.speed;
        upDownCubeAnimator.speed = 2f;
        yield return new WaitForSeconds(animationMeasureLength - 0.1f);
        upDownCubeAnimator.speed = originalSpeed;
        yield break;

    }


    public void BlueDropChoreography(int numberOfMeasures)
    {
        StartCoroutine("BlueDropChoreo", numberOfMeasures);
    }


    IEnumerator BlueDropChoreo(int numberOfMeasures)
    {
        PulseOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        PulseOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        FasterForABit();
        yield return new WaitForSeconds(animationMeasureLength);
        FasterForABit();
        yield return new WaitForSeconds(animationMeasureLength);
        if (numberOfMeasures == 4)
        {
            yield break;
        }
        RotateOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        RotateOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        RotateOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        RotateOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        if (numberOfMeasures == 8)
        {
            yield break;
        }
        PulseOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        FasterForABit();
        yield return new WaitForSeconds(animationMeasureLength);
        PulseOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        RotateOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        PulseOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        FasterForABit();
        yield return new WaitForSeconds(animationMeasureLength);
        PulseOnce();
        yield return new WaitForSeconds(animationMeasureLength);
        RotateOnce();
        yield break;


    }
}
