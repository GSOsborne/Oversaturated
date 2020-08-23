using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panelOptionPool;
    public float chosenPanelFadeFactor;

    bool dropFirstMeasurePlayed;

    private GameObject[] panels;

    private void Start()
    {

        // If panel randomizes twice on spawn, this is the culprit
        StereoRail_AudioManager.NewMeasureEvent += OnNewMeasure;
        StereoRail_AudioManager.WindupGestureRecieved += StartTheSpawning;
    }

    void StartTheSpawning()
    {
        if (!StereoRail_AudioManager.Instance.tutorialPreventingDrop)
        {
            if (panels == null)
            {
                //make sure we aren't over randomizing
                if (StereoRail_AudioManager.Instance.currentState == MusicState.Windup || StereoRail_AudioManager.Instance.currentState == MusicState.Filler)
                {
                    //Do nothing
                    Debug.Log("You woulda randomized extra but we stopped that.");
                }
                else
                {
                    SpawnRandomPanels();
                    //FadeInPanelHolder();
                }

            }
        }
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= OnNewMeasure;
        //StereoRail_AudioManager.WindupGestureRecieved -= StartTheSpawning;
    }

    private void Update()
    {

        /* changing this cuz of the new interaction machine -- Greg
        bool allPanelsNull = true;
        foreach (GameObject panel in panels)
        {
            if (panel)
            {
                allPanelsNull = false;
                break;
            }
        }
        if (allPanelsNull)
        {
            Debug.Log("No panels contained within holder; destroying holder");
            Destroy(gameObject);
        }
        */
    }

    // Checks if all child panels are ready to randomize; if any one of them is not, returns false
    private bool ReadyToRandomize()
    {
        bool readyToRandomize = true;
        //Debug.Log("panels container has " + panels[0]);
        if (panels != null)
        {
            foreach (GameObject panel in panels)
            {
                DropCharger dropCharger = panel.GetComponent<DropCharger>();
                readyToRandomize &= !dropCharger.IsCharging;
            }
        }
        else
        {
            readyToRandomize = false;
        }


        return readyToRandomize;
    }

    private void SpawnRandomPanels()
    {
            if (panelOptionPool.Length < 3)
            {
                throw new Exception("Panel option pool needs at least three panels");
            }

            DeleteAllPanels();

            GameObject[] chosenPanels = new GameObject[3];
            System.Random rand = new System.Random();
            int panelsSelected = 0;
            // Pick 3 random panels from panel pool
            for (int i = 0; i < panelOptionPool.Length; i++)
            {
                float probability = (float)(chosenPanels.Length - panelsSelected) / (panelOptionPool.Length - i);
                if (rand.NextDouble() <= probability)
                {
                    chosenPanels[panelsSelected++] = panelOptionPool[i];
                }

                if (panelsSelected >= chosenPanels.Length)
                {
                    break;
                }
            }

            // Shuffle order of array
            chosenPanels = CustomUtils.Reshuffle(new List<GameObject>(chosenPanels)).ToArray();

            panels = new GameObject[3];

            // TODO: Verify all of this below works
            // Get all placeholder children in prefab
            Transform thisTransform = gameObject.transform;
            GameObject leftPanel = thisTransform.GetChild(0).gameObject;
            GameObject rightPanel = thisTransform.GetChild(1).gameObject;
            GameObject middlePanel = thisTransform.GetChild(2).gameObject;

        // Spawn in randomly selected panel prefabs
        //StartCoroutine(SpreadSpawnOut(chosenPanels, leftPanel.transform.position, leftPanel.transform.rotation, rightPanel.transform.position, rightPanel.transform.rotation, middlePanel.transform.position, middlePanel.transform.rotation));
        
        panels[0] = Instantiate(chosenPanels[0], leftPanel.transform.position, leftPanel.transform.rotation, transform);

        panels[1] = Instantiate(chosenPanels[1], rightPanel.transform.position, rightPanel.transform.rotation, transform);

        panels[2] = Instantiate(chosenPanels[2], middlePanel.transform.position, middlePanel.transform.rotation, transform);
        

        // Destroy placeholders
        /*
        Destroy(leftPanel);
        Destroy(rightPanel);
        Destroy(middlePanel);
        */
    }
    /*
    IEnumerator SpreadSpawnOut(GameObject[] chosenPanels, Vector3 leftPanelPos, Quaternion leftPanelRot, Vector3 rightPanelPos, Quaternion rightPanelRot, Vector3 midPanelPos, Quaternion midPanelRot)
    {
        panels[0] = Instantiate(chosenPanels[0], leftPanelPos, leftPanelRot, transform);
        yield return null;
        panels[1] = Instantiate(chosenPanels[1], rightPanelPos, rightPanelRot, transform);
        yield return null;
        panels[2] = Instantiate(chosenPanels[2], midPanelPos, midPanelRot, transform);
    }
    */

    private void OnNewMeasure(MusicState state)
    {
        switch (state)
        {
            case MusicState.Windup:
                if (ReadyToRandomize())
                {
                    SpawnRandomPanels();
                }
                break;
            case MusicState.Groove:
                //FadeOutPanelHolder();
                DeleteAllPanels();
                dropFirstMeasurePlayed = false;
                break;
            case MusicState.Drop:
                if (!dropFirstMeasurePlayed) {
                    DeleteAllPanels();
                    Debug.Log("Deleting all the panels.");
                }


                dropFirstMeasurePlayed = true;

                break;

                // the fadeout that occurs when a drop is selected ought to be governed by the FadeOutPaneHolder(GameObject chosenPanel) method.
                //However, since we've been having difficulty getting it to cooperate, we've got a self-destruct function if it makes it to the second measure of the drop.
        }
    }

    public void DeleteAllPanels()
    {
        if (panels != null)
        {
            foreach (GameObject panel in panels)
            {
                Destroy(panel);
            }

        }
        panels = null;

    }
    /* obsolete because honestly this code was trash and these panels are invisible anyways now, we're using "DeleteAllPanels()" now.
    public void FadeOutPanelHolder(GameObject chosenPanel)
    {
        if (!chosenPanel)
        {
            throw new ArgumentException("Must provide non-null chosen panel");
        }
        //if (chosenPanel.CompareTag("GestureTrigger"))
        //{
            bool chosenPanelFound = false;
            foreach (GameObject panel in panels)
            {
                if (panel.Equals(chosenPanel))
                {
                    chosenPanelFound = true;
                    ColorFader currentFader = panel.GetComponent<ColorFader>();
                    if (currentFader)
                    {
                        currentFader.fadeSpeed *= chosenPanelFadeFactor;
                        Debug.Log(chosenPanel + " is the chosen Panel, fadeout speed = " + currentFader.fadeSpeed);

                    }
                }
            }
            if (chosenPanelFound)
            {
                StartAllFadeOuts();
            }
        //}
    }

    private void StartAllFadeOuts()
    {
        Debug.Log("Trying to fade out all these panels yo.");
        foreach (GameObject panel in panels)
        {
            //Debug.Log("Trying to fade out " + panel.name);
            if (!panel)
            {
                throw new InvalidOperationException("Panels under panel manager must not be null");
            }
            // If Drop has been selected, lock drop charging for all panels being faded out
            DropCharger dropCharger = panel.GetComponent<DropCharger>();
            dropCharger.ChargingDisabled = true;

            ColorFader currentFader = panel.GetComponent<ColorFader>();
            if (currentFader)
            {
                StartCoroutine(currentFader.FadeOut());
                //Debug.Log("Triggered FadeOut() on " + panel.name);
            }
            else
            {
                Debug.Log("Panel " + panel.name + " has no ColorFader to trigger for fade sequence.");
            }
        }
    }
    */

    public void FadeOutPanelHolder()
    {
        //StartAllFadeOuts();
        DeleteAllPanels();
    }


    //obsolete cuz of the new interaction machine
    /*
    private void FadeInPanelHolder()
    {
        foreach (GameObject panel in panels)
        {
            if (!panel)
            {
                throw new InvalidOperationException("Panels under panel manager must not be null");
            }
            ColorFader currentFader = panel.GetComponent<ColorFader>();
            if (currentFader)
            {
                StartCoroutine(currentFader.FadeIn());
            }
            else
            {
                Debug.Log("Panel " + panel.name + " has no ColorFader to trigger for fade sequence.");
            }
        }
    }
    */
}
