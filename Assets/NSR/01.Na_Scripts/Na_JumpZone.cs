using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_JumpZone : MonoBehaviour
{
    public float jumpForce = 60;

    bool isJump;

    //List<GameObject> objs;
    //List<CharacterController> ccs;


    //float currTime;
    //float jumpTime = 1.5f;

    // 필요속성 : 속도, CharacterController
    public float speed = 7f;
    CharacterController cc;

    // 필요속성 : 점프파워, 중력, y속도, 방향
    public float jumpPower = 3f;
    float yVelocity;
    public float gravity = 7f;
    Vector3 dir;

    // 필요속성 : 점프횟수, 최대 점프 가능 횟수
    int jumpCount;
    public int MaxJumpCount = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isJump)
        {
            ////cc = objs[i].GetComponent<CharacterController>();
            //if (cc != null)
            //{
            //    cc.Move(Vector3.up * jumpForce * Time.deltaTime);

            //    if (cc.isGrounded)
            //    {
            //        isJump = false;
            //        cc = null;
            //    }

            //}
        }
    }

    // 플레이어 W, S, A, D 로 이동하고 싶다.
    // 필요속성 : 속도, CharacterController
    void Move()
    {

        

        Jump(out dir.y);

        cc.Move(dir * speed * Time.deltaTime);
    }

    // 플레이어 스페이스바로 점프하고 싶다.
    // 필요속성 : 점프파워, 중력, y속도, 방향

    // 1단 점프를 하고싶다.
    // 필요속성 : 점프횟수, 최대 점프 가능 횟수
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
        yVelocity -= gravity * Time.deltaTime;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            isJump = true;
            //objs.Add(other.gameObject);
            cc = other.gameObject.GetComponent<CharacterController>();
        }
    }
}
