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
            print("왼");
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

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); //pos1쪽으로 몸을돌린다
            transform.position += wayPointDir * enemySpeed * Time.deltaTime; //pos1방향으로이동한다\
            //Vector3 leftDir = posLeft.transform.position - transform.position;
            //leftDir.Normalize();
            //왼쪽방향으로 이동
            //transform.position += leftDir * enemySpeed * Time.deltaTime;
            //지점에 도달한다면
            //도착지점으로 이동한다

        }

        if(rand==1)
        {
            print("오");
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

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); //pos1쪽으로 몸을돌린다
            transform.position += wayPointDir * enemySpeed * Time.deltaTime; //pos1방향으로이동한다\


            //Vector3 rightDir = posLeft.transform.position - transform.position;
            //rightDir.Normalize();
            //우측방향으로 이동
            //transform.position += rightDir * enemySpeed * Time.deltaTime;
            //지점에도달한다면
            //도착지점으로 이동한다
        }
    }
}
