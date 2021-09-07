using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Center : MonoBehaviour
{
    public static Na_Center instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            DontDestroyOnLoad(instance);
        }
    }

    public float rotSpeed = 200f;
    float y;

    public int chNum;

    // Start is called before the first frame update
    void Start()
    {
        y = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");


        if (Input.GetMouseButton(0))
            y -= h * rotSpeed * Time.deltaTime;

        y = Mathf.Clamp(y, 0, 160);

        if (Input.GetMouseButtonUp(0))
        {

            float tmp = y;
            if(tmp % 20 < 10)
            {
                chNum = (int)tmp / 20;
                y = tmp - (tmp % 20);
            }
            else
            {
                chNum = (int)tmp / 20 + 1;
                y = tmp + 20 - (tmp % 20);
            }           
            
        }
        transform.localEulerAngles = new Vector3(0, y, 0);
    }
}
