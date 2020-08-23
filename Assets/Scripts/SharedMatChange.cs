using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedMatChange : MonoBehaviour
{
    public Color colorChange;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial.SetColor("_Color", colorChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
