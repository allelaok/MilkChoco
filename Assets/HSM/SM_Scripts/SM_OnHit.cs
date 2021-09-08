using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_OnHit : MonoBehaviour
{

    public GameObject LineRay;
    
    public GameObject firePos;
    public SM_KH_EnemyFire enemy;
    public void OnHit()
    
    {
          enemy.Attack();
    }
}
