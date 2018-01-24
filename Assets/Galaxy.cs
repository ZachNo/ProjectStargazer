using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour {

    int ss;
    float size;

    GameObject[] ssObjects;

    // Use this for initialization
    void Start()
    {
        Random.InitState(UniverseSettings.Seed ^ Hash128.Parse(transform.position.ToString()).GetHashCode());
        size = Random.Range(UniverseSettings.GalaxySize.x, UniverseSettings.GalaxySize.y);
        ss = (int)(UniverseSettings.SolarSystemPerSize * size);

        transform.localScale = new Vector3(size, size, size);

        ssObjects = new GameObject[ss];
        for (int i = 0; i < ss; ++i)
        {
            ssObjects[i] = Instantiate(UniverseSettings.SolarSystem, transform.position + Random.insideUnitSphere * size * 100, Quaternion.identity, transform);
        }
    }
}
