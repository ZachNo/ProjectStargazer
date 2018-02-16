using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunColor : MonoBehaviour {

    Renderer render;
    MaterialPropertyBlock mpb;
    int emcolorID;
    int colorID;

    Color tempColor;

    // Use this for initialization
    void Start ()
    {
        if(GetComponent<SolarSystem>() != null)
        {
            tempColor = GetComponent<SolarSystem>().tempColor;
        }
        else
        {
            tempColor = transform.parent.GetComponent<SolarSystem>().tempColor;
        }

        render = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        colorID = Shader.PropertyToID("_Color");
        emcolorID = Shader.PropertyToID("_EmissionColor");
    }
	
	// Update is called once per frame
	void Update ()
    {
        mpb.SetColor(colorID, tempColor);
        mpb.SetColor(emcolorID, tempColor);
        render.SetPropertyBlock(mpb);
    }
}
