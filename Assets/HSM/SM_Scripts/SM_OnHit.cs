using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_OnHit : MonoBehaviour
{

    public GameObject LineRay;
    LineRenderer lr = null;
    public GameObject firePos;
    public SM_KH_EnemyFire enemy;
    public void OnHit() {
        //GameObject line = Instantiate(LineRay);
        //lr = line.GetComponent<LineRenderer>();
        //lr.SetPosition(0, firePos.transform.position);
        //lr.SetPosition(1, enemy.hitInfo.point);

        //Destroy(line, 0.1f);
        enemy.Attack();
       
    }
}
