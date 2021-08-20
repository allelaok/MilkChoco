using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_PlayerFire : MonoBehaviour
{
    public GameObject firePos;
    public GameObject firePos1;
    public GameObject firePos2;
    public GameObject firePos3;
    public GameObject firePos4;
    public GameObject bulletFactory;
    public float gunSpeed = 0.2f; //일반총 연사속도
    public float shotGunSpeed = 1f; //샷건 연사속도
    public float currTime = 0; //발사시간
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        ShotGun();
    }

    void NormalGun()
    {
        if (Input.GetButton("Fire1"))
        {
            currTime += Time.deltaTime;
            if (gunSpeed <= currTime)
            {
                GameObject bullet = Instantiate(bulletFactory);
                bullet.transform.position = firePos.transform.position;
                Destroy(bullet, 3);
                currTime = 0;
            }

        }
    }
    void ShotGun()
    {
        if (Input.GetButton("Fire1"))
        {
            currTime += Time.deltaTime;
            if (shotGunSpeed<= currTime)
            {
                GameObject bullet1 = Instantiate(bulletFactory);
                GameObject bullet2 = Instantiate(bulletFactory);
                GameObject bullet3 = Instantiate(bulletFactory);
                GameObject bullet4 = Instantiate(bulletFactory);
                GameObject bullet5 = Instantiate(bulletFactory);
                bullet1.transform.position = firePos.transform.position;
                bullet2.transform.position = firePos1.transform.position;
                bullet3.transform.position = firePos2.transform.position;
                bullet4.transform.position = firePos3.transform.position;
                bullet5.transform.position = firePos4.transform.position;
                bullet1.transform.rotation = firePos.transform.rotation;
                bullet2.transform.rotation = firePos1.transform.rotation;
                bullet3.transform.rotation = firePos2.transform.rotation;
                bullet4.transform.rotation = firePos3.transform.rotation;
                bullet5.transform.rotation = firePos4.transform.rotation;
                currTime = 0;
                Destroy(bullet1, 3);
                Destroy(bullet2, 3);
                Destroy(bullet3, 3);
                Destroy(bullet4, 3);
                Destroy(bullet5, 3);

                //반복문이 왜 생각이 안날까 ^_^
                //for (int i=0; i<5; i++)
                //{
                //    GameObject 
                //}


            }

        }
    }
}
