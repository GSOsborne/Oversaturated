using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatOffset : MonoBehaviour
{

    Animator upDownCubeAnimator;

    public float maxOffset = .2f;
    public float minOffset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        upDownCubeAnimator = GetComponent<Animator>();
        float random = Random.Range(minOffset, maxOffset);
        upDownCubeAnimator.SetFloat("CycleOffset", random);
        //Debug.Log(random);
        //upDownCubeAnimator.Play("UpDown", 0, 0.25f);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
