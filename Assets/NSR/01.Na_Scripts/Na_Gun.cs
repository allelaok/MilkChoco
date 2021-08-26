using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Gun : MonoBehaviour
{
    // ÃÑ½î±â ÇÊ¿ä ¼Ó¼º
    public Transform aimingPoint;

    public float firePower = 10f;
    public float fireTime = 0.1f;
    float currTime;
    public float crossroad = 30;

    public GameObject LineF;

    // ÀçÀåÀü ÇÊ¿ä ¼Ó¼º
    public int maxFire = 20;
    int fireCount;
    public float reloadTime = 3;
    float reloadCurrTime;

    

    void Start()
    {
        currTime = fireTime;
        fireCount = maxFire;
    }

    void Update()
    {
        if (fireCount > 0)
        {
            Fire();
        }         
        else
        {
            Reload();
        }
    }

    // ÃÑ½î±â
    void Fire()
    {
        Ray ray = new Ray();
        ray.origin = aimingPoint.transform.position;
        ray.direction = aimingPoint.transform.forward;

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, crossroad))
        {
            LineRenderer lr = null;


            if (hitInfo.transform.gameObject.tag == "Enemy")
            {

                currTime += Time.deltaTime;
                if (currTime > fireTime)
                {
                    GameObject line = Instantiate(LineF);
                    lr = line.GetComponent<LineRenderer>();
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, hitInfo.point);
                    Destroy(line, 0.1f);

                    AudioSource audio = GetComponent<AudioSource>();
                    audio.Play();

                    hitInfo.transform.gameObject.GetComponent<Na_Enemy_hp>().Damaged(firePower);

                    fireCount--;
                   
                    currTime = 0;
                }

            }

            else
            {
                currTime = fireTime;
            }

            if (lr != null)
                lr.SetPosition(1, hitInfo.point);
        }
    }
    
    //ÀçÀåÀü
    void Reload()
    {
        reloadCurrTime += Time.deltaTime;
        if (reloadCurrTime > reloadTime)
        {
            fireCount = maxFire;
            currTime = fireTime;
            reloadCurrTime = 0;
        }
    }

    void Rebound()
    {

    }


}
