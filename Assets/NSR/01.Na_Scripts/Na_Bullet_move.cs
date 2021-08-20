using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Bullet_move : MonoBehaviour
{
    public float speed = 10f;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        dir = transform.forward;
        transform.position += dir * speed * Time.deltaTime;
    }
}
