using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour {

    int ss;
    public float size;
    public int galaxyType;
    float distToShow;

    public float spiralSpread = 0.1f;

    GameObject[] ssObjects;
    GameObject[] gasObjects;
    Random.State regenStars;

    // Use this for initialization
    void Start()
    {
        Random.InitState((UniverseSettings.Seed * int.MaxValue) ^ Hash128.Parse(transform.position.ToString()).GetHashCode());
        size = Random.Range(UniverseSettings.GalaxySize.x, UniverseSettings.GalaxySize.y);
        ss = (int)(UniverseSettings.SolarSystemPerSize * size);
        galaxyType = Random.Range(0, 2);

        transform.localScale = new Vector3(size, size, size);
        transform.rotation = Random.rotation;

        distToShow = UniverseSettings.UniverseScale * size;

        gasObjects = new GameObject[2];

        gasObjects[0] = Instantiate(UniverseSettings.Gas, transform.position, transform.rotation, transform);
        gasObjects[1] = Instantiate(UniverseSettings.GasDark, transform.position, transform.rotation, transform);

        Color randomColor = Random.ColorHSV(0, 1, 0.5f, 0.5f, 1, 1);

        var particleSystem = gasObjects[0].GetComponent<ParticleSystem>().main;
        particleSystem.startColor = randomColor;
        particleSystem.maxParticles = (int)(size * 1000);

        randomColor = Random.ColorHSV(0, 1, 0.5f, 0.5f, 1, 1, 0.1f, 0.1f);

        var particleSystem2 = gasObjects[1].GetComponent<ParticleSystem>().main;
        particleSystem2.startColor = randomColor;

        particleSystem2.maxParticles = (int)(size * 1000);

        StartCoroutine(updateParticles());

        regenStars = Random.state;
    }

    void Update()
    {
        GameObject cam = GameObject.Find("Main Camera");
        if (cam == null)
            return;

        if (ssObjects == null && Vector3.Distance(cam.transform.position, transform.position) < distToShow)
        {
            StartCoroutine(createStars());
        }
        else if (ssObjects != null && Vector3.Distance(cam.transform.position, transform.position) > distToShow)
        {
            for (int i = 0; i < ss; ++i)
            {
                Destroy(ssObjects[i]);
            }
            ssObjects = null;
        }
    }

    IEnumerator updateParticles()
    {
        if (gasObjects != null)
        {
            for (int i = 0; i < 2; ++i)
            {
                gasObjects[i].GetComponent<ParticleSystem>().Stop();
            }
        }

        yield return new WaitForEndOfFrame();

        if (gasObjects != null)
        {
            for (int i = 0; i < 2; ++i)
            {
                gasObjects[i].GetComponent<ParticleSystem>().Play();
            }
        }
    }

    IEnumerator createStars()
    {
        Random.state = regenStars;
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
                        transform.position + transform.rotation * new Vector3(x, Random.Range(-spiralSpread, spiralSpread), y) * size * 100,
                        Quaternion.identity, transform);
                    break;
            }
        }
        yield return null;
    }
}
