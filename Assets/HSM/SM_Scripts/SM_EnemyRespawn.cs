using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_EnemyRespawn : MonoBehaviour
{
    public static SM_EnemyRespawn instance;

    public GameObject respawnPoint;
    public GameObject enemy;
    float reSpawnTime = 3;
    float currenTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject enemy = Instantiate(respawnPoint);
        enemy.transform.position = transform.position;
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
            currenTime += Time.deltaTime;
            if (currenTime > reSpawnTime)
            {

                GameObject enemy = Instantiate(respawnPoint);
                enemy.transform.position = transform.position;
                isDie = false;
                currenTime = 0;

            }
        }
    }
}
