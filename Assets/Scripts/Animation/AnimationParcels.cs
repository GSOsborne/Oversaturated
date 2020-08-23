using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationParcels : MonoBehaviour
{
    public GameObject GreenUpDownSwarmPrefab;
    public GameObject RedUpDownSwarmPrefab;
    public GameObject BlueUpDownSwarmPrefab;
    public int numGreenSwarm;
    public int numRedSwarm;
    public int numBlueSwarm;
    public float maxX, minX, maxY, minY, maxZ, minZ;

    public GameObject[] shapeHolder = null;


    // Start is called before the first frame update
    void Awake()
    {
        shapeHolder = null;
        for (int x = 0; x < numGreenSwarm; x++)
        {
            GameObject greenSwarm = Instantiate(GreenUpDownSwarmPrefab, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ)), Quaternion.identity);
            greenSwarm.transform.parent = this.transform;
        }
        for (int x = 0; x < numRedSwarm; x++)
        {
            GameObject redSwarm = Instantiate(RedUpDownSwarmPrefab, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ)), Quaternion.identity);
            redSwarm.transform.parent = this.transform;
        }
        for (int x = 0; x < numBlueSwarm; x++)
        {
            GameObject blueSwarm = Instantiate(BlueUpDownSwarmPrefab, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ)), Quaternion.identity);
            blueSwarm.transform.parent = this.transform;
        }
    }

    void Start()
    {
        StartCoroutine(FindAllTheShapes());
    }

    IEnumerator FindAllTheShapes()
    {
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("Yeah I started allright");
        if (shapeHolder == null)
        {
            shapeHolder = GameObject.FindGameObjectsWithTag("shapeAnimator");
            if (shapeHolder.Length == 0)
            {
                Debug.Log("I got nothing chief");
            }
            else
            {
                Debug.Log("Chief I found: " + shapeHolder.Length);
            }

        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ScalePulse()
    {
        //BroadcastMessage("PulseOnce");
        foreach (GameObject shape in shapeHolder)
        {
            shape.GetComponent<CubeAnimator>().PulseOnce();
        }

    }

    public void RotatePulse()
    {
        //Debug.Log("Should be rotating");
        //BroadcastMessage("RotateOnce");
        foreach (GameObject shape in shapeHolder)
        {
            shape.GetComponent<CubeAnimator>().RotateOnce();
            //Debug.Log(shape);
        }
    }

    public void FasterPulse()
    {
        //BroadcastMessage("FasterForABit");
        foreach (GameObject shape in shapeHolder)
        {
            shape.GetComponent<CubeAnimator>().FasterForABit();
        }
    }

    public void BlueDropStart(int numberOfMeasures)
    {
        //BroadcastMessage("BlueDropChoreography", numberOfMeasures);
        foreach (GameObject shape in shapeHolder)
        {
            shape.GetComponent<CubeAnimator>().BlueDropChoreography(numberOfMeasures);
        }
    }



    public void RandomAnimationSelect()
    {
        int randomAnimation = Random.Range(0, 3);
        if (randomAnimation == 0)
        {
            ScalePulse();
        }
        if (randomAnimation == 1)
        {
            RotatePulse();
        }
        if (randomAnimation == 2)
        {
            FasterPulse();
        }
    }
}
