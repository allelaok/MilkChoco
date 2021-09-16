using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_FakeEnemy : MonoBehaviour
{
    //----
    Vector3 startChocoPos;
    public GameObject startEnemyPos;

    public float IdleDelayTime = 2.0f;

    Animator anim;



    enum EnemyState
    {
        Idle, //��� -> Move
        Move, //����̵�, �����ȿ����Ë� �����̸� ���ø��
        Detect, //���ø�� 
        Attack, //���ݸ��
        Damage, //�ǰ� ��������
        Die //�״´�
    }

    // Start is called before the first frame update
    void Start()
    {
        //i = 0;
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        //startEnemyPos = transform.position;
    }
    private void OnEnable()
    {
        transform.position = startEnemyPos.transform.position;
        doMove = false;
        k = 0;
    }

    private void OnDisable()
    {
        transform.position = startEnemyPos.transform.position;
        doMove = false;
        k = 0;
        //transform.position = startEnemyPos.transform.position;

    }

    EnemyState m_state = EnemyState.Idle;
    // Update is called once per frame

    bool doMove;
    void Update()
    {

        //print(m_state);
        switch (m_state)
        {
            case EnemyState.Idle:
                Idle();
                doMove = false;
                break;
            case EnemyState.Move:

                doMove = true;
                break;
            case EnemyState.Detect:

                Detect();
                doMove = false;
                break;
            case EnemyState.Attack:
                Attack();
                doMove = false;
                break;
            case EnemyState.Damage:
                Damage();
                doMove = false;
                break;
            case EnemyState.Die:
                Die();
                doMove = false;
                break;
        }
    }


    private void FixedUpdate()
    {
        if (doMove)
        {
            Move();
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
            anim.SetBool("IsMove", true);
        }
    }

    public GameObject target; //Ÿ��= �÷��̾�
    public float attackRange = 15f; //��Ÿ�
    //-------attack enemy �� �� ���-------
    public float yVelocity; //���� ���ư��� ��
    public Transform[] pos; // ��ǥ
    Vector3 dir;
    public float speed = 5;
    int k;
    public float gravity = 1;
    bool isJump = false;
    CharacterController cc;
    public float jumpForwardSpeed = 10;
    float localSpeed;
    bool isJumpZone;
    bool canDetect; //@@@@@@@@@
    private void Move()
    {
        //attack enemy ��ũ��Ʈ �����ͼ� �ִ´�
        if (cc.isGrounded)
        {
            //i++;
            //print("�����ִ�");
            //canDetect = true; //�����Ҷ� Detect���ϰ��Ѵ�.
            isJump = false;
            localSpeed = speed;
        }
        else
        {
        }
        dir = pos[k + 1].position - transform.position;
        //print(i);
        dir.Normalize();
        dir.y = 0;
        float y = 0;
        //print("local speed 111111111111111 " + localSpeed);
        Jump(out y);
        //print("local speed --------------> " + localSpeed);

        dir *= localSpeed * Time.deltaTime;
        //Debug.DrawLine(transform.position, transform.position + dir * 100, Color.red);
        dir.y = y;
        cc.Move(dir);  //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@�̰ż����Ѵ�@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        Vector3 rotDir = dir;
        rotDir.y = 0;


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotDir), rotSpeed * Time.deltaTime); //����Ʈ�� �κ�
        Choco();




        //���� player�� ���������� ���´ٸ�?
        Vector3 Pdir = target.transform.position - transform.position; //Pdir �� ����
        float distance = Pdir.magnitude;
        if (distance < attackRange && canDetect == true)
        {
            anim.SetBool("IsMove", false);
            anim.SetBool("IsMove", false);
            //Detect�� �Ѿ��
            m_state = EnemyState.Detect;
            anim.SetBool("IsAttack", true);

        }
    }
    public Transform chocoPos;
    GameObject isChoco;
    public GameObject[] chocoContainer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FakePos")
        {
            if (other.gameObject.tag == "FakePos")
            {
                k++;
            }


            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                //�ٸ� ĳ���� �ٲ۴�..?
                k = 0;
            }
        }
        //===========

        //if (isChoco == null)
        //{
        //    if (other.gameObject.tag == "Choco")
        //    {
        //        print("���ڸ���");
        //        isChoco = other.gameObject;
        //        startChocoPos = other.gameObject.transform.position;
        //        //startChocoPos = isChoco.transform.position;
        //    }
        //}
        //if (isChoco != null)
        {
            //print("���ڸԹ���");
            //if (other.gameObject.tag =="ChocoContainer")
            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                //print("���ھ߹̾߹̾߹̾߹�");
                //chocoContainer[KH_GameManager.instance.chocoCount].SetActive(true);
                //KH_GameManager.instance.chocoCount++;
                //Destroy(isChoco.gameObject);
                //isChoco = null;

                //������ �ش� enemy getactive false �ϰ� �״��� enemy �����Ѵ� i++
                //int i = KH_GameManager.instance.i;
                //KH_GameManager.instance.enemyStart[i].SetActive(false);
                //KH_GameManager.instance.i++;
                //KH_GameManager.instance.enemyStart[i].SetActive(true);
                KH_FakeManager.instance.isChoco = true;

                //�׸��� ���࿡ �� ä��ٸ� �й� Scene������...
            }
        }


        if (other.gameObject.tag == "JumpZone")
        {
            //print("JJJ");
            isJumpZone = true;
            isJump = true;
            canDetect = false;
        }

    }




    void Choco()
    {
        if (isChoco != null)
            isChoco.transform.position = chocoPos.position;

        if (KH_GameManager.instance.chocoCount == 4)
        {
            print("chocoMax");
        }
    }

    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
    public int jumpCount;
    public int MaxJumpCount = 1;
    public float jumpZonePower = 15f;
    public void Jump(out float dirY)
    {
        if (cc.isGrounded)
        {
            anim.SetBool("IsJump", false);
            //print("��");
            yVelocity = 0;
            //jumpCount = 0;
            canDetect = true;

        }

        if (isJumpZone)
        {
            //print("�پ�");
            yVelocity = jumpZonePower;
            //jumpCount++;
            isJumpZone = false;
            localSpeed = jumpForwardSpeed;
            //yVelocity -= gravity * Time.deltaTime;
            canDetect = false;
            anim.SetBool("IsJump", true);
        }

        yVelocity -= gravity * Time.deltaTime;
        dirY = yVelocity;


    }

    private void Detect()

    {
        //print("Detect");
        Vector3 dirE = target.transform.position - transform.position; //���ʹ̰� �ٶ󺸴¹�������
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //����������
        Ray ray = new Ray();    //���� ����
        ray.origin = aimingPoint.transform.position;    //���� ��ġ 
        ray.direction = aimingPoint.transform.forward;  //���� ����

        print("���̹߻�");


        Vector3 dir = target.transform.position - transform.position; //���� Target(Player) ���� ���� ���
        float distance = dir.magnitude; //�Ÿ� ���
        if (distance > attackRange || (Na_Player.instace.isDie)) //���� �Ÿ��� ���ʹ��� ���� �������� ���?
        {
            anim.SetBool("IsAttack", false);
            anim.SetBool("IsMove", true);
            m_state = EnemyState.Move; //�̷��� Move�� �Ѿ��

        }

        RaycastHit hitInfo; //���̴������� ��������
        if (Physics.Raycast(ray, out hitInfo, 1000))
        {
            if (hitInfo.transform.gameObject.tag == "Player")
            {
                m_state = EnemyState.Attack;
            }
        }



    }


    public float rotSpeed = 2;
    public Transform aimingPoint; //�߻�����Ʈ
    public GameObject LineRay; //�Ѿ˹߻����(�ӽ�)
    public float fireTime = 0.1f; //����ӵ�

    private void Attack()
    {
        //print("������");
        Vector3 dirE = target.transform.position - transform.position; //���ʹ̰� �ٶ󺸴¹�������
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirE),
            rotSpeed * Time.deltaTime); //����������

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
                    hitInfo.transform.gameObject.GetComponent<Na_Player>().Damaged(10f);
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

        Vector3 dir = target.transform.position - transform.position; //���� Target(Player) ���� ���� ���
        float distance = dir.magnitude; //�Ÿ� ���
        if (distance > attackRange || (Na_Player.instace.isDie)) //���� �Ÿ��� ���ʹ��� ���� �������� ���?
        {
            anim.SetBool("IsAttack", false);
            anim.SetBool("IsMove", true);
            m_state = EnemyState.Move; //�̷��� Move�� �Ѿ��
        }

    }

    private void Damage()
    {
        //throw new NotImplementedException();
    }

    private void Die()
    {
        //throw new NotImplementedException();
    }
    public void DieAnim()
    {
        anim.SetBool("IsDie", true);
    }
}
