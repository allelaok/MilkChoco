using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Jump : MonoBehaviour
{
    public float jumpForce = 500;

    bool playerJump;

    GameObject player;
    CharacterController cc;

    float currTime;
    float jumpTime = 1.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerJump)
        {
            cc = player.GetComponent<CharacterController>();
            cc.Move(Vector3.up * jumpForce * Time.deltaTime);

            currTime += Time.deltaTime;
            if(currTime > jumpTime)
            {
                playerJump = false;
                currTime = 0;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerJump = true;
            player = other.gameObject;
        }
    }
}
