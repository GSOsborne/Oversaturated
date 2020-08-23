using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalShieldRespector : MonoBehaviour
{
    CapsuleCollider triggerCollider;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<CapsuleCollider>();
        //StartCoroutine(FadeOut());
    }
    

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "PersonalShield")
        {
            //Debug.Log("Entered your personal shield, sorry bout that!");
            StopCoroutine(FadeIn());
            StartCoroutine(FadeOut());

            //Also immediately want to disable trigger status just in case
            //triggerCollider.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "PersonalShield")
        {
            //Debug.Log("I'll get outta your way");
            StopCoroutine(FadeOut());
            StartCoroutine(FadeIn());

            //Also immediately want to disable trigger status just in case
            //triggerCollider.enabled = false;
        }
    }

    

    // the speed at which the transparency will change
    public float fadeSpeed = 5f;

    // Changes the opacity of the color at a fixed interval
    public IEnumerator FadeOut()
    {
        StopCoroutine(FadeIn());
        //Debug.Log("Called Fade Out");
        // Assigns the color as the material in the mesh rendere
        Material mat = this.GetComponent<MeshRenderer>().material;
        //Debug.Log(mat.color);
        //Debug.Log("I have started my coroutine!");

        // Loops until the object is transparent
        while (mat.color.a > 0f)
        {
            //StopCoroutine(FadeIn());
            //Debug.Log("Angling to Fade Out this bitch");
            Color newColor = mat.color;
            //Debug.Log("Old transparency: " + mat.color.a);
            newColor.a -= Time.deltaTime * fadeSpeed;
            // c.a = the opacity of the color 
                
            mat.color = newColor;
            //Debug.Log("New transparency: " + mat.color.a);


            yield return newColor;
        }

        // when the object is transparent it will be destroyed
        //Debug.Log("End of fade");

        //Object.Destroy(this.gameObject);
    }

    public IEnumerator FadeIn()
    {
        StopCoroutine(FadeOut());
        //Debug.Log("Called Fade In");
        Material mat = this.GetComponentInChildren<MeshRenderer>().material;
        Color invisibleColor = mat.color;
        invisibleColor.a = 0f;
        mat.color = invisibleColor;

        while (mat.color.a < 1)
        {
            Color newColor = mat.color;
            //Debug.Log("Old transparency: " + mat.color.a);
            newColor.a += Time.deltaTime * fadeSpeed;
            // c.a = the opacity of the color 
            //Debug.Log(newColor.a);
            //Debug.Log("Fighting to Fade In a motherfucker");
            mat.color = newColor;

            yield return newColor;
        }

        //now that done fading, use as collider
        //triggerCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
