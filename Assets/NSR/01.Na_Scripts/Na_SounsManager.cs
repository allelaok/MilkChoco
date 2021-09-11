using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_SounsManager : MonoBehaviour
{
    public static Na_SounsManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            DontDestroyOnLoad(instance);
        }
    }




}
