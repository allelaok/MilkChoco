using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_DestroyZone : MonoBehaviour
{
    bool isFallen;
    Vector3 camPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFallen)
        {
            Camera.main.transform.position = camPos;
            Na_Player.instace.Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isFallen = true;
            camPos = Camera.main.transform.position;
        }    
    }
}
