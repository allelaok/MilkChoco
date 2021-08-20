using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Na_AP_fire : MonoBehaviour
{
    public GameObject bulletF;
    public GameObject aimingPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletF);
            bullet.transform.forward = aimingPoint.transform.forward;
            bullet.transform.position = aimingPoint.transform.position;
        }
        
    }
}
