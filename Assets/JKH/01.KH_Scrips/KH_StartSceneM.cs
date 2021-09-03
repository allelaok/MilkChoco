using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
    }
    public void OnclickStart()
    {
        SceneManager.LoadScene("KH_Scene");
    }
}
