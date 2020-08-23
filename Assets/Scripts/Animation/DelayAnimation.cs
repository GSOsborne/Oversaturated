using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnimation : MonoBehaviour
{
    Animation anim;
    public float delayTime;
    public string animationName;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        //Debug.Log("Delay Animation script active!");
        //StartCoroutine(DelayAnimationByFloat());
    }

    private void OnEnable()
    {
        //Debug.Log("Delay Animation script enabled!");
        StartCoroutine(DelayAnimationByFloat());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayAnimationByFloat()
    {

        yield return new WaitForSeconds(delayTime);
        Debug.Log("should have started delayed animation");
        anim.Play(animationName);

    }
}
