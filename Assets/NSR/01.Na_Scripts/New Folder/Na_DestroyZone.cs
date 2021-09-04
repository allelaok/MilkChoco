using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {           
            Na_Player.instace.isDie = true;            
        }    
    }
}
