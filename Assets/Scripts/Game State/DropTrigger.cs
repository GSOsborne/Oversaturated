using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Begone, thot!")]
public class DropTrigger : MonoBehaviour
{
    /*

    public string colorOfDrop;
    public GameObject dropSelectHolder;

    private StereoRail_AudioManager audMan;

    public float measureLength = .75f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject audioManagerObj = GameObject.Find("StereoRail_AudioManager");

        if (audioManagerObj)
        {
            audMan = audioManagerObj.GetComponent<StereoRail_AudioManager>();
        }

        if (audMan)
        {
            StartCoroutine("Last2measures");

            dropSelectHolder = this.transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I detect a collision!");


        if (other.tag == "Controller")
        {
            Debug.Log("Controller Hit!");
            //Vector3 velocityVector = other.GetComponent<Rigidbody>().velocity;
            //Debug.Log(velocityVector);



            
            Debug.Log("Drop " + colorOfDrop + " triggered!");
            TriggerTheDrop();
            DestroyDropSelector();




        }
    }

    void DestroyDropSelector()
    {
        Destroy(dropSelectHolder);
    }

    IEnumerator Last2measures()
    {
        yield return new WaitForSeconds(measureLength * 2);
        DestroyDropSelector();
        yield break;


      
    }

    void TriggerTheDrop()
    {
        if (audMan)
        {
            if (colorOfDrop == "red")
            {
                audMan.ChangeDropColor("red");
                audMan.TriggerDrop();
            }
            else if (colorOfDrop == "blue")
            {
                audMan.ChangeDropColor("blue");
                audMan.TriggerDrop();
            }
            else if (colorOfDrop == "green")
            {
                audMan.ChangeDropColor("green");
                audMan.TriggerDrop();
            }
        }
        else
        {
            Debug.Log("No AudioManager present in current scene");
        }
    }*/

}
