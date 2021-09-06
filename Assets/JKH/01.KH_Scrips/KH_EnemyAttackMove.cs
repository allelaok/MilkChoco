using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KH_EnemyAttackMove : MonoBehaviour
{
    //----
    Vector3 startChocoPos;
    Vector3 startEnemyPos;

    public float IdleDelayTime = 2.0f;

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
        i = 0;
        cc = GetComponent<CharacterController>();
        startEnemyPos = transform.position;
    }

    EnemyState m_state = EnemyState.Idle;
    // Update is called once per frame
    void Update()
    {
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


    float currTime;
    private void Idle()
    {
        currTime += Time.deltaTime;
        if (currTime > IdleDelayTime)
        {
            m_state = EnemyState.Move;
            currTime = 0;
        }
    }

    public GameObject target; //Ÿ��= �÷��̾�
    public float attackRange = 15f; //��Ÿ�
    //-------attack enemy �� �� ���-------
    public float yVelocity; //���� ���ư��� ��
    public Transform[] pos; // ��ǥ
    Vector3 dir;
    public float speed = 5;
    int i;    
    public float gravity = 1;
    bool isJump = false;
    CharacterController cc;
    public float jumpForwardSpeed = 10;
    float localSpeed;
    bool isJumpZone;
    private void Move()
    {
        //attack enemy ��ũ��Ʈ �����ͼ� �ִ´�
        if (cc.isGrounded)
        {
            //i++;
            isJump = false;
            localSpeed = speed;
        }
        else
        {
        }
        dir = pos[i + 1].position - transform.position;
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

        Choco();




        //���� player�� ���������� ���´ٸ�?
        Vector3 Pdir = target.transform.position - transform.position; //Pdir �� ����
        float distance = Pdir.magnitude;
        if (distance < attackRange)
        {
            //Detect�� �Ѿ��
            m_state = EnemyState.Detect;
            
        }
    }
    public Transform chocoPos;
    GameObject isChoco;
    public GameObject[] chocoContainer;
    int chocoCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pos")
        {
            if (other.gameObject.tag == "Pos")
            {
                i++;
            }


            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                //�ٸ� ĳ���� �ٲ۴�..?
                i = 0;
            }
        }
        //===========

        if (isChoco == null)
        {
            if (other.gameObject.tag == "Choco")
            {
                print("���ڸ���");
                isChoco = other.gameObject;
                startChocoPos = other.gameObject.transform.position;
                //startChocoPos = isChoco.transform.position;
            }
        }
        if (isChoco != null)
        {
            print("���ڸԹ���");
            //if (other.gameObject.tag =="ChocoContainer")
            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                print("���ھ߹̾߹̾߹̾߹�");
                chocoContainer[chocoCount].SetActive(true);
                chocoCount++;
                Destroy(isChoco.gameObject);
                isChoco = null;

                //������ �ش� enemy getactive false �ϰ� �״��� enemy �����Ѵ� i++
                //int i = KH_GameManager.instance.i;
                //KH_GameManager.instance.enemyStart[i].SetActive(false);
                //KH_GameManager.instance.i++;
                //KH_GameManager.instance.enemyStart[i].SetActive(true);
                KH_GameManager.instance.isChoco = true;

                //�׸��� ���࿡ �� ä��ٸ� �й� Scene������...
            }
        }

        
        if (other.gameObject.tag == "JumpZone")
        {
            //print("JJJ");
            isJumpZone = true;
            isJump = true;
        }

    }




    void Choco()
    {
        if (isChoco != null)
            isChoco.transform.position = chocoPos.position;

        if (chocoCount == 4)
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
            //print("��");
            yVelocity = 0;
            //jumpCount = 0;

        }

        if (isJumpZone)
        {
            //print("�پ�");
            yVelocity = jumpZonePower;
            //jumpCount++;
            isJumpZone = false;
            localSpeed = jumpForwardSpeed;
            //yVelocity -= gravity * Time.deltaTime;
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
        RaycastHit hitInfo; //���̴������� ��������
        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            if (hitInfo.transform.gameObject.tag == "Player")
            {
                m_state = EnemyState.Attack;
            }
        }

        Vector3 dir = target.transform.position - transform.position; //���� Target(Player) ���� ���� ���
        float distance = dir.magnitude; //�Ÿ� ���
        if (distance > attackRange) //���� �Ÿ��� ���ʹ��� ���� �������� ���?
        {

            m_state = EnemyState.Move; //�̷��� Move�� �Ѿ��
        }

        

    }


    public float rotSpeed = 2;
    public Transform aimingPoint; //�߻�����Ʈ
    public GameObject LineRay; //�Ѿ˹߻����(�ӽ�)
    public float fireTime = 0.1f; //����ӵ�

    private void Attack()
    {
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
                    hitInfo.transform.gameObject.GetComponent<KH_Player_hp>().Damaged(.5f);
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
        if (distance > attackRange) //���� �Ÿ��� ���ʹ��� ���� �������� ���?
        {

            m_state = EnemyState.Move; //�̷��� Move�� �Ѿ��
        }

    }

    private void Damage()
    {
        throw new NotImplementedException();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
