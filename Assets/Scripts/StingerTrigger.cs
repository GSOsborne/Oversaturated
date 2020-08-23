using System.Collections;
using System.Collections.Generic;
using static StereoRail_AudioManager;
using UnityEngine;

public class StingerTrigger : MonoBehaviour
{
    public Animation bounceAnim;
    private StereoRail_AudioManager audioManager;
    public StingerBouncer stingBounce;
    //public float speedMinimum = 0f;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = StereoRail_AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
         
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name + " is trying to enter stinger collider");
        if (other.CompareTag("Controller"))
        {
            audioManager.TriggerDropStinger();
            bounceAnim.Play();
            stingBounce.BounceThatBoi();
        }
    }
}
