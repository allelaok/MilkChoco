using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_PlayerMove : MonoBehaviour
{
    public float speed = 4.0f;
    CharacterController cc;
    //�����Ŀ�
    public float jumpPower = 5;
    //y�ӵ�
    public float yVelocity;
    //�߷� rigidbody������
    float gravity = -20.0f;
    //����Ƚ��
    int jumpCount;
    //�ִ�����Ƚ��
    public int maxJumpCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //w,a,s,d�� ������
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dirh = transform.right * h;
        Vector3 dirv = transform.forward * v;
        Vector3 dir = dirh + dirv;
        dir.Normalize();

        Jump(out dir.y);


        //�׹������� �����δ�
        cc.Move(dir * speed * Time.deltaTime);
    }

    void Jump(out float dirY)
    {
        //���� �ٴڿ� ����ִٸ�      
        if (cc.isGrounded == true)
        {

            //y�ӵ��� 0���� �Ѵ�
            yVelocity = 0;
            //����Ƚ���� 0���� �ʱ�ȭ �����ش�
            jumpCount = 0;
        }

        //���࿡ �����̽���("Jump")�� �����ٸ�
        if (Input.GetButtonDown("Jump"))
        {
            //����Ƚ���� �ִ����� Ƚ�� ���� ������
            if (jumpCount < maxJumpCount)
            {
                //y�ӵ��� jumpPower���Ѵ�.
                yVelocity = jumpPower;
                jumpCount++;
            }


        }
        //dir.y �� y�ӵ��� �ִ´�
        dirY = yVelocity;
        //y�ӵ��� �߷¸�ŭ �����ش� 
        yVelocity += gravity * Time.deltaTime;
    }
}
