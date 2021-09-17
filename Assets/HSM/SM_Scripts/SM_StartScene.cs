using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Cinemachine;


public class SM_StartScene : MonoBehaviour
{
    PlayableDirector pd;

    public GameObject sceneSkip;

    

    // Start is called before the first frame update
    void Start()
    {
        pd = GetComponent<PlayableDirector>();
    }
    public void OnClickButton()
    {
        pd.time = 20;
    }
    // Update is called once per frame
    void Update()
    {
       


        

        //void OnClickButton()
        //{

        //}
    }
}
