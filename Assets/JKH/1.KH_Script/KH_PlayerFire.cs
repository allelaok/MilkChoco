using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_PlayerFire : MonoBehaviour
{
    public GameObject firePos1;
    public GameObject firePos2;
    public GameObject firePos3;
    public GameObject firePos4;
    public GameObject firePos5;
    public GameObject bulletFactory;
    public float gunDuration = 0.2f; //일반총 연사속도
    public float shotGunDuration = 1f; //샷건 연사속도
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
            if (gunDuration <= currTime)
            {
                GameObject bullet = Instantiate(bulletFactory);
                bullet.transform.position = firePos1.transform.position; //이거 방향 바꾸어야함
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
            if (shotGunDuration <= currTime)
            {
                var bList = new List<GameObject>();
                bList.Add(Instantiate(bulletFactory));
                bList.Add(Instantiate(bulletFactory));
                bList.Add(Instantiate(bulletFactory));
                bList.Add(Instantiate(bulletFactory));
                bList.Add(Instantiate(bulletFactory));

                var pos = new List<Vector3>();
                pos.Add(firePos1.transform.position);
                pos.Add(firePos2.transform.position);
                pos.Add(firePos3.transform.position);
                pos.Add(firePos4.transform.position);
                pos.Add(firePos5.transform.position);

                var rot = new List<Quaternion>();
                rot.Add(firePos1.transform.rotation);
                rot.Add(firePos2.transform.rotation);
                rot.Add(firePos3.transform.rotation);
                rot.Add(firePos4.transform.rotation);
                rot.Add(firePos5.transform.rotation);
                currTime = 0;


                //반복문이 왜 생각이 안날까 ^_ ^
                for (int i = 0; i < 5; i++)
                {
                    var b = bList[i];
                    b.transform.position = pos[i];
                    b.transform.rotation = rot[i];
                    Destroy(b, 3f);
                }


            }

        }
    }
}
