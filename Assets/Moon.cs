using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour {

    float size;
    float orbitTilt;
    float orbitDiameter;
    float orbitSpeed;
    float offset;

	// Use this for initialization
	void Awake () {
        Random.InitState(UniverseSettings.Seed ^ Hash128.Parse(transform.position.ToString()).GetHashCode());
        size = Random.Range(UniverseSettings.MoonSize.x, UniverseSettings.MoonSize.y);
        orbitTilt = Random.Range(-90f, 90f);
        orbitDiameter = Random.Range(UniverseSettings.MoonOrbitDiameter.x, UniverseSettings.MoonOrbitDiameter.y);
        orbitSpeed = Random.Range(UniverseSettings.MoonOrbitSpeed.x, UniverseSettings.MoonOrbitSpeed.y);
        offset = Random.value * 100;

        transform.localScale = new Vector3(size, size, size);
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
