using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SM_EnemyManager_New : MonoBehaviour
{
    public int enemyCount;

    public static SM_EnemyManager_New instance;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject[] enemyPoint;
    public int[] numbers = { 0, 1, 2, 3 };
    public int num1;
    public int num2;
    public int i=0;

    float currTime;
    float A_currTime;
    float respawnTime = 4;
    float ATime;

    // Start is called before the first frame update
    public void Start()
    {
        
        

        enemyPoint[numbers[i]].SetActive(true);
        //i++;
    }

   

    public bool isEnemyA;
    public bool isDie;
    // Update is called once per frame
    void Update()
    {
        if (isEnemyA)
        {
            enemyPoint[numbers[i]].SetActive(false);
            i++;
            enemyPoint[numbers[i]].SetActive(true);
            isEnemyA = false;


        }

        if (isDie)
        {
            
            enemyPoint[numbers[i]].SetActive(false);
            currTime += Time.deltaTime;
            print("현재시간: " + currTime);
            if (currTime > respawnTime)
            {
                enemyPoint[numbers[i]].SetActive(true);
                isDie = false;
                currTime = 0;

            }
        }
    }
}
