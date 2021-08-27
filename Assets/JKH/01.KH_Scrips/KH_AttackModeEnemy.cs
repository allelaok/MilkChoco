using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_AttackModeEnemy : MonoBehaviour
{
    public Transform posLeft;
    public Transform posRight;
    public Transform posGoal;
    public float enemySpeed = 3;
    public float rotSpeed = 3;
    int rand;
    // Start is called before the first frame update
    void Start()
    {
        rand = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (rand == 0)
        {
            print("��");
            var distance2 = Vector3.Distance(transform.position, posLeft.position);
            
            if (Vector3.Distance(gameObject.transform.position, posLeft.position) <= 1f)
            {
                
                posLeft = posGoal;
            }

            //if (Vector3.Distance(gameObject.transform.position, posGoal.position) <= 1f)
            //{
            //    Debug.Log("1");
            //    currentWayPoint = wayPoint1;
            //}

            var wayPointDir = posLeft.transform.position - transform.position; //gameObject.transform.position
            wayPointDir.Normalize();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); //pos1������ ����������
            transform.position += wayPointDir * enemySpeed * Time.deltaTime; //pos1���������̵��Ѵ�\
            //Vector3 leftDir = posLeft.transform.position - transform.position;
            //leftDir.Normalize();
            //���ʹ������� �̵�
            //transform.position += leftDir * enemySpeed * Time.deltaTime;
            //������ �����Ѵٸ�
            //������������ �̵��Ѵ�

        }

        if(rand==1)
        {
            print("��");
            var distance2 = Vector3.Distance(gameObject.transform.position, posRight.position);
            
            if (Vector3.Distance(gameObject.transform.position, posRight.position) <= 1f)
            {
                
                posRight = posGoal;
            }

            //if (Vector3.Distance(gameObject.transform.position, posGoal.position) <= 1f)
            //{
            //    Debug.Log("1");
            //    currentWayPoint = wayPoint1;
            //}

            var wayPointDir = posRight.transform.position - transform.position; //gameObject.transform.position
            wayPointDir.Normalize();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); //pos1������ ����������
            transform.position += wayPointDir * enemySpeed * Time.deltaTime; //pos1���������̵��Ѵ�\


            //Vector3 rightDir = posLeft.transform.position - transform.position;
            //rightDir.Normalize();
            //������������ �̵�
            //transform.position += rightDir * enemySpeed * Time.deltaTime;
            //�����������Ѵٸ�
            //������������ �̵��Ѵ�
        }
    }
}
