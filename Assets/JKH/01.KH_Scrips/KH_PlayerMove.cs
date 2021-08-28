using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_PlayerMove : MonoBehaviour
{
    public GameObject milkPos;
    public float speed = 10.0f;
    public float speedUP=20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir= dirH + dirV;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;

        Vector3 dis = transform.position - milkPos.transform.position;
        float distance = dis.magnitude;
        if (distance < speedUP)
        {
            transform.position += dir * speed* 2 * Time.deltaTime;
        }

    }
}
