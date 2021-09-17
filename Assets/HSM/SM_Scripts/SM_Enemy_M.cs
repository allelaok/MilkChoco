using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Enemy_M : MonoBehaviour
{
    public GameObject enemy;
    public GameObject respawnPoints;
    SM_Enemy_Hp hpScript;

    float currTime;
    float respawnTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        hpScript = GetComponentInChildren<SM_Enemy_Hp>();
        enemy.transform.position = respawnPoints.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(hpScript.isDie)
        {
            enemy.SetActive(false);
            currTime += Time.deltaTime;
            if(currTime > respawnTime)
            {
                enemy.transform.position = respawnPoints.transform.position;
                enemy.SetActive(true);
                hpScript.isDie = false;
                currTime = 0;
            }
        }
    }
}
