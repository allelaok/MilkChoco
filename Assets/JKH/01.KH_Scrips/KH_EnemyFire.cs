using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_EnemyFire : MonoBehaviour
{
    float currTime;
    float gunDuration = .2f;
    public GameObject bulletFactory;
    public GameObject firePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (gunDuration <= currTime)
        {
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = firePos.transform.position; //이거 방향 바꾸어야함
            Destroy(bullet, 3);
            currTime = 0;
        }
    }
}
