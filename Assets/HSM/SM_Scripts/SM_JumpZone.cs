using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_JumpZone : MonoBehaviour
{
    public float jumpForce;
    public float speed;
    public float jumpZoneForce;

    bool isJumpZone = false;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();        
    }

    void Jump()
    {
        if (isJumpZone)
        {
            rb.AddForce(new Vector3(0, jumpZoneForce, 0) * jumpForce);
            isJumpZone = false;
        }

    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SM_JumpZone")
        {
            isJumpZone = true;
        }
    }
}
