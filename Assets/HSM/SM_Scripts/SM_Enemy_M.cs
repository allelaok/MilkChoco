using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_Enemy_M : MonoBehaviour
{
    public GameObject enemy;
    public GameObject respawnPoint;
    SM_Enemy_Hp hpScript;

    float currTime;
    float respawnTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        hpScript = GetComponentInChildren<SM_Enemy_Hp>();
        enemy.transform.position = respawnPoint.transform.position;
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
                enemy.transform.position = respawnPoint.transform.position;
                enemy.SetActive(true);
                hpScript.isDie = false;
                currTime = 0;
            }
        }
    }
}
