using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Text playTimeSUI;
    Text playTimeMUI;

    float currTime;
    int playTimeS = 59;
    int playTimeM = 04;
    
    // Start is called before the first frame update
    void Start()
    {
        playTimeSUI = GameObject.Find("PlayTimeS").GetComponent<Text>();
        playTimeMUI = GameObject.Find("PlayTimeM").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        


        playTimeS = 59 - (int)currTime;
        currTime += Time.deltaTime;
        if(currTime > 60)
        {
            playTimeS = 59;
            playTimeM --;
            currTime = 0;
        }

        playTimeSUI.text = "" + playTimeS;
        playTimeMUI.text = "" + playTimeM;

        if (playTimeS == 0 && playTimeM == 0)
        {
            SceneManager.LoadScene("Na_LoseScene");
        }
    }
}
