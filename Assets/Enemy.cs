using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //����
    Vector3 dir;

    //�ӵ�
    public float speed;
    void Start()
    {
        //������ �� (0~9) �� ���´�
        
    }

    void Update()
    {

        Vector3 dirF = Vector3.forward;
        Vector3 dirR = Vector3.right;
        Vector3 dir = dirF + dirR;


        
            //�÷��̾ ã��
            GameObject player = GameObject.Find("Player");
            //�÷�� ���ϴ� ������ ���Ѵ�. P - E
            dir = player.transform.position - transform.position;
            //������ ũ�⸦ 1�� �Ѵ�.
            dir.Normalize();
        






        transform.position += dir * speed * Time.deltaTime;
    }
}