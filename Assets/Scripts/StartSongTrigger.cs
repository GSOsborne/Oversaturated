using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSongTrigger : MonoBehaviour
{
    public StereoRail_AudioManager.SongName thisSongName;
    CapsuleCollider triggerCollider;
    public GameObject startSongText;

    // Start is called before the first frame update
    void Start()
    {
        StereoRail_AudioManager.StopSongEvent += StartFadeInCoroutine;
        StereoRail_AudioManager.StartSongEvent += StartFadeOutCoroutine;
        triggerCollider = GetComponent<CapsuleCollider>();
        startSongText.SetActive(true);
        StartCoroutine(FadeIn());
    }

    void StartFadeInCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
        
    }
    void StartFadeOutCoroutine(StereoRail_AudioManager.SongName whichSong)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something has tried to enter this song collider *SLURP*");
        if (other.tag == "Controller")
        {
            StereoRail_AudioManager.StartASong(thisSongName);

            //Also immediately want to disable trigger status just in case
            triggerCollider.enabled = false;
        }
    }

    // the speed at which the transparency will change
    public float fadeSpeed = 0.5f;

    // Changes the opacity of the color at a fixed interval
    public IEnumerator FadeOut()
    {
        Debug.Log("Called Fade Out");
        // Assigns the color as the material in the mesh rendere
        Material mat = this.GetComponentInChildren<MeshRenderer>().material;
        Debug.Log(mat.color);
        Debug.Log("I have started my coroutine!");

        // Loops until the object is transparent
        while (mat.color.a > 0)
        {

            Color newColor = mat.color;
            newColor.a -= Time.deltaTime * fadeSpeed;
            // c.a = the opacity of the color 
            //Debug.Log(newColor.a);
            mat.color = newColor;

            yield return newColor;
        }
        startSongText.SetActive(false);
        // when the object is transparent it will be destroyed
        Debug.Log("End of fade");

        //Object.Destroy(this.gameObject);
    }

    public IEnumerator FadeIn()
    {
        startSongText.SetActive(true);
        Debug.Log("Called Fade In");
        Material mat = this.GetComponentInChildren<MeshRenderer>().material;
        Color invisibleColor = mat.color;
        invisibleColor.a = 0f;
        mat.color = invisibleColor;

        while (mat.color.a < 1)
        {
            Color newColor = mat.color;
            newColor.a += Time.deltaTime * fadeSpeed;
            // c.a = the opacity of the color 
            //Debug.Log(newColor.a);
            mat.color = newColor;

            yield return newColor;
        }

        //now that done fading, use as collider
        triggerCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
