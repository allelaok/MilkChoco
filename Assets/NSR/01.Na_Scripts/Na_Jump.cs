using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Jump : MonoBehaviour
{
    public float jumpForce = 500;

    bool isPlayer;

    GameObject player;
    CharacterController cc;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            cc = player.GetComponent<CharacterController>();
            cc.Move(Vector3.up * jumpForce * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayer = true;
            player = other.gameObject;
        }
    }
}
