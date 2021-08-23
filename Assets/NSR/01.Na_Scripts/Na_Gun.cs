using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Gun : MonoBehaviour
{
    public Transform aimingPoint;   //발사되는 지점

    public float firePower = 10f;   //총의 세기
    public float fireTime = 0.1f;   // 연사속도
    float currTime;

    public GameObject LineF;        //총알이 발사되는 라인

    void Start()
    {
        currTime = fireTime;        // 현재시간을 발사되는 시간으로 설정

    }

    void Update()
    {

        Ray ray = new Ray();    //레이 생성
        ray.origin = aimingPoint.transform.position;    //레이가 나오는 위치 aimingPoint로 지정
        ray.direction = aimingPoint.transform.forward;  //레이의 방향을 aimingPoint의 앞방향으로 설정

        RaycastHit hitInfo; //레이가 닿은 곳의 정보를 담을 변수

        //만약 레이가 발사되면
        if (Physics.Raycast(ray, out hitInfo)) 
        {
            LineRenderer lr = null;

            //hitInfo의 위치에 있는 게임오브젝트의 태그가 Enemy라면
            if (hitInfo.transform.gameObject.tag == "Enemy")
            {
                //fireTime후에
                currTime += Time.deltaTime;
                if(currTime > fireTime)  
                {                    
                    GameObject line = Instantiate(LineF);   //Line을 생성하고 
                    lr = line.GetComponent<LineRenderer>(); //Line에 있는 LineRenderer컴포넌트를 가져와 변수 lr에 저장
                    lr.SetPosition(0, transform.position);  //lr의 시작점을 이 오브젝트(Gun)의 위치로하고
                    lr.SetPosition(1, hitInfo.point);       //lr의 끝점을 레이가 닿은 곳을 위치로함
                    Destroy(line, 0.1f);                    //0.1초 후에 라인 파괴

                    AudioSource audio = GetComponent<AudioSource>(); //오디오 소스를 가져와
                    audio.Play();                                    //플레이

                    hitInfo.transform.gameObject.GetComponent<Na_Enemy_hp>().Damaged(firePower);
                  

                    currTime = 0;      //현재시간은 0으로 
                }

            }
            // hitInfo의 위치에 있는 게임오브젝트의 태그가 Enemy가 아니라면
            else
            {
                currTime = fireTime;    //현재시간을 발사시간으로 설정
            }

            if(lr != null)
            lr.SetPosition(1, hitInfo.point);   //hitInfo의 위치에 있는 게임오브젝트에 상관 없이 항상 lr의 끝점을 레이가 닿은 곳으로 함 
        }

    }

   
}
