using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 적의 기본 상태(목차)를 구성하고 싶다.
// 1. hp 를 갖고 싶다.
// 2. 맞으면 상태를 피격으로 전환하고 싶다.
// 3. hp 가 0 이하면 없애고 싶다.

// 적이 각 상태에서 애니메이션이 재생되도록 하고싶다.
// 1. Idle -> Move 애니메이션이 전환되도록 하고 싶다.
// 필요속성 : Animator 컴포넌트

// Navigation 을 이용한 AI 길찾기를 수행하게 하고 싶다.
[RequireComponent(typeof(CharacterController))]
public class SM_Enemy_B : MonoBehaviour
{
    // 열거형
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    }

    EnemyState m_state = EnemyState.Idle;

    public int hp = 3;

    Animator anim;

    NavMeshAgent agent;

    void Start()
    {
        // target 을 찾아서 할당해 주자
        target = GameObject.Find("Na_Player");


        // CharacterController 가져오기
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        print("현재상태 : " + m_state);

        // 적의 기본 상태(목차)를 구성하고 싶다.
        // 만약 적의 상태가 Idle 이라면
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
            //case EnemyState.Damage:
            //    Damage();
            //    break;
            case EnemyState.Die:
                Die();
                break;
        }
    }


    
    public float idleDelayTime = 2;
    float currentTime;
    private void Idle()
    {
        
        currentTime += Time.deltaTime;
        
        if (currentTime > idleDelayTime)
        {
           
            m_state = EnemyState.Move;
          
            anim.SetTrigger("isWalk");
            currentTime = 0;
            agent.enabled = true;
        }
    }

    
    public GameObject target;
    public float speed = 5;
    CharacterController cc;
    
    
    public float attackRange = 2;
    private void Move()
    {
        // 타겟쪽으로 이동하고 싶다.
        // 1. 방향이 필요
        // -> direction = target - me
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;

        // agent 를 이용해서 이동하기
        agent.destination = target.transform.position;

       
        if (distance < attackRange)
        {
            // 2. 상태를 공격으로 전환하고 싶다.
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            agent.enabled = false;
        }
    }

 
    public float attackDelayTime = 2;
    private void Attack()
    {

       
        currentTime += Time.deltaTime;
        // 2. 공격시간이 됐으니까
        if (currentTime > attackDelayTime)
        {
            // 공격하고 싶다.

            anim.SetTrigger("isAttack");
            currentTime = 0;

        }

        
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        transform.forward = dir;

        // 타겟이 공격 범위를 벗어나면 상태를 Move 로 전환하고 싶다.
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > attackRange)
        {
            // 상태를 Move 로 전환하고 싶다.
            m_state = EnemyState.Move;
            anim.SetTrigger("isWalk");
            agent.enabled = true;
        }

    }


    public float damageDelayTime = 2;
    private IEnumerator Damage(Vector3 shootDirection)
    {
        shootDirection.y = 0;
        transform.position += shootDirection * 2;
        m_state = EnemyState.Damage;
        
        anim.SetTrigger("isDamage");

        // 대기시간 만큼 기다렸다가 
        yield return new WaitForSeconds(damageDelayTime);
        // 상태를 Idle 로 전환
        m_state = EnemyState.Idle;

       
    }

    // 플레이어로부터 피격 받았을때 처리할 함수
    Vector3 knockbackPos;
    public void OnDamageProcess(Vector3 shootDirection)
    {
        // 죽은 애는 또 피격처리 하고 싶지 않다.
        if (m_state == EnemyState.Die)
        {
            return;
        }
        // 코루틴을 종료하고 싶다.
        StopAllCoroutines();

        agent.enabled = false;

        currentTime = 0;
        hp--;
        
        if (hp <= 0)
        {
            // 죽으면 충돌체 꺼버리자
            cc.enabled = false;
            m_state = EnemyState.Die;
            anim.SetTrigger("isDie");
            //Destroy(gameObject);
        }
      
        else
        {
            
            // 피격처리를 코루틴을 이용하여 처리하고 싶다.
            StartCoroutine(Damage(shootDirection));

        }
    }

 
    public float downSpeed = 2;
    
    private void Die()
    {
        
        currentTime += Time.deltaTime;
        if (currentTime > 2)
        {
            // 아래로 내려가도록 하자.
            // P = P0 + vt
            Vector3 vt = Vector3.down * downSpeed * Time.deltaTime;
            Vector3 P0 = transform.position;
            Vector3 P = P0 + vt;
            transform.position = P;

          
            if (P.y <= -1)
            {
              
                SM_EnemyManager.Instance.enemyPool.Add(gameObject);
                // 나를 비활성화시켜야 한다.
                gameObject.SetActive(false);
    
            }
        }
    }

}
