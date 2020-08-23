using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVelocity : MonoBehaviour
{

    /// <summary>
    /// The current velocity of the VR controller
    /// </summary>
    public bool checkThisIfLeftController;
        

    public Vector3 Velocity { get; private set; }

    /// <summary>
    /// The current position of the VR controller
    /// </summary>
    public Vector3 HandPosition { get; private set; }

    private Vector3 prevPos;

    // Start is called before the first frame update
    private void Start()
    {
        // Initalize values
        if (checkThisIfLeftController)
        {
            HandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            prevPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        }
        else
        {
            HandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            prevPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        }
        //HandPosition = gameObject.transform.position;
        //prevPos = gameObject.transform.position;
        Velocity = Vector3.zero;
    }

    private void Update()
    {
        // Calculate instantaneous velocity of gesture
        if (checkThisIfLeftController)
        {
            HandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            //Debug.Log("Are we tracking the Left? " + OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch));
            Velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            //Debug.Log("Velocity of left controller is: " + Velocity);
        }
        else
        {
            HandPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            //Debug.Log("Are we tracking the Right? " + OVRInput.GetControllerPositionTracked(OVRInput.Controller.LTouch));
            Velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            //Debug.Log("Velocity of right controller is: " + Velocity);
        }
        //Vector3 posDelta = HandPosition - prevPos;
        //Velocity = (Time.deltaTime == 0) ? Vector3.zero : posDelta / Time.deltaTime;

        //Debug.Log("posDelta of " + posDelta.ToString() + " recorded!");
        //Debug.Log("HandPosition is: " + HandPosition);
        // Set prevPos for next iteration
        //prevPos = HandPosition;
    }
}