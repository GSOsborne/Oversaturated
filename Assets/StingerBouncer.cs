using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingerBouncer : MonoBehaviour
{
    //Animation anim;
    Transform trans;
    public float downSpeed, returnSpeed, lowestPoint;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animation>();
        trans = GetComponent<Transform>();
    }

    public void BounceThatBoi()
    {
        //anim.Play();
        trans.position = new Vector3(0, Mathf.Max(trans.position.y - downSpeed, lowestPoint), 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.anyKey)
        {
            Debug.Log("Bouncing the BOIIIIIIII");
            BounceThatBoi();
        }
        */
        if (trans.position.y < .1)
        {
            trans.position = new Vector3(0, trans.position.y + returnSpeed, 0);
        }
    }


}
