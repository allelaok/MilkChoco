using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Grenade : MonoBehaviour
{
    Rigidbody rb;
    GameObject ap;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        ap = GameObject.Find("AimingPoint");
        player = GameObject.Find("Na_Player");
        //Vector3 dir = player.transform.position - ap.transform.forward;
        Vector3 dir = transform.forward;
        dir.y = 0.5f;
        rb.AddForce(dir * 15, ForceMode.Impulse);
        rb.AddTorque(Vector3.back * 10, ForceMode.Impulse);
    }

}
