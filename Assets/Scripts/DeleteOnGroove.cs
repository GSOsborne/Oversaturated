using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StereoRail_AudioManager;

public class DeleteOnGroove : MonoBehaviour
{
    public float deleteSpeed;
    // Start is called before the first frame update
    void Start()
    {
        StereoRail_AudioManager.NewMeasureEvent += CheckForGroove;
    }

    void CheckForGroove(MusicState mState)
    {
        if (mState == MusicState.Groove)
        {
            StartCoroutine(ShrinkTillDeath());
        }
    }

    IEnumerator ShrinkTillDeath()
    {
        Transform transform = GetComponent<Transform>();
        while (transform.localScale.x > .05)
        {
            transform.localScale = new Vector3(transform.localScale.x - deleteSpeed * Time.deltaTime, transform.localScale.y - deleteSpeed * Time.deltaTime, transform.localScale.z - deleteSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StereoRail_AudioManager.NewMeasureEvent -= CheckForGroove;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
