using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeMovement : MonoBehaviour {

    public float speed = 2.0f;

	public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -90.0f;
    public float maxY = 90.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    int modifier = 1;

    DemoMode d;

    void Start()
    {
        d = GameObject.Find("Main Camera").GetComponent<DemoMode>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * speed * modifier;
            d.userCon();
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * Time.deltaTime * speed * modifier;
            d.userCon();
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * speed * modifier;
            d.userCon();
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * Time.deltaTime * speed * modifier;
            d.userCon();
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            modifier = 2;
            d.userCon();
        }
        else if(modifier == 2)
        {
            modifier = 1;
            d.userCon();
        }
        if (Input.GetMouseButton(0))
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            d.userCon();
        }
    }
}
