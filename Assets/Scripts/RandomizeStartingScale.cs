using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeStartingScale : MonoBehaviour
{
    public float minScale, maxScale;
    // Start is called before the first frame update
    void Start()
    {
        Transform transform = GetComponent<Transform>();
        transform.localScale = new Vector3(Random.Range(minScale, maxScale), Random.Range(minScale, maxScale), Random.Range(minScale, maxScale));
        //Debug.Log(transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
