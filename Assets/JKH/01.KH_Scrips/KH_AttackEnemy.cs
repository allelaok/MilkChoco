using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_AttackEnemy : MonoBehaviour
{
    public Transform[] pos;
    Vector3 dir;
    public float speed = 5;
    int i;
    public float gravity = 1;
    bool isJump = false;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        cc = GetComponent<CharacterController>();
        startEnemyPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Choco();

        //-------------------------------------------------------------------------------------------------------------
        //print("현상태: " + i);
        //dir = pos[i + 1].position - transform.position;
        //dir.Normalize();


        ////Vector3 distance = transform.position - pos[i + 1].position;
        ////if (distance.magnitude < 0.1f)
        ////{
        ////    i++;
        ////}

        //if (i >= pos.Length - 1)
        //{
        //    dir = Vector3.forward;
        //}


        //cc.Move(Vector3.down * gravity * Time.deltaTime);
        //transform.position += dir * speed * Time.deltaTime;

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



        if (i < pos.Length - 1)
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
    Vector3 startChocoPos;
    Vector3 startEnemyPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pos")
        {
            if (other.gameObject.tag == "Pos")
            {
                i++;
            }
            else if (other.gameObject.tag == "Jump")
            {

                isJump = true;
            }

            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                i = 0;
            }
        }
        //===========

        if(isChoco == null)
        {
            if (other.gameObject.tag == "Choco")
            {
                isChoco = other.gameObject;
                startChocoPos = isChoco.transform.position;
            }
        }
        else
        {
            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                chocoContainer[chocoCount].SetActive(true);
                chocoCount++;
                Destroy(isChoco.gameObject);
                isChoco = null;
            }
        }

    }

    // 필요속성 : 우유위치, 우유, milkContainer, 우유개수, 우유개수UI
    public Transform chocoPos;
    GameObject isChoco;
    public GameObject[] chocoContainer;
    int chocoCount;
   
    

    void Choco()
    {
        if (isChoco != null)
            isChoco.transform.position = chocoPos.position;

        if (chocoCount == 4)
        {
            print("chocoMax");
        }
    }
}
