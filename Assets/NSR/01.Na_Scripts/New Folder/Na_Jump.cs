using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Jump : MonoBehaviour
{
    public float jumpForce = 60;

    bool isJump;

    //List<GameObject> objs;
    List<CharacterController> ccs;

    //CharacterController cc;

    //float currTime;
    //float jumpTime = 1.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isJump)
        {
            //cc = objs[i].GetComponent<CharacterController>();
            ccs[0].Move(Vector3.up * jumpForce * Time.deltaTime);

            //if (ccs[0].isGrounded)
            //{
            //    isJump = false;
            //    ccs.RemoveAt(0);
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            isJump = true;
            //objs.Add(other.gameObject);
            ccs.Add(other.gameObject.GetComponent<CharacterController>());
        }
    }
}
