using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_EnemyFire : MonoBehaviour
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
    public GameObject LineF; //�Ѿ˹߻����(�ӽ�)
    
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
    EnemyState m_state= EnemyState.Idle;
    void Start()
    {
        //cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
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

    public float IdleDelayTime = 2.0f;    

    private void Idle()
    {
        currTime += Time.deltaTime;
        if (currTime > IdleDelayTime)
        {
            m_state = EnemyState.Move;
            currTime = 0;
        }
    }

    private void Move()
    {
        //���� ��ǥ �ΰ��� �Դٸ� ���ٸ� �Ѵ�

        //Pos1,2�ΰ��� ���� �� ����ȭ
        Vector3 EnemyPos = transform.position; //����ġ
        Vector3 pos1Pos = pos1.transform.position;
        
        Vector3 pos2Pos = pos2.transform.position;
        Vector3 dirToPos1 = pos1.transform.position - transform.position; //pos1�� �ٶ󺸴� ����
        Vector3 dirToPos2 = pos2.transform.position - transform.position;
        dirToPos1.Normalize();
        dirToPos2.Normalize();
        //enemy�� pos1�� ���Ѵ�

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos1),
                rotSpeed * Time.deltaTime); //����������
        transform.position += dirToPos1 * EnemySpeed * Time.deltaTime; //pos1���������̵��Ѵ�
        //�������µ� �ɸ��� �ð�??????/

        //���� enemy�� ��ġ�� pos1�̶��

        if (transform.position== pos1.transform.position)
        {
            //ĳ������ ���� pos2�������� ���� ������ 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToPos2),
                rotSpeed * Time.deltaTime);
            //ĳ���Ͱ� pos2�� ���Ѵ�
            transform.position += dirToPos2 * EnemySpeed * Time.deltaTime;
            if (transform.position == pos2.transform.position)
            {
                
            }
        }

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
        //print("Detect");
        //Ray ray = new Ray();    //���� ����
        //ray.origin = aimingPoint.transform.position;    //���� ��ġ 
        //ray.direction = aimingPoint.transform.forward;  //���� ����
        //RaycastHit hitInfo; //���̴������� ��������
        //                    //Ray�� �浹 �ϰ� ���� Layer ���߿� ����

        ////Ray�� �߻���Ѽ� ��򰡿� �ε����ٸ�
        //if (Physics.Raycast(ray, out hitInfo, 100, layer))
        //{

        //}

    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

    private void Damage()
    {
        throw new NotImplementedException();
    }

    private void Die()
    {
        throw new NotImplementedException();
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
        if (distance<AttackRange) //���� player�� �����Ÿ� �ȿ� ���Դٸ�?
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
