using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_Bullet : MonoBehaviour
{
    public float speed = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //Destroy(gameObject, 1.5f);
    }
}
