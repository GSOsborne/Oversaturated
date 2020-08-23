using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualOVRControllerFaking : MonoBehaviour
{
    public GameObject leftHandAnchor, rightHandAnchor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftHandAnchor.transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        rightHandAnchor.transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        leftHandAnchor.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        rightHandAnchor.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }
}
