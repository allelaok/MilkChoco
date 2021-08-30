using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Na_Gun : MonoBehaviour
{
    
    public Transform aimingPoint;
    public Transform myCamera;

    public float firePower = 10f;   
    public float fireTime = 0.2f;   
    float currTime;
    public float crossroad = 30;    
    public float reboundPower = 0.2f;   
    public float reboundTime = 30f;
    public float weight = 1;   

    public GameObject LineF;   


    public int maxFire = 20;    
    int fireCount; 
    public float reloadTime = 3;   
    float reloadCurrTime;

    public float scope = 50;

    Text bulletCountUI;



    void Start()
    {
        currTime = fireTime;   
        fireCount = maxFire;  

        Na_Player_move playerMove = GetComponentInParent<Na_Player_move>();
        playerMove.speed -= weight;

        bulletCountUI = GameObject.Find("BulletCount").GetComponent<Text>();

    }

    void Update()
    {
        if (fireCount > 0)
        {
            Fire();
            bulletCountUI.text = "총알개수 : " + fireCount;
        }
        else
        {
            bulletCountUI.text = "장전중...";
            Reload();
        }


        Rebound();

        Scope();

        
       
    }


    void Fire()
    {
        Ray ray = new Ray();
        ray.origin = aimingPoint.position;
        ray.direction = aimingPoint.forward;

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

                    myCamera.Translate(new Vector3(-1, 1, 0) * reboundPower);


                    fireCount--;

                    currTime = 0;
                }
                

            }
            else
            {
                currTime += Time.deltaTime;
                if(currTime > 0.1f)
                {
                    currTime = fireTime;
                }
            }

            if (lr != null)
                lr.SetPosition(1, hitInfo.point);   
        }
        
    }


    void Rebound()
    {
        myCamera.localPosition = Vector3.Lerp(myCamera.localPosition, new Vector3(0, 6, -15), Time.deltaTime * reboundTime); 
    }



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


    void Scope()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView -= scope;
            reboundTime += scope;
            reboundPower += scope * 0.02f;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView += scope;
            reboundTime -= scope;
            reboundPower -= scope * 0.02f;
        }
    }

}
