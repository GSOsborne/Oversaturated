using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandVelocity : MonoBehaviour
{

    /// <summary>
    /// The current velocity of the VR controller
    /// </summary>
    //public bool checkThisIfLeftController;


    public Vector3 velocity;

    /// <summary>
    /// The current position of the VR controller
    /// </summary>
    public Vector3 handPosition;
    XRController controller;

    private Vector3 prevPos;

    // Start is called before the first frame update
    private void Start()
    {
        // Initalize values
        controller = GetComponent<XRController>();
        //HandPosition = gameObject.transform.position;
        //prevPos = gameObject.transform.position;
        velocity = Vector3.zero;
    }

    private void Update()
    {

        controller.inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out handPosition);
        controller.inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocity);
    }
}