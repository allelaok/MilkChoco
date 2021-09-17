using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KH_StartSceneM : MonoBehaviour
{
    public GameObject Manual;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //OnclickStart();
    }
    public void OnClickStart()
    {
        Manual.SetActive(true);
    }

    public void OnClickStartOff()
    {
        Manual.SetActive(false);
    }

    public void OnClickGameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickGameExit()
    {
        Application.Quit();
    }
}
