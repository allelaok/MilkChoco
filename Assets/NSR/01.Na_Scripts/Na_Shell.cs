using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Shell : MonoBehaviour
{
    Rigidbody rb;
    GameObject shellDir;
    //GameObject weaponPos;
    // Start is called before the first frame update
    void Start()
    {
        //weaponPos = GameObject.Find("WeaponPos");
        //transform.position = weaponPos.transform.position;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        shellDir = GameObject.Find("ShellDir");
        Vector3 dir = shellDir.transform.position - transform.position;
        rb.AddForce(dir * 5, ForceMode.Impulse);
        rb.AddTorque(Vector3.back * 10, ForceMode.Impulse);
    }

}
