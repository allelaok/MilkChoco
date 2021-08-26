using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
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


    }
}
