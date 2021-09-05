using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Rotate : MonoBehaviour
{
    public float rotSpeed = 200f;

    float x;
    float y;

    public bool onV;
    public bool onH;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.localEulerAngles.x;
        y = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        

        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        if(onV)
            x += v * rotSpeed * Time.deltaTime;
        if (onH)
            y -= h * rotSpeed * Time.deltaTime;      


        x = Mathf.Clamp(x, -50, 30);

        transform.localEulerAngles = new Vector3(-x, y, 0);
    }
}
