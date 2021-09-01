﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Enemy_A : MonoBehaviour
{
    
    Animator anim;

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

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public Vector3 rand_dir;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    //    rand_dir.x = Random.Range(-1.0f, 1.0f);
    //    rand_dir.y = Random.Range(-1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        print("현재상태 : " + m_state);
        
        // 피격 테스트
        if(Input.GetKeyDown(KeyCode.K))
        {
            OnHit(transform.forward * -1);
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
                Damaged();
                break;

            case EnemyState.Die:
                Die();
                break;
        }
    }

    
    public GameObject target;
    public float speed;
    CharacterController cc;
    public float rotSpeed = 5;

    //필요속성 : 공격범위
    



    public float detectRange;
    float currentTime;
    private void Idle()
    {
        m_state = EnemyState.Idle;

        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance < detectRange)
        {
           m_state = EnemyState.Move;
        }
    }

    private void Move()
    {
        // 타겟으로 이동
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        anim.SetBool("isWalk", true);

        // 2. P = PO + vt
        cc.SimpleMove(dir * speed);

        // 타겟으로 부드럽게 회전해서 이동
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

        //공격범위 안에 들어가면 상태를 공격으로 전환
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance +1 < attackRange)
        {
            m_state = EnemyState.Attack;
        }
    }

    public float attackDelayTime = 2;
    public float attackRange = 2;
   

    private void Attack()
    {

        // 일정시간에 한번씩 공격
        currentTime += Time.deltaTime;

        if (currentTime > attackDelayTime)
        {
            anim.SetBool("isAttack", true);

        }

        // 타겟이 공격 범위를 벗어나면 상태를 Move로 전환
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > attackRange + 2)
        {
            m_state = EnemyState.Move;
            anim.SetBool("isAttack", false);
        }
    }

    // 일정시간 기다렸다가 상태를 Idle 로 전환
    public float damageDelayTime = 2;

    private void Damaged()
    {
        // 일정시간에 한번씩 공격
        m_state = EnemyState.Damage;
        anim.SetBool("isDamaged", true);


    }

    float maxHp = 3;
   
    public void OnDamageProcess(Vector3 shootDirection)
    {
        maxHp--;

        if (maxHp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            shootDirection.y = 0;
            transform.position += shootDirection * 2;
            m_state = EnemyState.Damage;
            anim.SetBool("isDamaged", true);
        }
    }
    private void Die()
    {

    }


    // 총에 맞았을 때 호출될 함수
    public void OnHit(Vector3 knockbackDir)
    {
        HitProcess();
    }

    // 처리할 함수(죽고싶다.)
    public void HitProcess()
    {
        Destroy(gameObject);
    }
}
