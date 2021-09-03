using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KH_StartSceneM : MonoBehaviour
{
    public GameObject StartButton;
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
        SceneManager.LoadScene("KH_Scene");
    }
}
