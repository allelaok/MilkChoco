using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_KH_EnemyFire : MonoBehaviour
{
    float currTime;
    float gunDuration = 0.2f; //�� ����
    float EnemySpeed = 4.0f;
    float attackRange = 15.0f;
    public GameObject bulletFactory;
    public GameObject firePos;
    public GameObject target;
    public GameObject pos1;
    public GameObject pos2;
    public float rotSpeed = 2;
    public Transform aimingPoint; //�߻�����Ʈ
    public float fireTime = 0.1f; //����ӵ�
    public GameObject LineRay; //�Ѿ˹߻����(�ӽ�)
    public Animator animator;
    BoxCollider bc;
    Rigidbody rb;

    //CharacterController cc; //�̵��ϴ°� �Ⱦ� ����.��
    // Start is called before the first frame update

    //������
    enum EnemyState
    {
        Idle, //��� -> Move
        Move, //����̵�, �����ȿ����Ë� �����̸� ���ø��
        Detect, //���ø�� 
        Attack, //���ݸ��
        Damage, //�ǰ� ��������
        Die //�״´�
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
        print("���� ����: " + m_state);

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
        //���� ��ǥ �ΰ��� �Դٸ� ���ٸ� �Ѵ�

        //Pos1,2�ΰ��� ���� �� ����ȭ
        //Vector3 EnemyPos = transform.position; //����ġ        
        //Vector3 dirToPos1 = pos1.transform.position - transform.position; //pos1�� �ٶ󺸴� ����
        //Vector3 dirToPos2 = pos2.transform.position - transform.position;
        //float EnemyPos1Distance = dirToPos1.magnitude;
        //float EnemyPos2Distance = dirToPos2.magnitude;
        //float Distance = EnemyPos1Distance - EnemyPos2Distance;
        //float DistanceAbs = Mathf.Abs(Distance);
        //dirToPos1.Normalize();
        //dirToPos2.Normalize();

        //1. ù��° ��������Ʈ�� ������Ʈ�� �Ÿ��� ����Ѵ�
        //2. �Ÿ��� �������ϰ� �Ǹ� ��������Ʈ�� ��ȯ�Ѵ�.
        //3. ������Ʈ�� ������ ������ �ι�° ��������Ʈ�� ���� �̵��Ѵ�.
        //4. �Ÿ��� ���� ���ϰ� �Ǹ� ��������Ʈ�� ��ȯ�Ѵ�.
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

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); //pos1������ ����������
        transform.position += wayPointDir * EnemySpeed * Time.deltaTime; //pos1���������̵��Ѵ�\

        //if (EnemyPos1Distance > EnemyPos2Distance)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos1),
        //        rotSpeed * Time.deltaTime); //pos1������ ����������
        //    transform.position += dirToPos1 * EnemySpeed * Time.deltaTime; //pos1���������̵��Ѵ�
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos2),
        //        rotSpeed * Time.deltaTime);
        //    transform.position += dirToPos2 * EnemySpeed * Time.deltaTime;

        //}


        //���� player�� ���������� ���´ٸ�?        
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.magnitude;
        if (distance < attackRange)
        {
            //Detect�� �Ѿ��
            m_state = EnemyState.Detect;

        }
    }
    //�ӽ�
    //int layer = 1 << LayerMask.NameToLayer("");
    private void Detect()

    {
        animator.SetTrigger("isIdle");
        //print("Detect");
        Vector3 dirE = target.transform.position - transform.position; //���ʹ̰� �ٶ󺸴¹�������
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //����������
        Ray ray = new Ray();    //���� ����
        ray.origin = firePos.transform.position;    //���� ��ġ 
        ray.direction = transform.forward;  //���� ����
        RaycastHit hitInfo; //���̴������� ��������
        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            if (hitInfo.transform.gameObject.tag == "Player")
            {
                m_state = EnemyState.Attack;
                animator.SetTrigger("isAttack");
            }
        }

        Vector3 dir = target.transform.position - transform.position; //���� Target(Player) ���� ���� ���
        float distance = dir.magnitude; //�Ÿ� ���
        if (distance > attackRange) //���� �Ÿ��� ���ʹ��� ���� �������� ���?
        {

            m_state = EnemyState.Idle; //�̷��� Move�� �Ѿ��
        }

        ////Ray�� �߻���Ѽ� ��򰡿� �ε����ٸ�
        //if (Physics.Raycast(ray, out hitInfo, 100, layer))
        //{

        //}

    }
    bool isAttack = false;
    public RaycastHit hitInfo;
    public void Attack()
    {



        Vector3 dir = target.transform.position - transform.position; //���� Target(Player) ���� ���� ���
        float distance = dir.magnitude; //�Ÿ� ���
        if (distance > attackRange) //���� �Ÿ��� ���ʹ��� ���� �������� ���?
        {

            m_state = EnemyState.Move; //�̷��� Move�� �Ѿ��
        }

        //currTime += Time.deltaTime;
        //animator.SetTrigger("isAttackDelay");
        //if (currTime < fireTime)
        //    return;



        Vector3 dirE = target.transform.position - transform.position; //���ʹ̰� �ٶ󺸴¹�������
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //����������

        Ray ray = new Ray();
        ray.origin = aimingPoint.position;
        ray.direction = aimingPoint.forward;

       

        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            LineRenderer lr = null;


            if (hitInfo.transform.gameObject.tag == "Player" )
            {
                //print("�� �¾Ҿ�!!!");
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

        // �ִϸ��̼� Ÿ�ֿ̹� �°� ���� ���ʹ�.
        // �ִϸ��̼� Ÿ�̹�

        // ���� ���.

    }



    private void Damage()
    {
        //���� �˹���¶��
        if (isKnockBackFinish == false)
        {
            transform.position = Vector3.Lerp(transform.position, knockbackPos, knockbackSpeed * Time.deltaTime);
            float distance = Vector3.Distance(transform.position, knockbackPos);

            if (distance < 0.1f)
            {

                transform.position = knockbackPos;
                isKnockBackFinish = true;

            }
            //�˹���°� ������
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
        //// �ڷ�ƾ�� �����ϰ� �ʹ�.
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
        Vector3 dir = target.transform.position - transform.position; //���ʹ̰� �ٶ󺸴¹���
        dir.Normalize(); //����ȭ
        //transform.LookAt(target.transform); //���ʹ̰� Ÿ���� �ٶ󺻴�
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime); //����������        
        currTime += Time.deltaTime; //�ð����帣�µ�        
        if (gunDuration <= currTime) //�Ѿ˹߻�ӵ�
        {
            GameObject bullet = Instantiate(bulletFactory); //�Ѿ��� �����Ѵ�
            //transform.forward = dir;
            //dir = firePos.transform.forward;
            bullet.transform.position = firePos.transform.position; //firePos�� ���� �����Ѵ�
            bullet.transform.forward = firePos.transform.forward; //firePos �����̼ǵ� �ٲ��ش�


            Destroy(bullet, 3); //�����ð� ������ �ı��Ѵ�
            currTime = 0; //����ð� �ʱ�ȭ
        }
    }
    public float AttackRange = 15f;
    void findPlayer()
    {
        Vector3 dir = target.transform.position - transform.position; //P-E
        float distance = dir.magnitude; //P-E�Ÿ�
        if (distance < AttackRange) //���� player�� �����Ÿ� �ȿ� ���Դٸ�?
        {
            //���� �׹������� ���´� 
            Vector3 dirE = target.transform.position - transform.position; //���ʹ̰� �ٶ󺸴¹���
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
                rotSpeed * Time.deltaTime); //����������
            //���� ray�� �߻��ؼ� player�� �����ٸ�?
            //(����ġ��) �ұ��
            //����������� (�̰� �� �ȵ�, �������)
            //�Ѿ��� �߻��Ѵ�.
        }

        else
        {
            //�����·� ���ư���
        }


        //���� ���������ȿ�a ������ �ν��Ѵٸ�(�Ÿ���? ray��? ������Ϲ¤Ф�) 
        //������ �����(���ݸ��)
        //���� �ѱ��� ���������� ���´�
        //���̰� ���濡�� �����ٸ�? (�� �ȶո��� �ؾ���)
        //���� �߻��Ѵ� (�����ڵ�)
        //���� �������� �ۿ� �ִٸ� 
        //������(�������)�� �ٲ۴�
    }









    void justMemo()
    {
        //Enemy�� �����ϴٰ� player��ġ�� �����ϰ� �ѱ��� �÷��̾ ���Ѵ��� ��� 

        //���ʹ̰� ���������� �����Ѵ� (�������) //navi
        //- ���� �ΰ���� �͸����� ġ��
        //���� ���������ȿ�a ������ �ν��Ѵٸ�(�Ÿ���? ray��? ������Ϲ¤Ф�) 
        //������ �����(���ݸ��)
        //���� �ѱ��� ���������� ���´�
        //���̰� ���濡�� �����ٸ�? (�� �ȶո��� �ؾ���)
        //���� �߻��Ѵ� (�����ڵ�)
        //���� �������� �ۿ� �ִٸ� 
        //������(�������)�� �ٲ۴�

        //-------------------------------------------
        //�׳� ���̷� �ν��ϰ� �� ���� ������? ��

    }
}
