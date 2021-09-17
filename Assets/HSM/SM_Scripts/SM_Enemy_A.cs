using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SM_Enemy_A : MonoBehaviour
{
    //피격 함수 만들자.
    //public static SM_Enemy_A instance;
    Animator anim;

    BoxCollider bc;

    //열거형
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    }

    EnemyState m_state = EnemyState.Idle;

    public Vector3 rand_dir;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        bc = GetComponent<BoxCollider>();
        target = GameObject.Find("Na_Player");
    }

    // Update is called once per frame
    void Update()
    {
        //print("현재상태 : " + m_state);
        
        // 피격 테스트
        if(Input.GetKeyDown(KeyCode.K))
        {
            OnDamageProcess(transform.forward * -1);
        }

        switch (m_state)
        {

            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Move:
                Move();
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

    
    GameObject target;
    public float speed;
    public float rotSpeed = 5;

    //필요속성 : 공격범위
    



    float detectRange = 10;
    float currentTime;
    private void Idle()
    {
        m_state = EnemyState.Idle;
        anim.SetTrigger("doIdle");

        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance < detectRange)
        {
            m_state = EnemyState.Move;
            anim.SetTrigger("isWalk");
        }

    }

    private void Move()
    {
        // 타겟으로 이동
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        //dir.y = 0;

        //// 2. P = PO + vt
        //cc.SimpleMove(dir * speed);
        //dir.y = 0;

        // 타겟으로 부드럽게 회전해서 이동
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        transform.position += dir * speed * Time.deltaTime;

        //공격범위 안에 들어가면 상태를 공격으로 전환
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance +1 < attackRange)
        {
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
        }
        if(distance >= detectRange)
        {
            m_state = EnemyState.Idle;
        }
    }

    public float attackDelayTime = 2;
    public float attackRange = 2;


    bool isAttackCheck = false;
    float elaspedAttackTime;

    bool isAttackToWalk = false;
    private void Attack()
    {
       
        // Move 로 상태 전환 해야 할때
        if (isAttackToWalk)
        {
            // -> 일정시간 기다렸다가 전환
            currentTime += Time.deltaTime;
            if(currentTime > 2)
            {

                m_state = EnemyState.Move;
                anim.SetTrigger("isWalk");
                Na_Player.instace.Damaged(10);
                isAttackToWalk = false;
            }
            return;
        }

        // 일정시간에 한번씩 공격
        currentTime += Time.deltaTime;

        // 공격애니메이션이 진행중이지 않고, 대기시간보다 경과시간이 커지면
        if (isAttackCheck == false && currentTime > attackDelayTime)
        {
            anim.SetTrigger("isAttack");
            currentTime = 0;
            isAttackCheck = true;
            //print("Attack");
        }
        // 공격애니메이션이 진행중
        if (isAttackCheck)
        {
            elaspedAttackTime += Time.deltaTime;
            if(elaspedAttackTime > 2)
            {
                elaspedAttackTime = 0;
                isAttackCheck = false;
            }
        }

        if (isAttackCheck == false)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;
            transform.forward = dir;

            // 타겟이 공격 범위를 벗어나면 상태를 Move로 전환
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance > attackRange + 2)
            {
                isAttackToWalk = true;
                
            }
        }
    }
    bool IsKnockbackFinish = false;
    float elaspedDamagedTime = 0.5f;

    private void Damage()
    {
        if (IsKnockbackFinish == false)
        {
            transform.position = Vector3.Lerp(transform.position, knockbackPos, knockbackSpeed * Time.deltaTime);
            // 피격을 받는 도중에는 피격 애니메이션을 보여주고 싶다.
            float distance = Vector3.Distance(transform.position, knockbackPos);


            // 1. 넉백이 끝나면
            if (distance < 0.1f)
            {
                transform.position = knockbackPos;
                IsKnockbackFinish = true;
                
            }
        }

        // 2. 피격 대기 시간만큼 기다리고 싶다.
        if (IsKnockbackFinish == true)
        {
            currentTime += Time.deltaTime;
            if (currentTime > elaspedDamagedTime)
            {
                m_state = EnemyState.Idle;
                currentTime = 0;
               
            }
        }
        


    }
    public float damageDelayTime = 2;
    public float knockbackSpeed = 10;

    //private IEnumerator Damage(Vector3 shootDirection)
    //{
    //    shootDirection.y = 0;
    //    transform.position += shootDirection * 2;
    //    m_state = EnemyState.Damage;
    //    anim.SetTrigger("Damage");

    //    // 대기시간 만큼 기다렸다가 
    //    yield return new WaitForSeconds(damageDelayTime);
    //    // 상태를 Idle 로 전환
    //    m_state = EnemyState.Idle;

    //}
    

    float maxHp = 5;
    Vector3 knockbackPos;
    public void OnDamageProcess(Vector3 shootDirection)
    {
        maxHp--;

        if (maxHp <= 0)
        {
            m_state = EnemyState.Die;
            anim.SetTrigger("Die");
        }
        else
        {
            

            shootDirection.y = 0;
            // 넉백
            // P = P0 + vt;
            knockbackPos = transform.position + shootDirection * 2;
            m_state = EnemyState.Damage;
            anim.SetTrigger("Damage");
            IsKnockbackFinish = false;
            currentTime = 0;
            

        }
    }


    public float downSpeed = 2;
    private void Die()
    {
        
        currentTime += Time.deltaTime;
        if (currentTime > 2)
        {
            this.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            bc.enabled = true;
            Vector3 vt = Vector3.down * downSpeed * Time.deltaTime;
            Vector3 Po = transform.position;
            Vector3 P = Po + vt;
            transform.position = P;

            if(P.y <= -1)
            {
                Destroy(gameObject);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("FallZone"))
        {
            transform.position = GetComponentInParent<SM_Enemy_M>().respawnPoint.transform.position;
        }
    }
}
