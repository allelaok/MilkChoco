using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SM_ResultManager : MonoBehaviour
{

    public GameObject Result;

    public GameObject retryBt;

    public GameObject mainBt;

    // Start is called before the first frame update
    void Start()
    {
        iTween.ScaleTo(Result, iTween.Hash(
            "x", 1,
            "y", 1,
            "z", 1,
            "time", 0.5f,
            "easytype", iTween.EaseType.easeOutBounce
           
            ));

        //다시하기 버튼 크기 0 -> 1
        iTween.ScaleTo(retryBt, iTween.Hash(
            "x", 1,
            "y", 1,
            "z", 1,
            "time", 0.5f,
            "easytype", iTween.EaseType.easeOutBack,
            "delay", 2
            ));

        //메인 버튼 크기 0 -> 1
        iTween.ScaleTo(mainBt, iTween.Hash(
            "x", 1,
            "y", 1,
            "z", 1,
            "time", 0.5f,
            "easytype", iTween.EaseType.easeOutBack,
            "delay", 2
            ));

    }





    // Update is called once per frame
    void Update()
    {
       
    }

    public   void OnClickRetry()
    {
        //GameScene으로 전환
        SceneManager.LoadScene("Characters");
    }

    public void OnClickMain()
    {
        //GameScene으로 전환
        SceneManager.LoadScene("StartScene");
    }
}
