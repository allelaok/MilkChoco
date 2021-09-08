using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public GameObject camPos;

    float currTime;
    float time = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime > time)
        {
            Na_Player.instace.Damaged(10);
            currTime = 0;
        }
    }
}
