using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DropColor = StereoRail_AudioManager.DropColor;

/// <summary>
/// For a given set of drop-related animation responses, controls when those animations can be triggered,
/// waiting for the proper combination of drop color and measure number.
/// </summary>
public class AudioreactiveDropListener : MonoBehaviour, ISerializationCallbackReceiver
{
    [Serializable]
    public struct ColorMeasurePair
    {
        public DropColor color;
        public int[] measures;

        public ColorMeasurePair(DropColor c, int[] m)
        {
            color = c;
            measures = m;
        }
    }

    public List<ColorMeasurePair> keyValuePairs;

    public Dictionary<DropColor, int[]> triggerMap;
    public AudioreactiveDropAnim[] responses;

    private int currentDropMeasure = 0;
    private DropColor lastDropColor;
    private int lastDropLength;
    private System.Random rand;

    public void OnBeforeSerialize()
    {
//<<<<<<< .merge_file_a01204
        keyValuePairs.Clear();
//=======
        keyValuePairs = new List<ColorMeasurePair>();
//>>>>>>> .merge_file_a29528

        foreach (var kvp in triggerMap)
        {
            keyValuePairs.Add(new ColorMeasurePair(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        triggerMap = new Dictionary<DropColor, int[]>();

        foreach (ColorMeasurePair cmp in keyValuePairs)
        {
//<<<<<<< .merge_file_a01204
//=======
            string measures = "";
            foreach (int measure in cmp.measures)
            {
                measures += measure + " ";
            }
            //Debug.Log("Adding pair: " + cmp.color.ToString() + " " + measures);
//>>>>>>> .merge_file_a29528
            triggerMap.Add(cmp.color, cmp.measures);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.TriggerDropEvent += OnTriggerDrop;
        rand = new System.Random();
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
        StereoRail_AudioManager.TriggerDropEvent -= OnTriggerDrop;
    }

    private void OnTriggerDrop(DropColor dropColor, int dropLength)
    {
        lastDropColor = dropColor;
        lastDropLength = dropLength;
    }
    
    private void OnNewMeasure(MusicState state)
    {
        if (state == MusicState.Drop)
        {
            currentDropMeasure++;
            if (IsTriggerMatch())
            {
                if (responses == null || responses.Length < 1)
                {
                    return;
                }
                int randIndex = rand.Next(responses.Length);
                responses[randIndex]?.TriggerDropAnimation(lastDropColor, lastDropLength);
//<<<<<<< .merge_file_a01204
//=======
                if (responses[randIndex] == null)
                {
                    Debug.Log("Null drop anim found at index " + randIndex);
                }
                else
                {
                    Debug.Log("Triggering drop animation " + randIndex);
                }
//>>>>>>> .merge_file_a29528
            }

        }
        else
        {
            currentDropMeasure = 0;
        }
    }

    private bool IsTriggerMatch()
    {
        if (triggerMap == null)
        {
            return true;
        }
        else if (!triggerMap.ContainsKey(lastDropColor))
        {
            return false;
        }
        else if (triggerMap[lastDropColor] == null || triggerMap[lastDropColor].Length == 0)
        {
            return true;
        }
        else
        {
            return Array.IndexOf(triggerMap[lastDropColor], currentDropMeasure) != -1;
        }
    }
}
