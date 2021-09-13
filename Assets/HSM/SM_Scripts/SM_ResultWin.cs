using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_ResultWin : MonoBehaviour
{
    Animator anim;

    CharacterController cc;
    BoxCollider bc;

    float speed = 1;

    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider>();
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();



    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1)
        {
            anim.SetTrigger("isHello");

        }
        else
        {
            dir = transform.up;
            transform.position -= dir * speed * Time.deltaTime;
        }
        //cc.Move(transform.up * speed * Time.deltaTime * (-1));


        //if (cc.isGrounded)
        //{
        //    anim.SetTrigger("ishello");

        //}

    }
}
