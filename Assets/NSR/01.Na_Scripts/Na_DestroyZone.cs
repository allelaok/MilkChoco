using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_DestroyZone : MonoBehaviour
{
    public bool playerDie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDie)
        {         
            Na_Player.instace.Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerDie = true;
        }    
    }
}
