using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SM_Enemy_A : MonoBehaviour
{
    

    Animator anim;

    Rigidbody rb;
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

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
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
        anim.SetBool("isWalk",true);

        // 2. P = PO + vt
        cc.SimpleMove(dir * speed);
        dir.y = 0;

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


    bool isAttackCheck = false;
    float elaspedAttackTime;
    private void Attack()
    {

        // 일정시간에 한번씩 공격
        currentTime += Time.deltaTime;

        // 공격애니메이션이 진행중이지 않고, 대시간보다 경과시간이 커지면
        if (isAttackCheck == false && currentTime > attackDelayTime)
        {
            anim.SetBool("isAttack",true);
            currentTime = 0;
            isAttackCheck = true;
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
                m_state = EnemyState.Move;
                anim.SetBool("isWalk", true);
            }
        }
    }

    

    private void Damaged()
    {
        // 넉백 이동처리
        transform.position = Vector3.Lerp(transform.position, knockbackPos, knockbackSpeed * Time.deltaTime);

        // 넉벡이 다 되면 상태를 Idle로 전환
        float distance = Vector3.Distance(transform.position, knockbackPos);
        if (distance < 0.1f)
        {
            transform.position = knockbackPos;
            new WaitForSeconds(damageDelayTime);
            m_state = EnemyState.Idle;

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
            knockbackPos = transform.position + shootDirection * 5;
            m_state = EnemyState.Damage;
            anim.SetTrigger("Damage");
            

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
}
