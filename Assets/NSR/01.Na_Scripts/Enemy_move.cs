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
        print(i);

        if (isJump == false)
        {
            dir = pos[i + 1].position - pos[i].position;
            dir.Normalize();
            
        }
        else
        {
            dir = Vector3.forward;
            isJump = false;
        }

       
        transform.position += dir * speed * Time.deltaTime;
    
        cc.Move(Vector3.down * gravity * Time.deltaTime);       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pos")
        {
            i++;                                 
        }
        else if(other.gameObject.tag == "Jump")
        {
            i++;
            isJump = true;
        }
    }
}
