using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour {

    int ss;
    float size;
    int galaxyType;

    float spiralSpread = 0.1f;

    GameObject[] ssObjects;

    // Use this for initialization
    void Start()
    {
        Random.InitState(UniverseSettings.Seed ^ Hash128.Parse(transform.position.ToString()).GetHashCode());
        size = Random.Range(UniverseSettings.GalaxySize.x, UniverseSettings.GalaxySize.y);
        ss = (int)(UniverseSettings.SolarSystemPerSize * size);
        galaxyType = Random.Range(0, 2);

        transform.localScale = new Vector3(size, size, size);

        ssObjects = new GameObject[ss];
        for (int i = 0; i < ss; ++i)
        {
            switch (galaxyType)
            {
                case 0:
                    ssObjects[i] = Instantiate(UniverseSettings.SolarSystem, transform.position + Random.insideUnitSphere * size * 100, Quaternion.identity, transform);
                    break;
                case 1:
                    float negative = Random.Range(-1f, 1f) > 0 ? 1 : -1;
                    float t = Mathf.Pow(Random.value, 3) * 5f;
                    float x = negative * Mathf.Sqrt(t) * Mathf.Cos(t) + Random.Range(-spiralSpread, spiralSpread) * (5.5f - t);
                    float y = negative * Mathf.Sqrt(t) * Mathf.Sin(t) + Random.Range(-spiralSpread, spiralSpread) * (5.5f - t);
                    ssObjects[i] = Instantiate(UniverseSettings.SolarSystem,
                        transform.position + new Vector3(x, Random.Range(-spiralSpread, spiralSpread), y) * size * 100,
                        Quaternion.identity, transform);
                    break;
            }
        }
    }

    /*void Update()
    {
        GameObject cam = GameObject.Find("Main Camera");
        if (cam == null)
            return;

        if (ssObjects == null && Vector3.Distance(cam.transform.position, transform.position) < UniverseSettings.PlanetOrbitDiameter.y)
        {
            ssObjects = new GameObject[ss];
            for (int i = 0; i < ss; ++i)
            {
                switch (galaxyType)
                {
                    case 0:
                        ssObjects[i] = Instantiate(UniverseSettings.SolarSystem, transform.position + Random.insideUnitSphere * size * 100, Quaternion.identity, transform);
                        break;
                    case 1:
                        float negative = Random.Range(-1f, 1f) > 0 ? 1 : -1;
                        float t = Random.Range(0f, 5f);
                        float x = negative * Mathf.Sqrt(t) * Mathf.Cos(t);
                        float y = negative * Mathf.Sqrt(t) * Mathf.Sin(t);
                        ssObjects[i] = Instantiate(UniverseSettings.SolarSystem,
                            transform.position + new Vector3(x + Random.Range(-spiralSpread, spiralSpread), Random.Range(-spiralSpread, spiralSpread), y + Random.Range(-spiralSpread, spiralSpread)) * size * 100,
                            Quaternion.identity, transform);
                        break;
                }
            }
        }
        else if (ssObjects != null && Vector3.Distance(cam.transform.position, transform.position) > UniverseSettings.PlanetOrbitDiameter.y)
        {
            for (int i = 0; i < ss; ++i)
            {
                Destroy(ssObjects[i]);
            }
            ssObjects = null;
        }
    }*/
}
