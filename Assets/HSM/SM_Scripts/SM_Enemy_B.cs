using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ���� �⺻ ����(����)�� �����ϰ� �ʹ�.
// 1. hp �� ���� �ʹ�.
// 2. ������ ���¸� �ǰ����� ��ȯ�ϰ� �ʹ�.
// 3. hp �� 0 ���ϸ� ���ְ� �ʹ�.

// ���� �� ���¿��� �ִϸ��̼��� ����ǵ��� �ϰ�ʹ�.
// 1. Idle -> Move �ִϸ��̼��� ��ȯ�ǵ��� �ϰ� �ʹ�.
// �ʿ�Ӽ� : Animator ������Ʈ

// Navigation �� �̿��� AI ��ã�⸦ �����ϰ� �ϰ� �ʹ�.
[RequireComponent(typeof(CharacterController))]
public class SM_Enemy_B : MonoBehaviour
{
    // ������
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
        // target �� ã�Ƽ� �Ҵ��� ����
        target = GameObject.Find("Na_Player");


        // CharacterController ��������
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        print("������� : " + m_state);

        // ���� �⺻ ����(����)�� �����ϰ� �ʹ�.
        // ���� ���� ���°� Idle �̶��
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
        // Ÿ�������� �̵��ϰ� �ʹ�.
        // 1. ������ �ʿ�
        // -> direction = target - me
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;

        // agent �� �̿��ؼ� �̵��ϱ�
        agent.destination = target.transform.position;

       
        if (distance < attackRange)
        {
            // 2. ���¸� �������� ��ȯ�ϰ� �ʹ�.
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            agent.enabled = false;
        }
    }

 
    public float attackDelayTime = 2;
    private void Attack()
    {

       
        currentTime += Time.deltaTime;
        // 2. ���ݽð��� �����ϱ�
        if (currentTime > attackDelayTime)
        {
            // �����ϰ� �ʹ�.

            anim.SetTrigger("isAttack");
            currentTime = 0;

        }

        
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        transform.forward = dir;

        // Ÿ���� ���� ������ ����� ���¸� Move �� ��ȯ�ϰ� �ʹ�.
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > attackRange)
        {
            // ���¸� Move �� ��ȯ�ϰ� �ʹ�.
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

        // ���ð� ��ŭ ��ٷȴٰ� 
        yield return new WaitForSeconds(damageDelayTime);
        // ���¸� Idle �� ��ȯ
        m_state = EnemyState.Idle;

       
    }

    // �÷��̾�κ��� �ǰ� �޾����� ó���� �Լ�
    Vector3 knockbackPos;
    public void OnDamageProcess(Vector3 shootDirection)
    {
        // ���� �ִ� �� �ǰ�ó�� �ϰ� ���� �ʴ�.
        if (m_state == EnemyState.Die)
        {
            return;
        }
        // �ڷ�ƾ�� �����ϰ� �ʹ�.
        StopAllCoroutines();

        agent.enabled = false;

        currentTime = 0;
        hp--;
        
        if (hp <= 0)
        {
            // ������ �浹ü ��������
            cc.enabled = false;
            m_state = EnemyState.Die;
            anim.SetTrigger("isDie");
            //Destroy(gameObject);
        }
      
        else
        {
            
            // �ǰ�ó���� �ڷ�ƾ�� �̿��Ͽ� ó���ϰ� �ʹ�.
            StartCoroutine(Damage(shootDirection));

        }
    }

 
    public float downSpeed = 2;
    
    private void Die()
    {
        
        currentTime += Time.deltaTime;
        if (currentTime > 2)
        {
            // �Ʒ��� ���������� ����.
            // P = P0 + vt
            Vector3 vt = Vector3.down * downSpeed * Time.deltaTime;
            Vector3 P0 = transform.position;
            Vector3 P = P0 + vt;
            transform.position = P;

          
            if (P.y <= -1)
            {
              
                SM_EnemyManager.Instance.enemyPool.Add(gameObject);
                // ���� ��Ȱ��ȭ���Ѿ� �Ѵ�.
                gameObject.SetActive(false);
    
            }
        }
    }

}
