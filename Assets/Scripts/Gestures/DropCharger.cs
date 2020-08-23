using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class DropCharger : MonoBehaviour
{
    public float punchSpeed = 1;

    public float ChargeValue { get; private set; } = 0f;
    public bool IsCharging { get; private set; } = false;
    // Charge percentage per second when the player is charging the drop
    private float chargeSpeed = 2.0f;
    // Charge percentage lost per second when the player is not charging the drop
    private float decaySpeed = 1.0f;
    // The charge percentage at which the drop will activate on a new measure
    private float dropThreshold = .98f;
    public bool ChargingDisabled { get; set; }

    public DropColor colorOfDrop;
    private MusicState currentState;

    private StereoRail_AudioManager audioManager;
    private PanelManager parentManager;

    bool alreadyResetToGroove;
    bool acceptingNewInputs;



    Animation chargeAnim;

    // Update is called once per frame
    private void Update()
    {
        UpdateCharge();
        if (!ChargingDisabled)
        {
            UpdateNextState();
        }
    }

    private void Start()
    {
        ChargingDisabled = false;
        audioManager = StereoRail_AudioManager.Instance;
        NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.DropGestureRecieved += DropGestureRecieved;

        chargeAnim = GetComponent<Animation>();
        //Make sure you have attached your animation in the Animations attribute
        chargeAnim.Play("DroptionChargeIntensity");
        chargeAnim["DroptionChargeIntensity"].speed = 0;

        alreadyResetToGroove = false;
        acceptingNewInputs = true;
    }

    private void OnDestroy()
    {
        NewMeasureEvent -= OnNewMeasure;
        StereoRail_AudioManager.DropGestureRecieved -= DropGestureRecieved;
    }

    private void UpdateNextState()
    {
        if (audioManager == null)
        {
            audioManager = StereoRail_AudioManager.Instance;
        }

        if (IsCharging && audioManager.nextState == MusicState.Groove)
        {
            audioManager.nextState = MusicState.Filler;
            alreadyResetToGroove = false;
            Debug.Log("CHARGING SO NEXT STATE IS FILLER!!!");
        }
        else if (!IsCharging && audioManager.currentState == MusicState.Filler)
        {
            if (!alreadyResetToGroove)
            {
                alreadyResetToGroove = true;
                audioManager.nextState = MusicState.Groove;
            }
        }
    }

    private void UpdateCharge()
    {
        if (!ChargingDisabled)
        {
            if (IsCharging)
            {
                ChargeValue = Mathf.Clamp(ChargeValue + Time.deltaTime * chargeSpeed, 0f, 1f);
            }
            else
            {
                ChargeValue = Mathf.Clamp(ChargeValue - Time.deltaTime * decaySpeed, 0f, 1f);
            }
            chargeAnim["DroptionChargeIntensity"].normalizedTime = ChargeValue;
            //Debug.Log("Drop Charge Value is: " + ChargeValue);
            if ((currentState == MusicState.Windup || currentState == MusicState.Filler) && ChargeValue >= dropThreshold)
            {
                TriggerDrop();
                Debug.Log("Triggered the drop of " + gameObject.name + "from charging");
                ChargingDisabled = true;
            }

        }
    }

    private void OnNewMeasure(MusicState state)
    {
        currentState = state;
    }

    private void DropGestureRecieved()
    {
        ChargingDisabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller") && acceptingNewInputs)
        {
            IsCharging = true;
            //Debug.Log(OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude + ", " + OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude);
            if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude > punchSpeed || OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude > punchSpeed)
            {

                Debug.Log(gameObject.name + " was PUNCHED!");
                //chargeSpeed = 8f;
                TriggerDrop();
                ChargingDisabled = true;
                QuickChargeAnim();
                acceptingNewInputs = false;
                
            }
        }
    }

    private void QuickChargeAnim()
    {
        chargeAnim["DroptionChargeIntensity"].speed = 1.5f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            IsCharging = false;
        }
    }

    public void TriggerDrop()
    {
        if (audioManager == null)
        {
            audioManager = StereoRail_AudioManager.Instance;
        }

        if (parentManager == null)
        {
            GameObject myParent = transform.parent.gameObject;
            parentManager = myParent.GetComponent<PanelManager>();
        }

        // Trigger the drop and fade out this panel's manager
        audioManager.TriggerDrop(colorOfDrop, audioManager.nextDropLength);
        //audioManager.DropGestureRecievedCall();
        //parentManager.FadeOutPanelHolder(gameObject);
        parentManager.DeleteAllPanels();
        Debug.Log("Sent Drop Trigger, color is: " + colorOfDrop);
    }

}
