using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class DSPBox : MonoBehaviour
{
    Vector3 positiveDSPDirection;
    Vector3 objectZeroPostion;
    public string RTPCName;    
    float expectedScale;
    public bool dspEnabled;

    public DSPLightBend[] lightBend;
    public DSPTopRotation topRot;
    public DSPBrightShift brightShift;
    //public float minTemp, maxTemp, defaultTemp, minTint, maxTint, defaultTint, minContrast, maxContrast, defaultContrast, minBright, maxBright, defaultBright, returnToDefaultSpeed;

    //public PostProcessVolume postProcessVolume;
    //private ColorGrading colorGrading;
    //three post processing values are temperature, tint, contrast

    // Start is called before the first frame update
    void Start()
    {
        positiveDSPDirection = Vector3.Normalize(transform.right);
        expectedScale = gameObject.transform.localScale.x;

        objectZeroPostion = gameObject.transform.position - positiveDSPDirection * expectedScale / 2;
        dspEnabled = false;
        //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), objectZeroPostion, Quaternion.identity);
        /*
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        colorGrading.temperature.value = 0;
        colorGrading.tint.value = 0;
        colorGrading.contrast.value = 0;
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        AkSoundEngine.SetRTPCValue(RTPCName, 0);
        //Debug.Log("All DSP has been reset!");
    }

    private void OnTriggerStay(Collider other)
    {
        if (dspEnabled)
        {
            if (other.tag == "Controller")
            {
                StopAllCoroutines();
                Vector3 vectorFromZero = objectZeroPostion - other.transform.position;
                Vector3 projectedVector = Vector3.Project(vectorFromZero, positiveDSPDirection);
                float calculatedVectorMagnitude = projectedVector.magnitude;
                float DSPValue = calculatedVectorMagnitude * 100 / expectedScale * 0.88f + 10;
                DSPValue = Mathf.Clamp(DSPValue, 10, 100);

                AkSoundEngine.SetRTPCValue(RTPCName, DSPValue);
                //Debug.Log(DSPValue);

                // adding some DSP logic
                /*
               if (RTPCName == "DSP_1")
                {
                    colorGrading.contrast.value = minContrast + ((DSPValue - 10) / 90 * (maxContrast - minContrast));
                    colorGrading.brightness.value = maxBright - ((DSPValue - 10) / 90 * (maxContrast - minContrast));
                }
               else if (RTPCName == "DSP_2")
                {
                    colorGrading.tint.value = minTint + ((DSPValue - 10) / 90 * (maxTint - minTint));
                }
               else if (RTPCName == "DSP_3")
                {
                    colorGrading.temperature.value = minTemp + ((DSPValue - 10) / 90 * (maxTemp - minTemp));
                }
                */

            if (RTPCName == "DSP_1")
                {
                    if (lightBend != null)
                    {
                        foreach (DSPLightBend bend in lightBend)
                        {
                            bend.AdjustSystemSlant(DSPValue);
                        }
                    }

                }
            else if (RTPCName == "DSP_2")
                {
                    topRot.AdjustSystemRotation(DSPValue);
                }
            else if (RTPCName == "DSP_3")
                {
                    brightShift.AdjustBackgroundBrightness(DSPValue);
                }


            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Controller")
        {
            Debug.Log("exited the dspbox!");
            AkSoundEngine.SetRTPCValue(RTPCName, 0);

            if (RTPCName == "DSP_1")
            {
                foreach (DSPLightBend bend in lightBend)
                {
                    bend.ReturnToNormal();
                }
            }
            else if (RTPCName == "DSP_2")
            {
                topRot.ReturnToZero();
            }
            else if (RTPCName == "DSP_3")
            {
                brightShift.ReturnToDropColor();
            }
            //StartCoroutine(ReturnToDefault(RTPCName));
            /*
            if (RTPCName == "DSP_1")
            {
                colorGrading.contrast.value = defaultContrast;
                colorGrading.brightness.value = defaultBright;
            }
            else if (RTPCName == "DSP_2")
            {
                colorGrading.tint.value = defaultTint;
            }
            else if (RTPCName == "DSP_3")
            {
                colorGrading.temperature.value = defaultTemp;
            }
            */
        }
    }
    /*
    IEnumerator ReturnToDefault(string whichDSP)
    {
        if (RTPCName == "DSP_1")
        {
            while (colorGrading.contrast.value > (defaultContrast + 1))
            {
                colorGrading.contrast.value -= returnToDefaultSpeed * Time.deltaTime;
                if (colorGrading.brightness.value > (defaultBright + 1))
                {
                    colorGrading.brightness.value -= returnToDefaultSpeed * Time.deltaTime;
                }
                yield return null;
            }
            while (colorGrading.brightness.value > (defaultBright + 1))
            {
                colorGrading.brightness.value -= returnToDefaultSpeed * Time.deltaTime;
                yield return null;
            }
            colorGrading.contrast.value = defaultContrast;
            colorGrading.brightness.value = defaultBright;

        }
        else if (RTPCName == "DSP_2")
        {
            if (colorGrading.tint.value > defaultTint)
            {
                while (colorGrading.tint.value > (defaultTint + 1))
                {
                    colorGrading.tint.value -= returnToDefaultSpeed * Time.deltaTime;
                    Debug.Log("tint value is " + colorGrading.tint.value);
                    yield return null;
                }
            }
            else
            {
                while (colorGrading.tint.value < defaultTint)
                {
                    colorGrading.tint.value += returnToDefaultSpeed * Time.deltaTime;
                    Debug.Log("tint value is " + colorGrading.tint.value);
                    yield return null;
                }
            }
            colorGrading.tint.value = defaultTint;
            Debug.Log("tint value has finally returned to " + colorGrading.tint.value);


        }
        else if (RTPCName == "DSP_3")
        {
            if (colorGrading.temperature.value > defaultTemp)
            {
                while (colorGrading.temperature.value > (defaultTemp + 1))
                {
                    colorGrading.temperature.value -= returnToDefaultSpeed * Time.deltaTime/3;
                    Debug.Log("temperature value is " + colorGrading.temperature.value);
                    yield return null;
                }
            }
            else
            {
                while (colorGrading.temperature.value < defaultTemp)
                {
                    colorGrading.temperature.value += returnToDefaultSpeed * Time.deltaTime/3;
                    Debug.Log("temperature value is " + colorGrading.temperature.value);
                    yield return null;
                }
            }
            colorGrading.temperature.value = defaultTemp;
            Debug.Log("temperature value has finally returned to " + colorGrading.temperature.value);
        }
        yield return null;
    }
    */
}
