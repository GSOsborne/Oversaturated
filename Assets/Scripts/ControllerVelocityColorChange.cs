using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class ControllerVelocityColorChange : MonoBehaviour
{
    public float triggerSpeed;
    Animation colorAnim;

    public Vector3 Velocity { get; private set; }
    public Vector3 HandPosition { get; private set; }

    public bool checkThisIfLeftController;

    private Vector3 prevPos;

    bool windupGestureReceptionPause;


    // Start is called before the first frame update
    void Start()
    {
        colorAnim = GetComponent<Animation>();
        colorAnim.Play("VelocityColor");
        colorAnim["VelocityColor"].speed = 0;
        colorAnim["VelocityColor"].normalizedTime = 0;

        Velocity = Vector3.zero;
        windupGestureReceptionPause = false;

        StereoRail_AudioManager.WindupGestureRecieved += PauseAtMax;
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.WindupGestureRecieved -= PauseAtMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkThisIfLeftController)
        {
            Velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            //Debug.Log("Velocity of left controller is: " + Velocity);
        }
        else
        {
            Velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            //Debug.Log("Velocity of right controller is: " + Velocity);
        }
        if (!windupGestureReceptionPause)
        {
            colorAnim["VelocityColor"].normalizedTime = Mathf.Clamp(Velocity.magnitude / triggerSpeed, 0f, 1f);
            //Debug.Log(Velocity.magnitude / triggerSpeed);
        }
    }

    private void PauseAtMax()
    {
        StartCoroutine(PauseAtMaxRoutine());
    }

    IEnumerator PauseAtMaxRoutine()
    {
        windupGestureReceptionPause = true;
        colorAnim["VelocityColor"].normalizedTime = 1f;
        yield return new WaitForSeconds(.2f);
        windupGestureReceptionPause = false;
    }
}
