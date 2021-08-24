using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_EnemyFire : MonoBehaviour
{
    float currTime;
    float gunDuration = .2f;
    public GameObject bulletFactory;
    public GameObject firePos;
    public GameObject target;
    public float rotSpeed = 5;
    //CharacterController cc; //이동하는거 안씀 아직.ㅎ
    // Start is called before the first frame update
    void Start()
    {
        //cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.transform.position - transform.position; //에너미가 바라보는방향
        dir.Normalize(); //정규화
        //transform.LookAt(target.transform); //에너미가 타겟을 바라본다
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime); //몸을돌린다        
        currTime += Time.deltaTime; //시간이흐르는데
        
        if (gunDuration <= currTime) //총알발사속도
        {
            GameObject bullet = Instantiate(bulletFactory); //총알을 생성한다
            //transform.forward = dir;
            //dir = firePos.transform.forward;
            bullet.transform.position = firePos.transform.position; //firePos에 무기 장착한다
            bullet.transform.forward = firePos.transform.forward; //firePos 로테이션도 바꿔준다


            Destroy(bullet, 3); //일정시간 지나면 파괴한다
            currTime = 0; //현재시간 초기화
        }
        
    }

    void justMemo()
    {
        //Ray?? 를발사 
        //에너미가 일정구간을 정찰한다 (정찰모드)
        //-해당방향으로 움직인다
        //-해당방향에 도착한다면
        //-반대방향으로 움직인다 
        //-반대방향에 도착하면 또 다시 반복
        //만약 일정범위안에 적군을 인식한다면(거리로? ray로? 벽어떻게하뮤ㅠㅠ) 
        //정찰을 멈춘다
        //몸과 총구를 상대방쪽으로 꺾는다
        //레이가 상대방에게 닿은다면? (벽 안뚫리게 해야함)
        //(살짝살짝 움직이면서) 총을 발사한다 (위에코드)
        //만약 일정범위 밖에 있다면 
        //원상태(정찰모드)로 바꾼다

        //-------------------------------------------
        //그냥 레이로 인식하게 할 수는 없을까? ㅠ

    }
}
