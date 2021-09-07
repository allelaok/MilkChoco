using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_KH_EnemyFire : MonoBehaviour
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
    public GameObject LineRay; //총알발사라인(임시)
    public Animator animator;
    BoxCollider bc;
    Rigidbody rb;

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
    EnemyState m_state = EnemyState.Idle;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnDamageProcess(transform.forward * -1);
        }

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
                //Attack();
                break;
            case EnemyState.Damage:
                //Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    public float IdleDelayTime = 2.0f;

    private void Idle()
    {
        //animator.SetTrigger("isIdle");
        
        currTime += Time.deltaTime;
        if (currTime > IdleDelayTime)
        {
            m_state = EnemyState.Move;
            currTime = 0;
        }
    }

    public Transform wayPoint1;
    public Transform wayPoint2;
    public Transform currentWayPoint;
    private void Move()
    {
        //일정 좌표 두개를 왔다리 갔다리 한다

        //Pos1,2로가는 방향 및 정규화
        //Vector3 EnemyPos = transform.position; //내위치        
        //Vector3 dirToPos1 = pos1.transform.position - transform.position; //pos1을 바라보는 방향
        //Vector3 dirToPos2 = pos2.transform.position - transform.position;
        //float EnemyPos1Distance = dirToPos1.magnitude;
        //float EnemyPos2Distance = dirToPos2.magnitude;
        //float Distance = EnemyPos1Distance - EnemyPos2Distance;
        //float DistanceAbs = Mathf.Abs(Distance);
        //dirToPos1.Normalize();
        //dirToPos2.Normalize();

        //1. 첫번째 웨이포인트와 오브젝트의 거리를 계산한다
        //2. 거리가 일정이하가 되면 웨이포인트를 전환한다.
        //3. 오브젝트의 방향을 돌리고 두번째 웨이포인트를 향해 이동한다.
        //4. 거리가 일정 이하가 되면 웨이포인트를 전환한다.
        animator.SetTrigger("isWalk");


        var distance2 = Vector3.Distance(gameObject.transform.position, currentWayPoint.position);
        //Debug.Log(distance2);
        if (Vector3.Distance(gameObject.transform.position, wayPoint1.position) <= 1f)
        {
            m_state = EnemyState.Idle;


            //Debug.Log("2");
            currentWayPoint = wayPoint2;
        }

        if (Vector3.Distance(gameObject.transform.position, wayPoint2.position) <= 1f)
        {
            m_state = EnemyState.Idle;
            //Debug.Log("1");
            currentWayPoint = wayPoint1;
        }

        var wayPointDir = currentWayPoint.transform.position - transform.position; //gameObject.transform.position
        wayPointDir.Normalize();

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); //pos1쪽으로 몸을돌린다
        transform.position += wayPointDir * EnemySpeed * Time.deltaTime; //pos1방향으로이동한다\

        //if (EnemyPos1Distance > EnemyPos2Distance)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos1),
        //        rotSpeed * Time.deltaTime); //pos1쪽으로 몸을돌린다
        //    transform.position += dirToPos1 * EnemySpeed * Time.deltaTime; //pos1방향으로이동한다
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos2),
        //        rotSpeed * Time.deltaTime);
        //    transform.position += dirToPos2 * EnemySpeed * Time.deltaTime;

        //}


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
        animator.SetTrigger("isIdle");
        //print("Detect");
        Vector3 dirE = target.transform.position - transform.position; //에너미가 바라보는방향으로
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //몸을돌린다
        Ray ray = new Ray();    //레이 생성
        ray.origin = firePos.transform.position;    //레이 위치 
        ray.direction = transform.forward;  //레이 방향
        RaycastHit hitInfo; //레이닿은변수 가져오기
        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            if (hitInfo.transform.gameObject.tag == "Player")
            {
                m_state = EnemyState.Attack;
                animator.SetTrigger("isAttack");
            }
        }

        Vector3 dir = target.transform.position - transform.position; //나와 Target(Player) 간의 방향 계산
        float distance = dir.magnitude; //거리 계산
        if (distance > attackRange) //만약 거리가 에너미의 공격 범위보다 길다?
        {

            m_state = EnemyState.Idle; //이러면 Move로 넘어간다
        }

        ////Ray를 발사시켜서 어딘가에 부딪혔다면
        //if (Physics.Raycast(ray, out hitInfo, 100, layer))
        //{

        //}

    }
    bool isAttack = false;
    public RaycastHit hitInfo;
    public void Attack()
    {



        Vector3 dir = target.transform.position - transform.position; //나와 Target(Player) 간의 방향 계산
        float distance = dir.magnitude; //거리 계산
        if (distance > attackRange) //만약 거리가 에너미의 공격 범위보다 길다?
        {

            m_state = EnemyState.Move; //이러면 Move로 넘어간다
        }

        //currTime += Time.deltaTime;
        //animator.SetTrigger("isAttackDelay");
        //if (currTime < fireTime)
        //    return;



        Vector3 dirE = target.transform.position - transform.position; //에너미가 바라보는방향으로
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //몸을돌린다

        Ray ray = new Ray();
        ray.origin = aimingPoint.position;
        ray.direction = aimingPoint.forward;

       

        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            LineRenderer lr = null;


            if (hitInfo.transform.gameObject.tag == "Player" )
            {
                //print("나 맞았어!!!");
                //if(isAttack == false)
                //{
                //    animator.SetTrigger("isAttack");
                //    isAttack = true;
                //} 

                GameObject line = Instantiate(LineRay);
                lr = line.GetComponent<LineRenderer>();
                lr.SetPosition(0, firePos.transform.position);
                lr.SetPosition(1, hitInfo.point);

                Destroy(line, 0.1f);
                hitInfo.transform.gameObject.GetComponent<SM_KH_Player_hp>().Damaged(.5f);
                currTime = 0f;

            }

            //else
            //{
            //    currTime = fireTime;
            //}

            if (lr != null)
                lr.SetPosition(1, hitInfo.point);
        }


    }


    bool isKnockBackFinish = false;
    float isDamagedTime = 2;

    void OnHit()
    {

        // 애니메이션 타이밍에 맞게 총을 쏘고싶다.
        // 애니메이션 타이밍

        // 총을 쏜다.

    }



    private void Damage()
    {
        //만약 넉백상태라면
        if (isKnockBackFinish == false)
        {
            transform.position = Vector3.Lerp(transform.position, knockbackPos, knockbackSpeed * Time.deltaTime);
            float distance = Vector3.Distance(transform.position, knockbackPos);

            if (distance < 0.1f)
            {

                transform.position = knockbackPos;
                isKnockBackFinish = true;

            }
            //넉백상태가 끝나면
            if (isKnockBackFinish)
            {
                currTime += Time.deltaTime;
                if (currTime > isDamagedTime)
                {
                    m_state = EnemyState.Idle;
                    currTime = 0;

                }
            }
        }
    }



    public float knockbackSpeed = 10;
    Vector3 knockbackPos;
    float maxHp = 5;
    public void OnDamageProcess(Vector3 shootDirection)
    {
        maxHp--;

        //if (m_state == EnemyState.Die)
        //{
        //    return;
        //}
        //// 코루틴을 종료하고 싶다.
        //StopAllCoroutines();


        if (maxHp <= 0)
        {
            m_state = EnemyState.Die;

            animator.SetTrigger("Die");

        }


        else
        {
            shootDirection.y = 0;
            knockbackPos = transform.position + shootDirection * 1;
            m_state = EnemyState.Idle;
            animator.SetTrigger("isDamaged");
            isKnockBackFinish = false;
        }
    }

    public float downSpeed = 2;

    private void Die()
    {
        currTime += Time.deltaTime;
        if (currTime > 2)
        {

            this.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            bc.enabled = false;
            Vector3 vt = Vector3.down * downSpeed * Time.deltaTime;
            Vector3 Po = transform.position;
            Vector3 P = Po + vt;
            transform.position = P;
            currTime = 0;

            if (P.y <= -1)
            {
                Destroy(gameObject);
            }

        }
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
        if (distance < AttackRange) //만약 player가 사정거리 안에 들어왔다면?
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
