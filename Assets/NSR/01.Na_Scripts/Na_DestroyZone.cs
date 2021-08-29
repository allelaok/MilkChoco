using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_DestroyZone : MonoBehaviour
{
    public static Na_DestroyZone instance;

    public GameObject player;

    float currTime = 0;
    float respawnTime = 3f;

    public bool playerDie;

    public Transform playerPos;

    //public GameObject gun;

    //public GameObject enemyCamera;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //enemyCamera.SetActive(false);
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDie)
        {
            currTime += Time.deltaTime;
            if(currTime > respawnTime - 1)
            {
                //enemyCamera.SetActive(false);
                player.transform.position = playerPos.position;
                
                //gun.SetActive(true);
                
                
            }
            if (currTime > respawnTime)
            {
                player.SetActive(true);
                playerDie = false;
                currTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //enemyCamera.SetActive(true);
        player.SetActive(false);


        //gun.SetActive(false);
        playerDie = true;

    }
}
