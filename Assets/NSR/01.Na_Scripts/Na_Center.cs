using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [HideInInspector]
    public int[] chStat = new int[4];

    public Text nameUI;
    public Text[] statUI;
    public Image[] statBarUI;

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
                chStat = new int[] { 150, 80, 80, 80 };
                nameUI.text = "Ludo";
                StatUI();
                break;
            case 1:
                chStat = new int[] { 200, 80, 75, 75 };
                nameUI.text = "Viking";
                StatUI();
                break;
            case 2:
                chStat = new int[] { 140, 70, 90, 100 };
                nameUI.text = "Sombrero";
                StatUI();
                break;
            case 3:
                chStat = new int[] { 150, 70, 90,90 };
                nameUI.text = "Shower";
                StatUI();
                break;
            case 4:
                chStat = new int[] { 180, 90,80, 90};
                nameUI.text = "Mastache";
                StatUI();
                break;
            case 5:
                chStat = new int[] { 190, 100, 80, 75 };
                nameUI.text = "Miner";
                StatUI();
                break;
            case 6:
                chStat = new int[] { 150, 90, 100, 80 };
                nameUI.text = "Magician";
                StatUI();
                break;
            case 7:
                chStat = new int[] { 120, 90, 80, 100 };
                nameUI.text = "Crown";
                StatUI();
                break;
            case 8:
                chStat = new int[] { 110, 100, 80, 100 };
                nameUI.text = "Cowboy";
                StatUI();
                break;
        }
    }

    void StatUI()
    {
        for (int i = 0; i < chStat.Length; i++)
        {
            statUI[i].text = "" + chStat[i];
            statBarUI[i].fillAmount = chStat[i] * 0.01f;
        }
        statBarUI[0].fillAmount = chStat[0] * 0.005f;
    }
}

   

