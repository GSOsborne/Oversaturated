using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindupTrigger : MonoBehaviour
{

    float velocityFloat;
    public float hitSensitivity;
    public GameObject dropSelectPrefab;

    public GameObject cameraParent;

    public StereoRail_AudioManager AudMan;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Windup trigger box initialized");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I detect a collision!");


        if (other.tag == "Controller")
            {
                //Debug.Log("Controller Hit!");
            //Vector3 velocityVector = other.GetComponent<Rigidbody>().velocity;
            //Debug.Log(velocityVector);


            velocityFloat = other.GetComponent<VelocityUpdate>().velocityMagnitude;
            Debug.Log("Hit windup with velocity: " + velocityFloat);

            if (velocityFloat > hitSensitivity)
            {
                Debug.Log("Windup Triggered!");
                AudMan.TriggerWindup();

                //Object.Instantiate(dropSelectPrefab, cameraParent.transform);
            }


            }
    }
}
