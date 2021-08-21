using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Gun : MonoBehaviour
{
    public Transform aimingPoint;

    Vector3 origin;
    Vector3 direction;

    public float firePower = 50f;
    public float fireTime = 0.1f;
    float currTime;

    public GameObject LineF;

    // Start is called before the first frame update
    void Start()
    {
        currTime = fireTime;

    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = new Ray();
        ray.origin = aimingPoint.transform.position;
        ray.direction = aimingPoint.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            LineRenderer lr = null;
            if (hitInfo.transform.gameObject.tag == "Enemy")
            {

                

                currTime += Time.deltaTime;
                if(currTime > fireTime)
                {
                    GameObject line = Instantiate(LineF);
                    lr = line.GetComponent<LineRenderer>();
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, hitInfo.point);
                    Destroy(line, 0.1f);

                    AudioSource audio = GetComponent<AudioSource>();
                    audio.Play();

                    currTime = 0;
                }

            }
            else
            {

                currTime = fireTime;
            }
            if(lr != null)
            lr.SetPosition(1, hitInfo.point);
        }

    }

   
}
