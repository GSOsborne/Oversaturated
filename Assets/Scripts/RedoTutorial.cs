using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class RedoTutorial : MonoBehaviour
{
    public GestureTutorial gesTut;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            gesTut.EndPSTutorial();
            //will need to change this when we add more songs
            gesTut.TutorialBehaviour(SongName.PaperclipSwarm);
        }

    }

}
