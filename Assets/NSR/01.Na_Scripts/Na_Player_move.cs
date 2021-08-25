using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Player_move : MonoBehaviour
{
    public float speed = 7f;
    CharacterController cc;

    public float jumpPower = 3f;
    float yVelocity;
    public float gravity = -7f;
    Vector3 dir;

    int jumpCount;
    public int MaxJumpCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;

        dir = dirH + dirV;
        dir.Normalize();


        Jump(out dir.y);

        cc.Move(dir * speed * Time.deltaTime);

    }


    void Jump(out float dirY)
    {
        if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < MaxJumpCount)
            {
                yVelocity = jumpPower;
                jumpCount++;
            }


        }
        dirY = yVelocity;
        yVelocity += gravity * Time.deltaTime;


    }
}
