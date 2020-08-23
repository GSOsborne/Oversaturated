using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public abstract class ModularParticleSystem : MonoBehaviour
{
    protected ParticleSystem targetParticleSystem;
    protected MusicState latestState;
    protected int measuresInState = 0;

    private void UpdateVariables(MusicState state)
    {
        if (state == latestState)
        {
            measuresInState++;
        }
        else
        {
            measuresInState = 1;
            latestState = state;
        }
    }

    private void OnNewMeasure(MusicState state)
    {
        OnMeasureTransition(latestState, state);
        UpdateVariables(state);
        UpdateForNewMeasure();
    }

    // Override this method for additional processing on new measure
    protected abstract void UpdateForNewMeasure();

    // Override this method for info on previous music state
    protected abstract void OnMeasureTransition(MusicState oldState, MusicState newState);

    protected abstract void Initialize();

    // Start is called before the first frame update
    protected void Start()
    {
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        targetParticleSystem = GetComponent<ParticleSystem>();
        if (targetParticleSystem == null)
        {
            Debug.Log("No particle system found on object " + transform.name);
        }
        Initialize();
    }

    protected void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
    }
}
