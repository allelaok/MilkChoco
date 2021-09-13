using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_ResultChocowin : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetTrigger("isTalk");
    }
}
