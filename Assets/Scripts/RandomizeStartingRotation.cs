using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeStartingRotation : MonoBehaviour
{
    public float minRotation, maxRotation;
    // Start is called before the first frame update
    void Start()
    {
        Transform transform = GetComponent<Transform>();
        transform.rotation = new Quaternion(Random.Range(minRotation, maxRotation), Random.Range(minRotation, maxRotation), Random.Range(minRotation, maxRotation), Random.Range(minRotation, maxRotation));
        //Debug.Log(transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
