using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodToFlare : MonoBehaviour {

    MeshRenderer mesh;
    LensFlare flare;

	// Use this for initialization
	void Start () {
        mesh = GetComponent<MeshRenderer>();
        flare = GetComponent<LensFlare>();
        flare.color = transform.parent.gameObject.GetComponent<MeshRenderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
        if (mesh.isVisible && !flare.enabled)
            flare.enabled = true;
        else if (!mesh.isVisible && flare.enabled)
            flare.enabled = false;

	}
}
