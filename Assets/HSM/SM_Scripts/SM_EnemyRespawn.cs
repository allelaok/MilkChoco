using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_EnemyRespawn : MonoBehaviour
{
    //public static SM_EnemyRespawn instance;

    //public GameObject respawnPoint;
    public GameObject enemy;
    float reSpawnTime = 3;
    float currenTime=0;

    //private void Awake() ½Ì±ÛÅÏ »èÁ¦
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //}


    // Start is called before the first frame update
    void Start()
    {
        enemy.SetActive(true);
        //enemy.transform.position = respawnPoint.transform.position;
        isDie = false;
        currenTime = 0;
        isDie = false;
    }
    public  bool isLive;
    public  bool isDie;

    // Update is called once per frame
    void Update()
    {
        if (isDie == true)
        {
            
            enemy.SetActive(false);
            currenTime += Time.deltaTime;
            print(currenTime);
            if (currenTime > reSpawnTime)
            {
                enemy.SetActive(true);
                //enemy.transform.position = transform.position;
                isDie = false;
                currenTime = 0;
                

            }
        }
    }
}
