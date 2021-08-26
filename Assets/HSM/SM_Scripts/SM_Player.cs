using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Player : MonoBehaviour
{

    public float speed = 5;

    CharacterController cc;

    //점프파워
    public float jumpPower = 5;

    //y속도 
    float yVelocity;

    //중력
    float gravity = -20;

    

    
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //A,D 좌우
        float h = Input.GetAxis("Horizontal");

        //W,S 앞뒤
        float v = Input.GetAxis("Vertical");

        //방향을 정하고
        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;
        dir.Normalize();

        Jump(out dir.y); // dir.y 앞에 out 혹은 ref을 넣어줘야 밑에 있는 Jump가 불러와짐
        {

            cc.Move(dir * speed * Time.deltaTime);

        }

        void Jump(out float dirY) // float dirY 앞에 out을 넣어줘야 점프가 실행됨
        {
            //만약에 Player가 바닥에 닿아있다면
            if (cc.isGrounded)
            {
                //y속도를 0으로 만든다.
                yVelocity = 0;
            }

            //만약에 스페이스바(jump)를 누르면
            if (Input.GetButtonDown("Jump"))
            {
                //y속도를 jumPower로 한다.
                yVelocity = jumpPower;

            }

            //dirY에 y속도를 넣는다.
            dirY = yVelocity;

            //y속도를 중력만큼 더해준다.
            yVelocity += gravity * Time.deltaTime;

            
        }
    }
}
