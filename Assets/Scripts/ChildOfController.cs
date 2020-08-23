using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOfController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Child of the controller is also still here");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided!");
        /*
        if (other.tag == "WindupCollider")
        {
            Debug.Log("Windup Triggered!");
        }
        */
    }
}
