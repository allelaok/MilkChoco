using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_EnemyM : MonoBehaviour
{
    public GameObject EnemyFactory;
    public GameObject AttackEnemyPos;
    public GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        GameObject AttackEnemy = Instantiate(EnemyFactory);
        AttackEnemy.transform.position = AttackEnemyPos.transform.position;
        Destroy(gameObject,10);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
