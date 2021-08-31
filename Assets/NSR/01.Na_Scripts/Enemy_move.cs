using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_move : MonoBehaviour
{
    public Transform[] pos;

    Vector3 dir;
    public float speed = 5;

    int i;

    public float gravity = 1;

    CharacterController cc;

    bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (isJump == false)
        {
            dir = pos[i + 1].position - transform.position;
            dir.Normalize();

        }
        else
        {
            dir = transform.forward;
            if (cc.isGrounded)
            {
                i++;
                isJump = false;

            }
        }

        

        if(i < pos.Length - 1)
        {
            cc.Move(Vector3.down * gravity * Time.deltaTime);
            transform.position += dir * speed * Time.deltaTime;
        }
        else
        {
            isJump = true;
        }

        transform.forward = dir;
         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pos")
        {
            i++;                                 
        }
        else if(other.gameObject.tag == "Jump")
        {
            
            isJump = true;
        }

        if (other.gameObject.name.Contains("ChocoContainer"))
        {
            i = 0;
        }
    }
}
