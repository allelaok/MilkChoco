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

    public int[] chStat = new int[4];

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
            if (tmp % 20 < 10)
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



        switch (chNum)
        {
            case 0:
                chStat = new int[] { 100, 100, 100, 100 };
                break;
            case 1:
                chStat = new int[] { 200, 50, 200, 50 };
                break;
            case 2:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
            case 3:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
            case 4:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
            case 5:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
            case 6:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
            case 7:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
            case 8:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
            case 9:
                chStat = new int[] { 200, 90, 90, 30 };
                break;
        }
    }
}

   

