using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_AttackModeEnemy : MonoBehaviour
{
    public Transform posLeft;
    public Transform posRight;
    public Transform posMid;
    public Transform posMidL;
    public Transform posMidR;
    public Transform posGoal;
    public float enemySpeed = 3;
    public float rotSpeed = 3;
    int randStart1;
    int randStartMid1;
    // Start is called before the first frame update
    void Start()
    {
        
        randStart1 = Random.Range(2, 3);
        randStartMid1 = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {        

        if (randStart1 == 0)
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

        if(randStart1 == 1)
        {
            print("오");
            var distance2 = Vector3.Distance(gameObject.transform.position, posRight.position);
            
            if (Vector3.Distance(gameObject.transform.position, posRight.position) <= 1f)
            {
                
                posRight = posGoal;
            }


            var wayPointDir = posRight.transform.position - transform.position; //gameObject.transform.position
            wayPointDir.Normalize();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); //pos1쪽으로 몸을돌린다
            transform.position += wayPointDir * enemySpeed * Time.deltaTime; //pos1방향으로이동한다\

        }

        if (randStart1 == 2) 
        {
            print("중간");
            var distance2 = Vector3.Distance(gameObject.transform.position, posMid.position);

            if (Vector3.Distance(gameObject.transform.position, posMid.position) <= 1f)
            {
                if (randStartMid1 == 0)
                {
                    posMid = posMidL;
                    if (Vector3.Distance(gameObject.transform.position, posMid.position) <= 1f)
                    {
                        posMid= posGoal;
                    }
                }
                if (randStartMid1 == 1)
                {
                    posMid = posMidR;
                    if (Vector3.Distance(gameObject.transform.position, posMid.position) <= 1f)
                    {
                        posMid = posGoal;
                    }
                }
                //posMid = posGoal;
            }

            var wayPointDir = posMid.transform.position - transform.position; 
            wayPointDir.Normalize();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(wayPointDir), rotSpeed * Time.deltaTime); 
            transform.position += wayPointDir * enemySpeed * Time.deltaTime; 
        }
    }
}
