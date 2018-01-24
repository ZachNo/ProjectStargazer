using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {

    int planets;
    float size;

    GameObject[] planetObjects;

    // Use this for initialization
    void Start()
    {
        Random.InitState(UniverseSettings.Seed ^ Hash128.Parse(transform.position.ToString()).GetHashCode());
        size = Random.Range(UniverseSettings.StarSize.x, UniverseSettings.StarSize.y);
        planets = (int)Random.Range(UniverseSettings.PlanetNumber.x, UniverseSettings.PlanetNumber.y);

        transform.localScale = new Vector3(size, size, size);

        planetObjects = new GameObject[planets];
        for (int i = 0; i < planets; ++i)
        {
            planetObjects[i] = Instantiate(UniverseSettings.Planet, transform.position + new Vector3(i, i, i), Quaternion.identity, transform);
        }
    }
}
