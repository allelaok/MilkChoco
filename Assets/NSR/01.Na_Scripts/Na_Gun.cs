using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Gun : MonoBehaviour
{
    public Transform aimingPoint;

    Vector3 origin;
    Vector3 direction;

    public float firePower = 50f;
    float currTime;
    public float fireTime = 0.5f;

    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {

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
            if (hitInfo.transform.gameObject.tag == "Enemy")
            {
                currTime += Time.deltaTime;
                if(currTime > fireTime)
                {
                    print(hitInfo.transform.gameObject.name);
                    AudioSource audio = GetComponent<AudioSource>();
                    audio.Play();
                    currTime = 0;
                }
                
            }
        }

    }
}
