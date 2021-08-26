using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_EnemyFire : MonoBehaviour
{
    float currTime;
    float gunDuration = 0.2f; //총 공속
    float EnemySpeed = 4.0f;
    float attackRange = 15.0f;
    public GameObject bulletFactory;
    public GameObject firePos;
    public GameObject target;
    public GameObject pos1;
    public GameObject pos2;
    public float rotSpeed = 2;
    public Transform aimingPoint; //발사포인트
    public float fireTime = 0.1f; //연사속도
    public GameObject LineF; //총알발사라인(임시)
    
    //CharacterController cc; //이동하는거 안씀 아직.ㅎ
    // Start is called before the first frame update

    //열거형
    enum EnemyState
    {
        Idle, //평시 -> Move
        Move, //평소이동, 범위안에들어올떄 움직이며 감시모드
        Detect, //감시모드 
        Attack, //공격모드
        Damage, //피격 당했을때
        Die //죽는다
    }
    EnemyState m_state= EnemyState.Idle;
    void Start()
    {
        //cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //enemyShot();
        print("현재 상태: " + m_state);

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

    public float IdleDelayTime = 2.0f;    

    private void Idle()
    {
        currTime += Time.deltaTime;
        if (currTime > IdleDelayTime)
        {
            m_state = EnemyState.Move;
            currTime = 0;
        }
    }

    private void Move()
    {
        //일정 좌표 두개를 왔다리 갔다리 한다

        //Pos1,2로가는 방향 및 정규화
        Vector3 EnemyPos = transform.position; //내위치
        Vector3 pos1Pos = pos1.transform.position;
        
        Vector3 pos2Pos = pos2.transform.position;
        Vector3 dirToPos1 = pos1.transform.position - transform.position; //pos1을 바라보는 방향
        Vector3 dirToPos2 = pos2.transform.position - transform.position;
        dirToPos1.Normalize();
        dirToPos2.Normalize();
        //enemy가 pos1로 향한다

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos1),
                rotSpeed * Time.deltaTime); //몸을돌린다
        transform.position += dirToPos1 * EnemySpeed * Time.deltaTime; //pos1방향으로이동한다
        //몸돌리는데 걸리는 시간??????/

        //만약 enemy의 위치가 pos1이라면

        if (transform.position== pos1.transform.position)
        {
            //캐릭터의 몸을 pos2방향으로 몸을 돌린다 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos2),
                rotSpeed * Time.deltaTime);
            //캐릭터가 pos2로 향한다
            transform.position += dirToPos2 * EnemySpeed * Time.deltaTime;
            if (transform.position == pos2.transform.position)
            {
                
            }
        }

        //만약 player가 범위안으로 들어온다면?        
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;
        if (distance < attackRange)
        {
            //Detect로 넘어간다
            m_state = EnemyState.Detect;
        }
    }
    //임시
    //int layer = 1 << LayerMask.NameToLayer("");
    private void Detect()
    {
        //print("Detect");
        //Ray ray = new Ray();    //레이 생성
        //ray.origin = aimingPoint.transform.position;    //레이 위치 
        //ray.direction = aimingPoint.transform.forward;  //레이 방향
        //RaycastHit hitInfo; //레이닿은변수 가져오기
        //                    //Ray에 충돌 하고 싶은 Layer 나중에 적기

        ////Ray를 발사시켜서 어딘가에 부딪혔다면
        //if (Physics.Raycast(ray, out hitInfo, 100, layer))
        //{

        //}

    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Damage()
    {
        throw new NotImplementedException();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    void enemyShot()
    {
        Vector3 dir = target.transform.position - transform.position; //에너미가 바라보는방향
        dir.Normalize(); //정규화
        //transform.LookAt(target.transform); //에너미가 타겟을 바라본다
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime); //몸을돌린다        
        currTime += Time.deltaTime; //시간이흐르는데        
        if (gunDuration <= currTime) //총알발사속도
        {
            GameObject bullet = Instantiate(bulletFactory); //총알을 생성한다
            //transform.forward = dir;
            //dir = firePos.transform.forward;
            bullet.transform.position = firePos.transform.position; //firePos에 무기 장착한다
            bullet.transform.forward = firePos.transform.forward; //firePos 로테이션도 바꿔준다


            Destroy(bullet, 3); //일정시간 지나면 파괴한다
            currTime = 0; //현재시간 초기화
        }
    }
    public float AttackRange = 15f;
    void findPlayer()
    {
        Vector3 dir = target.transform.position - transform.position; //P-E
        float distance = dir.magnitude; //P-E거리
        if (distance<AttackRange) //만약 player가 사정거리 안에 들어왔다면?
        {
            //몸을 그방향으로 꺾는다 
            Vector3 dirE = target.transform.position - transform.position; //에너미가 바라보는방향
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
                rotSpeed * Time.deltaTime); //몸을돌린다
            //만약 ray를 발사해서 player가 잡힌다면?
            //(무빙치며) 할까말까
            //레이저를쏜다 (이건 딜 안들어감, 희망사항)
            //총알을 발사한다.
        }

        else
        {
            //원상태로 돌아간다
        }


        //만약 일정범위안에a 적군을 인식한다면(거리로? ray로? 벽어떻게하뮤ㅠㅠ) 
        //정찰을 멈춘다(공격모드)
        //몸과 총구를 상대방쪽으로 꺾는다
        //레이가 상대방에게 닿은다면? (벽 안뚫리게 해야함)
        //총을 발사한다 (위에코드)
        //만약 일정범위 밖에 있다면 
        //원상태(정찰모드)로 바꾼다
    }









    void justMemo()
    {
        //Enemy가 정찰하다가 player위치에 반응하고 총구를 플레이어를 향한다음 쏜다 

        //에너미가 일정구간을 정찰한다 (정찰모드) //navi
        //- 방향 두개찍고 와리가리 치기
        //만약 일정범위안에a 적군을 인식한다면(거리로? ray로? 벽어떻게하뮤ㅠㅠ) 
        //정찰을 멈춘다(공격모드)
        //몸과 총구를 상대방쪽으로 꺾는다
        //레이가 상대방에게 닿은다면? (벽 안뚫리게 해야함)
        //총을 발사한다 (위에코드)
        //만약 일정범위 밖에 있다면 
        //원상태(정찰모드)로 바꾼다

        //-------------------------------------------
        //그냥 레이로 인식하게 할 수는 없을까? ㅠ

    }
}
