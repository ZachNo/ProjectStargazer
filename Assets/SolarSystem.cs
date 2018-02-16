using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {

    int planets;
    float size;
    int temperature;

    GameObject[] planetObjects;

    public Color tempColor;

    void Awake()
    {
        Random.InitState(UniverseSettings.Seed ^ Hash128.Parse(transform.position.ToString()).GetHashCode());
        size = Random.Range(UniverseSettings.StarSize.x, UniverseSettings.StarSize.y);
        planets = (int)Random.Range(UniverseSettings.PlanetNumber.x, UniverseSettings.PlanetNumber.y);
        temperature = (int)(4900 * Mathf.Tan(Random.value * 1.5f) + 1000);
        tempColor = Temp2Color(temperature);

        float intensity = Mathf.Clamp(Random.value * 100, 10, 100);
        tempColor.r *= intensity;
        tempColor.g *= intensity;
        tempColor.b *= intensity;
    }

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector3(size, size, size);

        /*GameObject cam = GameObject.Find("Main Camera");
        if (cam == null)
            return;

        if (planetObjects == null && Vector3.Distance(cam.transform.position, transform.position) < UniverseSettings.PlanetOrbitDiameter.y)
        {
            planetObjects = new GameObject[planets];
            for (int i = 0; i < planets; ++i)
            {
                planetObjects[i] = Instantiate(UniverseSettings.Planet, transform.position + new Vector3(i, i, i), Quaternion.identity, transform);
            }
        }*/
    }

    void Update()
    {
        GameObject cam = GameObject.Find("Main Camera");
        if (cam == null)
            return;

        if(planetObjects == null && Vector3.Distance(cam.transform.position, transform.position) < UniverseSettings.PlanetOrbitDiameter.y)
        {
            planetObjects = new GameObject[planets];
            for (int i = 0; i < planets; ++i)
            {
                planetObjects[i] = Instantiate(UniverseSettings.Planet, transform.position + new Vector3(i, i, i), Quaternion.identity, transform);
            }
        }
        else if(planetObjects != null && Vector3.Distance(cam.transform.position, transform.position) > UniverseSettings.PlanetOrbitDiameter.y)
        {
            for (int i = 0; i < planets; ++i)
            {
                Destroy(planetObjects[i]);
            }
            planetObjects = null;
        }
    }


    //Color temperature to an RGB color
    //Using the algorithmn from here:
    //http://www.tannerhelland.com/4435/convert-temperature-rgb-algorithm-code/
    Color Temp2Color(int temp)
    {
        Color ret = new Color();

        temp /= 100;

        ret.r = temp <= 66 ? 1f : Mathf.Clamp01(1.292936f * Mathf.Pow(temp - 60, -0.1332047f));
        ret.g = temp <= 66 ? Mathf.Clamp01(0.3900815f * Mathf.Log(temp) - 0.6318414f) : Mathf.Clamp01(1.129891f * Mathf.Pow(temp - 60, -0.07551484f));
        ret.b = temp >= 66 ? 1f : (temp <= 19 ? 0f : Mathf.Clamp01(0.5432067f * Mathf.Log(temp - 10) - 1.196254f));

        return ret;
    }
}
