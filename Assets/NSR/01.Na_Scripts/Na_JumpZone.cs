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

    // �ʿ�Ӽ� : �ӵ�, CharacterController
    public float speed = 7f;
    CharacterController cc;

    // �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����
    public float jumpPower = 3f;
    float yVelocity;
    public float gravity = 7f;
    Vector3 dir;

    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
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

    // �÷��̾� W, S, A, D �� �̵��ϰ� �ʹ�.
    // �ʿ�Ӽ� : �ӵ�, CharacterController
    void Move()
    {

        

        Jump(out dir.y);

        cc.Move(dir * speed * Time.deltaTime);
    }

    // �÷��̾� �����̽��ٷ� �����ϰ� �ʹ�.
    // �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����

    // 1�� ������ �ϰ�ʹ�.
    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
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
