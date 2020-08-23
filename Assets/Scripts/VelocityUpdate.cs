using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityUpdate : MonoBehaviour
{
    Rigidbody rb;
    Vector3 velocityVector;
    public float velocityMagnitude;
    Vector3 previousTransformPosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousTransformPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        velocityVector = previousTransformPosition - transform.position;
        //Debug.Log("Velocity Vector is " + velocityVector);
        velocityMagnitude = velocityVector.magnitude;

        //Debug.Log(previousTransformPosition - transform.position);
        previousTransformPosition = transform.position;

    }
}
