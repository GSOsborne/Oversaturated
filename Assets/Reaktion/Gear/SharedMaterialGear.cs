//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;
using static StereoRail_AudioManager;

namespace Reaktion {

[AddComponentMenu("Reaktion/Gear/Material Gear")]
public class SharedMaterialGear : MonoBehaviour
{



    public enum TargetType { Color, Float, Vector, Texture }

    public ReaktorLink reaktor;

    public int materialIndex;

    public string targetName = "_Color";
    public TargetType targetType = TargetType.Color;

    public float threshold = 0.5f;

    public Gradient colorGradient;

    public AnimationCurve floatCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public Vector4 vectorFrom = Vector4.zero;
    public Vector4 vectorTo = Vector4.one;

    public Texture textureLow;
    public Texture textureHigh;

    Material material;

        public float startMinIntensity, startMaxIntensity, finalMinIntensity, finalMaxIntensity, incrementValue;
        float currentMinIntensity, currentMaxIntensity, reaktionIntensity;
        bool firstGrooveMeasurePassed;

        void Awake()
    {
        reaktor.Initialize(this);

        if (materialIndex == 0)
            material = GetComponent<Renderer>().sharedMaterial;
        else
            material = GetComponent<Renderer>().sharedMaterials[materialIndex];

        UpdateMaterial(0);
    }

        void Start()
        {
            StereoRail_AudioManager.TriggerDropEvent += StartFadeToDropIntensity;
            StereoRail_AudioManager.StartSongEvent += ResetMinMaxIntensity;
            StereoRail_AudioManager.NewMeasureEvent += CheckForGroove;
            ResetMinMaxIntensity(SongName.PaperclipSwarm);
            firstGrooveMeasurePassed = true;
        }

        void Update()
    {
        UpdateMaterial(reaktor.Output);
    }

    void UpdateMaterial(float param)
    {
        switch (targetType)
        {
        case TargetType.Color:
            material.SetColor(targetName, colorGradient.Evaluate(param) * reaktionIntensity);
            break;
        case TargetType.Float:
            material.SetFloat(targetName, floatCurve.Evaluate(param));
            break;
        case TargetType.Vector:
            material.SetVector(targetName, Vector4.Lerp(vectorFrom, vectorTo, param));
            break;
        case TargetType.Texture:
            material.SetTexture(targetName, param < threshold ? textureLow : textureHigh);
            break;
        }
    }

        void ResetMinMaxIntensity(SongName songName)
        {
            currentMinIntensity = startMinIntensity;
            currentMaxIntensity = startMaxIntensity;
            reaktionIntensity = currentMinIntensity;
        }

        void CheckForGroove(MusicState givenState)
        {
            if (givenState == MusicState.Groove)
            {
                if (!firstGrooveMeasurePassed)
                {
                    StartCoroutine(DropIntensityOnGroove());
                    firstGrooveMeasurePassed = true;
                }
            }
            else if (givenState == MusicState.Drop)
            {
                firstGrooveMeasurePassed = false;
            }
        }

        IEnumerator DropIntensityOnGroove()
        {
            for (float i = 0; i <= 10; i++)
            {
                reaktionIntensity = currentMaxIntensity - i / 10 * (currentMaxIntensity - currentMinIntensity);
                //Debug.Log("current cam color is: " + cam.backgroundColor);
                Debug.Log("Fading Out: " + (currentMaxIntensity - i / 10 * (currentMaxIntensity - currentMinIntensity)));
                yield return null;
            }
        }

        void StartFadeToDropIntensity(DropColor dColor, int dropLength)
        {
            StartCoroutine(FadeUpToDropIntensity());
        }

        IEnumerator FadeUpToDropIntensity()
        {
            for (float i = 0; i <= 10; i++)
            {
                reaktionIntensity = currentMinIntensity + i / 10 * (currentMaxIntensity - currentMinIntensity);
                //Debug.Log("current cam color is: " + cam.backgroundColor);
                //Debug.Log("Fading In: " + currentMinIntensity + i / 10 * (currentMaxIntensity - currentMinIntensity));
                yield return null;
            }
        }
    }



} // namespace Reaktion
