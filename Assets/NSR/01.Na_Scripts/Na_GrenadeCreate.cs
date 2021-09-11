using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_GrenadeCreate : MonoBehaviour
{
    public GameObject grenadeF;
    public Transform grenadePos;
    public void Grenade()
    {

        GameObject grenade = Instantiate(grenadeF, grenadePos.transform.position, grenadePos.transform.rotation);
       
    }
}
