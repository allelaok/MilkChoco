using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_JumpZone : MonoBehaviour
{
    public float jumpForce;
    public float speed;
    public float jumpZoneForce;

    bool isJumpZone;
    Rigidbody rb;

    // Start is called before the first frame update
    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}

    //void FixedUpdate()
    //{
    //    //rb.AddForce(transform.up * jumpForce);        
    //}
    // Update is called once per frame
    void Update()
    { }

    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isJumpZone = true;
        }
    }
}
