﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    int moons;
    float size;
    float orbitTilt;
    float orbitDiameter;
    float orbitSpeed;
    float offset;

    GameObject[] moonObjects;

    // Use this for initialization
    void Awake () {
        Random.InitState(UniverseSettings.Seed ^ Hash128.Parse(transform.position.ToString()).GetHashCode());
        size = Random.Range(UniverseSettings.PlanetSize.x, UniverseSettings.PlanetSize.y);
        orbitTilt = Random.Range(0, 360);
        orbitDiameter = Random.Range(UniverseSettings.PlanetOrbitDiameter.x, UniverseSettings.PlanetOrbitDiameter.y);
        orbitSpeed = Random.Range(UniverseSettings.PlanetOrbitSpeed.x, UniverseSettings.PlanetOrbitSpeed.y);
        moons = (int)Random.Range(UniverseSettings.MoonNumber.x, UniverseSettings.MoonNumber.y);
        offset = Random.value * 100;

        transform.localScale = new Vector3(size, size, size);

        moonObjects = new GameObject[moons];
        for(int i = 0; i < moons; ++i)
        {
            moonObjects[i] = Instantiate(UniverseSettings.Moon, transform.position + new Vector3(i, i, i), Quaternion.identity, transform);
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(
            orbitDiameter * Mathf.Sin(Time.time * (orbitSpeed / 100) + offset),
            0f,
            orbitDiameter * Mathf.Cos(Time.time * (orbitSpeed / 100) + offset)
        );
    }
}
