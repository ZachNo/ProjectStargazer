using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseGen : MonoBehaviour {

    public bool GenerateUniverse = true;

    public int Seed = 0;

    [Header("Universe")]
    public float UniverseScale = 100f;
    public int GalaxyCells = 10;

    [Header("Galaxy")]
    //[MinMaxRange(10, 100)]
    public Vector2 GalaxySizeRange;
    public int SolarSystemPerSize = 100;

    [Header("Solar System")]
    //[MinMaxRange(10, 100)]
    public Vector2 PlanetNumberRange;
    //[MinMaxRange(10, 100)]
    public Vector2 StarSizeRange;

    [Header("Planet")]
    //[MinMaxRange(10, 100)]
    public Vector2 MoonNumberRange;
    //[MinMaxRange(10, 100)]
    public Vector2 PlanetSizeRange;
    //[MinMaxRange(10, 100)]
    public Vector2 PlanetOrbitDiameterRange;
    //[MinMaxRange(10, 100)]
    public Vector2 PlanetOrbitSpeedRange;

    [Header("Moon")]
    //[MinMaxRange(10, 100)]
    public Vector2 MoonSizeRange;
    //[MinMaxRange(10, 100)]
    public Vector2 MoonOrbitDiameterRange;
    //[MinMaxRange(10, 100)]
    public Vector2 MoonOrbitSpeedRange;

    [Header("Prefabs")]
    public GameObject Galaxy;
    public GameObject SolarSystem;
    public GameObject Planet;
    public GameObject Moon;
    public GameObject Gas;
    public GameObject GasDark;

    //Private vars
    GameObject[] galaxies;

    // Use this for initialization
    void Awake ()
    {
        //Overall Seed
        UniverseSettings.Seed = Seed;

        //Universe Settings
        UniverseSettings.UniverseScale = UniverseScale;

        //Galaxy Settings
        UniverseSettings.GalaxyCells = GalaxyCells;
        UniverseSettings.GalaxySize = GalaxySizeRange;

        //Solar System Settings
        UniverseSettings.SolarSystemPerSize = SolarSystemPerSize;
        UniverseSettings.PlanetNumber = PlanetNumberRange;
        UniverseSettings.StarSize = StarSizeRange;

        //Planet Settings
        UniverseSettings.MoonNumber = MoonNumberRange;
        UniverseSettings.PlanetSize = PlanetSizeRange;
        UniverseSettings.PlanetOrbitDiameter = PlanetOrbitDiameterRange;
        UniverseSettings.PlanetOrbitSpeed = PlanetOrbitSpeedRange;

        //Moon Settings
        UniverseSettings.MoonSize = MoonSizeRange;
        UniverseSettings.MoonOrbitDiameter = MoonOrbitDiameterRange;
        UniverseSettings.MoonOrbitSpeed = MoonOrbitSpeedRange;

        //Prefabs
        UniverseSettings.Galaxy = Galaxy;
        UniverseSettings.SolarSystem = SolarSystem;
        UniverseSettings.Planet = Planet;
        UniverseSettings.Moon = Moon;
        UniverseSettings.Gas = Gas;
        UniverseSettings.GasDark = GasDark;

        //Generate Universe
        //transform.localScale = new Vector3(UniverseScale, UniverseScale, UniverseScale);

        if (GenerateUniverse)
        {
            galaxies = new GameObject[GalaxyCells * GalaxyCells * GalaxyCells];

            for (int i = 0; i < GalaxyCells; ++i)
            {
                for (int j = 0; j < GalaxyCells; ++j)
                {
                    for (int k = 0; k < GalaxyCells; ++k)
                    {
                        galaxies[i] = Instantiate(Galaxy, transform.position + new Vector3(i, j, k) * UniverseScale, Quaternion.identity, transform);
                    }
                }
            }
        }
    }


    public void Regenerate()
    {
        Seed = (int)(System.DateTime.Now.ToBinary());
        UniverseSettings.Seed = Seed;

        for (int i = 0; i < GalaxyCells * GalaxyCells * GalaxyCells; ++i)
        {
            Destroy(galaxies[i]);
        }

        galaxies = new GameObject[GalaxyCells * GalaxyCells * GalaxyCells];

        if (GenerateUniverse)
        {
            galaxies = new GameObject[GalaxyCells * GalaxyCells * GalaxyCells];

            for (int i = 0; i < GalaxyCells; ++i)
            {
                for (int j = 0; j < GalaxyCells; ++j)
                {
                    for (int k = 0; k < GalaxyCells; ++k)
                    {
                        galaxies[i] = Instantiate(Galaxy, transform.position + new Vector3(i, j, k) * UniverseScale, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}

public static class UniverseSettings {

    //Overall Seed
    public static int Seed;

    //Universe Settings
    public static float UniverseScale;

    //Galaxy Settings
    public static int GalaxyCells;
    public static Vector2 GalaxySize;

    //Solar System Settings
    public static int SolarSystemPerSize;
    public static Vector2 PlanetNumber;
    public static Vector2 StarSize;

    //Planet Settings
    public static Vector2 MoonNumber;
    public static Vector2 PlanetSize;
    public static Vector2 PlanetOrbitDiameter;
    public static Vector2 PlanetOrbitSpeed;

    //Moon Settings
    public static Vector2 MoonSize;
    public static Vector2 MoonOrbitDiameter;
    public static Vector2 MoonOrbitSpeed;

    //Prefabs
    public static GameObject Galaxy;
    public static GameObject SolarSystem;
    public static GameObject Planet;
    public static GameObject Moon;
    public static GameObject Gas;
    public static GameObject GasDark;
}