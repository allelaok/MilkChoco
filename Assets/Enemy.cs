using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //방향
    Vector3 dir;

    //속도
    public float speed;
    void Start()
    {
        //랜덤한 값 (0~9) 을 얻어온다
        
    }

    void Update()
    {

        Vector3 dirF = Vector3.forward;
        Vector3 dirR = Vector3.right;
        Vector3 dir = dirF + dirR;


        
            //플레이어를 찾자
            GameObject player = GameObject.Find("Player");
            //플레어를 향하는 방향을 구한다. P - E
            dir = player.transform.position - transform.position;
            //방향의 크기를 1로 한다.
            dir.Normalize();
        






        transform.position += dir * speed * Time.deltaTime;
    }
}