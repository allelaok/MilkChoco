using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_HpBar : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main)
            transform.forward = Camera.main.transform.forward;
        
    }
}
