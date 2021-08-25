using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_EnemyFire : MonoBehaviour
{
    float currTime;
    float gunDuration = 0.2f;
    public GameObject bulletFactory;
    public GameObject firePos;
    public GameObject target;
    public float rotSpeed = 5;
    //CharacterController cc; //�̵��ϴ°� �Ⱦ� ����.��
    // Start is called before the first frame update

    //������
    enum EnemyState
    {
        Idle,
        Detect,
        Attack,
        Damage,
        Die
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

    private void Idle()
    {
       
    }

    private void Detect()
    {
        throw new NotImplementedException();
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
