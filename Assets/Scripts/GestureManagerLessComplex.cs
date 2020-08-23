using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class GestureManagerLessComplex : MonoBehaviour
{
    public HandVelocity leftHand, rightHand;
    //LineRenderer gestureVectorRenderer;
    // Minimum velocity that can trigger a gesture
    public float windupTriggerVelocity = 0;
    //public float maxDistance = Mathf.Infinity;

    //private RaycastHit hit;
    //private Ray ray;
    private Vector3 velocity;

    //bool acceptNewRender;
    //bool renderRelevant;
    bool inMiddleOfDrop;
    bool recentlyTriggered = false;

    private StereoRail_AudioManager audioManager;

    //private readonly IGestureType EmptyGesture = new EmptyGesture();
    //private const string GestureTriggerTag = "GestureTrigger";

    private void Start()
    {
        //gestureVectorRenderer = GetComponent<LineRenderer>();
        //gestureVectorRenderer.enabled = false;
        //acceptNewRender = true;
        //renderRelevant = false;
        //StereoRail_AudioManager.StartSongEvent += AllowLineDrawing;
        //StereoRail_AudioManager.StopSongEvent += DisallowLineDrawing;

        audioManager = StereoRail_AudioManager.Instance;
        StereoRail_AudioManager.TriggerDropEvent += DropActiveMeasuring;
    }

    private void OnDestroy()
    {
        //StereoRail_AudioManager.StartSongEvent -= AllowLineDrawing;
        //StereoRail_AudioManager.StopSongEvent -= DisallowLineDrawing;
        StereoRail_AudioManager.NewMeasureEvent -= AreWeInDrop;
    }

    // Update is called once per frame
    private void Update()
    {
        if(leftHand.velocity.magnitude > windupTriggerVelocity || rightHand.velocity.magnitude > windupTriggerVelocity)
        {
            if (!recentlyTriggered)
            {
                audioManager.TriggerWindup();
                StartCoroutine(PreventMachineGun());
            }

        }
        /*
        IGestureType gesture = GetGestureType();
        gesture.ExecuteEvent();
        */
    }

    IEnumerator PreventMachineGun()
    {
        recentlyTriggered = true;
        yield return new WaitForSeconds(.2f);
        recentlyTriggered = false;
    }

    /*
    private IGestureType GetGestureType()
    {
        
         // TODO: Raycast out in the direction of the gesture from the controller, check the velocity, and
         // if a valid IGestureType is found on the target, return it; otherwise, return an EmptyGesture
       

        IGestureType returnGesture = null;

        if (leftHand.Velocity.magnitude >= minimumVelocity)
        {
            velocity = leftHand.Velocity;
            //ray = new Ray(leftHand.HandPosition, velocity);
            //returnGesture = RetrieveGesture();
        }

        if (returnGesture == null || returnGesture.Equals(EmptyGesture))
        {
            if (rightHand.Velocity.magnitude >= minimumVelocity)
            {
                velocity = rightHand.Velocity;
                //ray = new Ray(rightHand.HandPosition, velocity);
                //returnGesture = RetrieveGesture();
            }
            else
            {
                returnGesture = EmptyGesture;
            }
        }

        return returnGesture;
    }
    */
    /*
    private IGestureType RetrieveGesture()
    {
        if (Physics.Raycast(ray, out hit, maxDistance) && hit.transform.gameObject.CompareTag(GestureTriggerTag))
        {
            //Debug.Log("Hit object " + hit.transform.gameObject.name + " with raycast at velocity vector: " + velocity + " magnitude was: " + velocity.magnitude);

            ComponentGesture targetGesture = hit.transform.gameObject.GetComponent<ComponentGesture>();
            if (targetGesture == null)
            {
                throw new System.Exception("Expected IGestureType script not found on object " + hit.transform.gameObject.name);
            }
            if (targetGesture.GesturesEnabled && targetGesture.MeetsTriggerSpeed(velocity))
            {
                //Debug.Log("Accepted by target gesture " + targetGesture.GetType().ToString() + " with velocity: " + velocity + " and the magnitude was: " + velocity.magnitude);

                //since this is only currently being used for windups, I'm adding logic to make it only draw at relevant times.  
                /*
                if (acceptNewRender && renderRelevant && !inMiddleOfDrop)
                {
                    gestureVectorRenderer.enabled = true;
                    acceptNewRender = false;
                    gestureVectorRenderer.SetPosition(0, ray.origin);
                    gestureVectorRenderer.SetPosition(1, hit.point);
                    //StartCoroutine(DeleteRayAfterPause());
                } 
                return targetGesture;

            }
        }
        

        return EmptyGesture;
    }
    */

    /*
    IEnumerator DeleteRayAfterPause()
    {
        yield return new WaitForSeconds(0.2f);
        acceptNewRender = true;
        yield return new WaitForSeconds(0.3f);
        gestureVectorRenderer.enabled = false;
    }

    void AllowLineDrawing(SongName songName)
    {
        if (songName == SongName.PaperclipSwarm)
        {
            renderRelevant = true;
        }
    }
    void DisallowLineDrawing()
    {
        renderRelevant = false;
        StereoRail_AudioManager.NewMeasureEvent -= AreWeInDrop;

    }
    */

    void DropActiveMeasuring(DropColor dropColor, int integer)
    {
        StereoRail_AudioManager.NewMeasureEvent += AreWeInDrop;
    }

    void AreWeInDrop(MusicState currentState)
    {
        switch (currentState)
        {
            case MusicState.Windup:
            case MusicState.Filler:
            case MusicState.Groove:
                inMiddleOfDrop = false;
                StereoRail_AudioManager.NewMeasureEvent -= AreWeInDrop;
                break;
            case MusicState.Drop:
                inMiddleOfDrop = true;
                break;
        }
    }

    /*
     * TODO: Enable the following functionalities:
     * - Raycast from hand in direction of velocity vector
     * - Check the type of gesture based on velocity magnitude and raycast
     * - Trigger the appropriate event based on the type of gesture (make a gesture an interface???)
     */
}

