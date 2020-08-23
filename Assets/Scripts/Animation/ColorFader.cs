using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFader : MonoBehaviour
{
    // the speed at which the transparency will change
    public float fadeSpeed = 5f;
    public float targetOpacity;
    public float aValue;

    private void Start()
    {
        //Debug.Log(gameObject.name + " has active Color Fader Script!");
    }

    // Changes the opacity of the color at a fixed interval
    public IEnumerator FadeOut()
    {
        //Debug.Log(gameObject.name + " is beginning fadeout!");
        //StopCoroutine(FadeIn()); // doesn't seem to work anyways
        // Assigns the color as the material in the mesh rendere
        Material mat = this.GetComponentInChildren<MeshRenderer>().material;
        //Debug.Log(mat.color);
        //Debug.Log("I have started my coroutine!");


        // Loops until the object is transparent
        while (mat.color.a > 0.07f)
        {

            Color newColor = mat.color;
            newColor.a -= Time.deltaTime * fadeSpeed;
            // c.a = the opacity of the color 
            //Debug.Log(newColor.a);
            mat.color = newColor;
            aValue = newColor.a;

            yield return newColor;
        }

        // when the object is transparent it will be destroyed
        //Debug.Log("Destroying: " + gameObject.name);
        Destroy(gameObject);
    }

    public IEnumerator FadeIn()
    {
        //Debug.Log(gameObject.name + " is beginning fade in!");
        //StopCoroutine(FadeOut());  // doesn't seem to work anyways
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        Color invisibleColor = mat.color;
        invisibleColor.a = 0f;
        mat.color = invisibleColor;

        while (mat.color.a < targetOpacity)
        {
            Color newColor = mat.color;
            newColor.a += Time.deltaTime * fadeSpeed;
            // c.a = the opacity of the color 
            //Debug.Log(newColor.a);
            mat.color = newColor;
            aValue = newColor.a;

            yield return newColor;
        }
    }
}
