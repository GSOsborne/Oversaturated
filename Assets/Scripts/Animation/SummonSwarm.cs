using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSwarm : MonoBehaviour
{
    public GameObject upDownPrefabColor;
    public int numOfSwarm;

    public float maxX, minX, maxY, minY, maxZ, minZ;

    // The length of time in seconds for all swarm elements to spawn
    public float spawnDuration = 7;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnSwarmElements());
    }

    private IEnumerator SpawnSwarmElements()
    {
        Vector3 origin = this.transform.position;
        Vector3[] relativePositions = new Vector3[numOfSwarm];

        for (int i = 0; i < numOfSwarm; i++)
        {
            float xValue = Random.Range(minX, maxX);
            float yValue = Random.Range(minY, maxY);
            float zValue = Random.Range(minZ, maxZ);
            Vector3 relativePos = new Vector3(xValue, yValue, zValue);
            relativePositions[i] = relativePos;
        }

        // Sorts by ascending distance from origin
        System.Array.Sort(relativePositions, new MagnitudeComparer());

        // Determines the max distance from origin
        float maxMagnitude = relativePositions[numOfSwarm-1].magnitude;

        // Calculates the growth of the spawn circle radius per second
        float spawnRadiusGrowthRate = maxMagnitude / spawnDuration;

        float spawnRadius = 0;
        int index = 0;
        while (index < numOfSwarm-1)
        {
            spawnRadius += spawnRadiusGrowthRate * Time.deltaTime;

            while (relativePositions[index].magnitude <= spawnRadius && index < numOfSwarm)
            {
                Vector3 randomPos = relativePositions[index++] + origin;
                GameObject particle = Instantiate(upDownPrefabColor, randomPos, Quaternion.identity/*, transform*/);
                //Debug.Log(new Vector3(xValue, yValue, zValue));
                particle.transform.parent = this.transform;
                particle.transform.position = randomPos;
                ColorFader fader = particle.GetComponent<ColorFader>();
                StartCoroutine(fader.FadeIn());
                //Debug.Log(particle.transform.position);
            }

            yield return spawnRadius;
        }
    }

    private class MagnitudeComparer : IComparer<Vector3>
    {
        public int Compare(Vector3 x, Vector3 y)
        {
            return System.Math.Sign(x.magnitude - y.magnitude);
        }
    }
}
