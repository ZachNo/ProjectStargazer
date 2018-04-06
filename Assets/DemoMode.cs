using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoMode : MonoBehaviour {

    public float ControlTimeout = 5f;
    public float CycleTime = 15f;
    public float ChangeTime = 4f;
    public GameObject blackout;

    bool userControl = false;

    float timeSinceUser = 0f;

    float speed = 0.1f;
    float idleTime = 0f;
    float dist = 80f;

    Image blackoutImage;

	// Use this for initialization
	void Start ()
    {
        blackoutImage = blackout.GetComponent<Image>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ug = GameObject.Find("UniverseGen");
            if (ug == null)
            { return; }

            ug.GetComponent<UniverseGen>().Regenerate();
            userCon();
        }

        if(userControl && timeSinceUser > ControlTimeout)
        {
            userControl = false;
        }
        else
        {
            timeSinceUser += Time.deltaTime;
        }

        if(!userControl)
        {
            idleTime += Time.deltaTime;

            float alpha = Mathf.Max(0f, Mathf.Abs((idleTime - (CycleTime / 2f)) / (ChangeTime / 2f)) - (CycleTime / 4f) + 1);
            Color black = new Color(0, 0, 0, alpha);
            blackoutImage.color = black;

            if(idleTime > CycleTime)
            {
                idleTime = 0f;

                GameObject ug = GameObject.Find("UniverseGen");
                if (ug != null)
                {
                    ug.GetComponent<UniverseGen>().Regenerate();
                    dist = ug.GetComponentInChildren<Galaxy>().size * Random.Range(50f, 150f);
                    speed = Random.Range(0.01f, 0.1f);
                }
            }

            transform.position = new Vector3(Mathf.Sin(idleTime * speed) * dist, 0, Mathf.Cos(idleTime * speed) * dist);
            transform.rotation = Quaternion.LookRotation(-transform.position);
        }
    }

    public void userCon()
    {
        timeSinceUser = 0f;
        userControl = true;
    }
}
