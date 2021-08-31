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
    }

    // Update is called once per frame
    void Update()
    {
        print("ว๖ป๓ลย: " + i);
        dir = pos[i + 1].position - pos[i].position;
        dir.Normalize();
        

        //Vector3 distance = transform.position - pos[i + 1].position;
        //if (distance.magnitude < 0.1f)
        //{
        //    i++;
        //}

        if (i >= pos.Length - 1)
        {
            dir = Vector3.forward;
        }


        cc.Move(Vector3.down * gravity * Time.deltaTime);
        transform.position += dir * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pos")
        {
            //Debug.Log("!!");
            i++;
        }
    }
}
