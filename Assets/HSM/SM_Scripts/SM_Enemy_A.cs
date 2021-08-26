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

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        print("현재상태 : " + m_state);
        
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

    
    public GameObject target;
    public float speed;
    CharacterController cc;
    public float rotSpeed = 5;

    //필요속성 : 공격범위
    public float attackRange = 2;



    public float idleDelayTime = 2;
    float currentTime;
    private void Idle()
    {
        // 일정 시간이 지나면 상태를 Move로 전환
        currentTime += Time.deltaTime;
       
        if (currentTime > idleDelayTime)
        {
            m_state = EnemyState.Move;
            currentTime = 0;
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
        if (attackRange > distance)
        {
            m_state = EnemyState.Attack;
        }
    }

    public float attackDelayTime = 2;

    private void Attack()
    {

        // 일정시간에 한번씩 공격
        currentTime += Time.deltaTime;
        
        if (currentTime > attackDelayTime)
        {
            
            print("Attack");
            currentTime = 0;
        }

        // 타겟이 공격 범위를 벗어나면 상태를 Move로 전환
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > attackRange)
        {
            m_state = EnemyState.Move;
        }
    }

    // 일정시간 기다렸다가 상태를 Idle 로 전환
    public float damageDelayTime = 2;

    private void Damage()
    {
        // 일정시간에 한번씩 공격
        currentTime += Time.deltaTime;
        
        if (currentTime > damageDelayTime)
        {
            m_state = EnemyState.Idle;
            currentTime = 0;
        }


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
        }
    }
    private void Die()
    {

    }
}