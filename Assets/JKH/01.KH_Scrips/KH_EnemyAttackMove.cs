using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KH_EnemyAttackMove : MonoBehaviour
{
    //----
    Vector3 startChocoPos;
    Vector3 startEnemyPos;

    public float IdleDelayTime = 2.0f;

    enum EnemyState
    {
        Idle, //평시 -> Move
        Move, //평소이동, 범위안에들어올떄 움직이며 감시모드
        Detect, //감시모드 
        Attack, //공격모드
        Damage, //피격 당했을때
        Die //죽는다
    }

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        cc = GetComponent<CharacterController>();
        startEnemyPos = transform.position;
    }

    EnemyState m_state = EnemyState.Idle;
    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Detect:
                Detect();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }


    float currTime;
    private void Idle()
    {
        currTime += Time.deltaTime;
        if (currTime > IdleDelayTime)
        {
            m_state = EnemyState.Move;
            currTime = 0;
        }
    }

    public GameObject target; //타겟= 플레이어
    public float attackRange = 15f; //사거리
    //-------attack enemy 거 걍 들고감-------
    public float yVelocity; //위로 나아가는 힘
    public Transform[] pos; // 좌표
    Vector3 dir;
    public float speed = 5;
    int i;    
    public float gravity = 1;
    bool isJump = false;
    CharacterController cc;
    public float jumpForwardSpeed = 10;
    float localSpeed;
    bool isJumpZone;
    private void Move()
    {
        //attack enemy 스크립트 가져와서 넣는다
        if (cc.isGrounded)
        {
            //i++;
            isJump = false;
            localSpeed = speed;
        }
        else
        {
        }
        dir = pos[i + 1].position - transform.position;
        dir.Normalize();
        dir.y = 0;
        float y = 0;
        //print("local speed 111111111111111 " + localSpeed);
        Jump(out y);
        //print("local speed --------------> " + localSpeed);

        dir *= localSpeed * Time.deltaTime;
        //Debug.DrawLine(transform.position, transform.position + dir * 100, Color.red);
        dir.y = y;
        cc.Move(dir);  //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@이거수정한다@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

        Choco();




        //만약 player가 범위안으로 들어온다면?
        Vector3 Pdir = target.transform.position - transform.position; //Pdir 로 수정
        float distance = Pdir.magnitude;
        if (distance < attackRange)
        {
            //Detect로 넘어간다
            m_state = EnemyState.Detect;
            
        }
    }
    public Transform chocoPos;
    GameObject isChoco;
    public GameObject[] chocoContainer;
    int chocoCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pos")
        {
            if (other.gameObject.tag == "Pos")
            {
                i++;
            }


            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                //다른 캐릭로 바꾼다..?
                i = 0;
            }
        }
        //===========

        if (isChoco == null)
        {
            if (other.gameObject.tag == "Choco")
            {
                print("초코먹음");
                isChoco = other.gameObject;
                startChocoPos = other.gameObject.transform.position;
                //startChocoPos = isChoco.transform.position;
            }
        }
        if (isChoco != null)
        {
            print("초코먹방중");
            //if (other.gameObject.tag =="ChocoContainer")
            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                print("초코야미야미야미야미");
                chocoContainer[chocoCount].SetActive(true);
                chocoCount++;
                Destroy(isChoco.gameObject);
                isChoco = null;

                //먹으면 해당 enemy getactive false 하고 그다음 enemy 실행한다 i++
                //int i = KH_GameManager.instance.i;
                //KH_GameManager.instance.enemyStart[i].SetActive(false);
                //KH_GameManager.instance.i++;
                //KH_GameManager.instance.enemyStart[i].SetActive(true);
                KH_GameManager.instance.isChoco = true;

                //그리고 만약에 다 채운다면 패배 Scene꺼낸다...
            }
        }

        
        if (other.gameObject.tag == "JumpZone")
        {
            //print("JJJ");
            isJumpZone = true;
            isJump = true;
        }

    }




    void Choco()
    {
        if (isChoco != null)
            isChoco.transform.position = chocoPos.position;

        if (chocoCount == 4)
        {
            print("chocoMax");
        }
    }

    // 필요속성 : 점프횟수, 최대 점프 가능 횟수
    public int jumpCount;
    public int MaxJumpCount = 1;    
    public float jumpZonePower = 15f;
    public void Jump(out float dirY)
    {
        if (cc.isGrounded)
        {
            //print("땅");
            yVelocity = 0;
            //jumpCount = 0;

        }

        if (isJumpZone)
        {
            //print("뛰어");
            yVelocity = jumpZonePower;
            //jumpCount++;
            isJumpZone = false;
            localSpeed = jumpForwardSpeed;
            //yVelocity -= gravity * Time.deltaTime;
        }

        yVelocity -= gravity * Time.deltaTime;
        dirY = yVelocity;


    }

    private void Detect()

    {
        //print("Detect");
        Vector3 dirE = target.transform.position - transform.position; //에너미가 바라보는방향으로
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //몸을돌린다
        Ray ray = new Ray();    //레이 생성
        ray.origin = aimingPoint.transform.position;    //레이 위치 
        ray.direction = aimingPoint.transform.forward;  //레이 방향
        RaycastHit hitInfo; //레이닿은변수 가져오기
        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            if (hitInfo.transform.gameObject.tag == "Player")
            {
                m_state = EnemyState.Attack;
            }
        }

        Vector3 dir = target.transform.position - transform.position; //나와 Target(Player) 간의 방향 계산
        float distance = dir.magnitude; //거리 계산
        if (distance > attackRange) //만약 거리가 에너미의 공격 범위보다 길다?
        {

            m_state = EnemyState.Move; //이러면 Move로 넘어간다
        }

        

    }


    public float rotSpeed = 2;
    public Transform aimingPoint; //발사포인트
    public GameObject LineRay; //총알발사라인(임시)
    public float fireTime = 0.1f; //연사속도

    private void Attack()
    {
        Vector3 dirE = target.transform.position - transform.position; //에너미가 바라보는방향으로
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //몸을돌린다

        Ray ray = new Ray();
        ray.origin = aimingPoint.position;
        ray.direction = aimingPoint.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            LineRenderer lr = null;


            if (hitInfo.transform.gameObject.tag == "Player")
            {

                currTime += Time.deltaTime;
                if (currTime > fireTime)
                {
                    GameObject line = Instantiate(LineRay);
                    lr = line.GetComponent<LineRenderer>();
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, hitInfo.point);
                    Destroy(line, 0.1f);
                    hitInfo.transform.gameObject.GetComponent<KH_Player_hp>().Damaged(.5f);
                    currTime = 0;
                }

            }

            else
            {
                currTime = fireTime;
            }

            if (lr != null)
                lr.SetPosition(1, hitInfo.point);
        }

        Vector3 dir = target.transform.position - transform.position; //나와 Target(Player) 간의 방향 계산
        float distance = dir.magnitude; //거리 계산
        if (distance > attackRange) //만약 거리가 에너미의 공격 범위보다 길다?
        {

            m_state = EnemyState.Move; //이러면 Move로 넘어간다
        }

    }

    private void Damage()
    {
        throw new NotImplementedException();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
