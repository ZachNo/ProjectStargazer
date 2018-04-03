﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMode : MonoBehaviour {

    public float ControlTimeout = 5f;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameObject ug = GameObject.Find("UniverseGen");
            if (ug == null)
                return;

            ug.GetComponent<UniverseGen>().Regenerate();
        }
    }
}
