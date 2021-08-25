using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Gun : MonoBehaviour
{
    // 총쏘기 필요 속성
    public Transform aimingPoint;   // 레이 발사되는 위치 -> 카메라

    public float firePower = 10f;   // 총의 세기
    public float fireTime = 0.1f;   // 연사 속도
    float currTime; 
    public float crossroad = 30;    // 사거리
    public float reboundPower = 0.2f;   // 반동 세기
    float startReboundPower;
    Vector3 aimPos;
    Vector3 startAimPos;
    public float weight = 1;    // 총 무게

    public GameObject LineF;    // 총알 궤적

    // 재장전 필요 속성
    public int maxFire = 20;    // 탄창 크기
    int fireCount;  // 총알 개수
    public float reloadTime = 3;    // 재장전 시간
    float reloadCurrTime;

    public float scope = 30; // 조준경

    

    void Start()
    {
        currTime = fireTime;    // 현재시간을 발사시간으로 초기화 -> 에너미가 레이에 닿았을때 바로 쏘기 위해
        fireCount = maxFire;    // 총알개수 탄창크기로 초기화

        startAimPos = aimingPoint.localPosition;
        aimPos = startAimPos;

        startReboundPower = reboundPower;

        Na_Player_move playerMove = GetComponentInParent<Na_Player_move>();
        playerMove.speed -= weight;
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

        Rebound(); // 반동받은 에임 원래 위치로


        Scope();    // 조준경

    }

    // 총쏘기
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
                    GameObject line = Instantiate(LineF);   // 총알궤적
                    lr = line.GetComponent<LineRenderer>();
                    lr.SetPosition(0, transform.position);  // 라인의 시작점
                    lr.SetPosition(1, hitInfo.point);   // 라인의 끝점
                    Destroy(line, 0.1f);

                    AudioSource audio = GetComponent<AudioSource>();  // 총소리
                    audio.Play();                                       

                    hitInfo.transform.gameObject.GetComponent<Na_Enemy_hp>().Damaged(firePower); // 적 hp 감소

                    aimingPoint.Translate(new Vector3(-1,1,0) * reboundPower); // 카메라 반동
                    

                    fireCount--; // 탄창에 총알 감소
                   
                    currTime = 0;
                }

            }

            else
            {
                currTime = fireTime;
            }

            if (lr != null)
                lr.SetPosition(1, hitInfo.point);   // 총알 궤적 끝점 계속 조정
        }
    }

    // 반동 후 제자리로
    void Rebound()
    {
        aimingPoint.localPosition = Vector3.Lerp(aimingPoint.localPosition, aimPos, Time.deltaTime * 10.0f); //에임의 위치도 Lerp로 천천히 되돌린다.        
    }
    

    // 재장전
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

    // 조준경
    void Scope()
    {
        if (Input.GetMouseButtonDown(1))
        {
            aimingPoint.localPosition = startAimPos + Vector3.forward * scope;
            aimPos = startAimPos + Vector3.forward * scope;
            reboundPower = startReboundPower + scope * 0.01f;
        }

        if (Input.GetMouseButtonUp(1))
        {
            aimingPoint.localPosition = startAimPos;
            aimPos = startAimPos;
            reboundPower = startReboundPower;
        }
    }

}
