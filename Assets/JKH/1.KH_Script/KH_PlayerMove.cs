using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_PlayerMove : MonoBehaviour
{
    public float speed = 4.0f;
    CharacterController cc;
    //점프파워
    public float jumpPower = 5;
    //y속도
    public float yVelocity;
    //중력 rigidbody사용안함
    float gravity = -20.0f;
    //점프횟수
    int jumpCount;
    //최대점프횟수
    public int maxJumpCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //w,a,s,d로 움직임
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dirh = transform.right * h;
        Vector3 dirv = transform.forward * v;
        Vector3 dir = dirh + dirv;
        dir.Normalize();

        Jump(out dir.y);


        //그방향으로 움직인다
        cc.Move(dir * speed * Time.deltaTime);
    }

    void Jump(out float dirY)
    {
        //만약 바닥에 닿아있다면      
        if (cc.isGrounded == true)
        {

            //y속도를 0으로 한다
            yVelocity = 0;
            //점프횟수를 0으로 초기화 시켜준다
            jumpCount = 0;
        }

        //만약에 스페이스바("Jump")를 누른다면
        if (Input.GetButtonDown("Jump"))
        {
            //점프횟수가 최대점프 횟수 보다 작으면
            if (jumpCount < maxJumpCount)
            {
                //y속도를 jumpPower로한다.
                yVelocity = jumpPower;
                jumpCount++;
            }


        }
        //dir.y 에 y속도를 넣는다
        dirY = yVelocity;
        //y속도를 중력만큼 더해준다 
        yVelocity += gravity * Time.deltaTime;
    }
}
