using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Na_StartBtn : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene("Characters");
    }
}
