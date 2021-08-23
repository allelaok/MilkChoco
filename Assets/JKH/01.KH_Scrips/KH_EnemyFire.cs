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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        
        currTime += Time.deltaTime;
        
        if (gunDuration <= currTime) //일정시간을 넘는다
        {
            GameObject bullet = Instantiate(bulletFactory); //총알을 생성한다
            //transform.forward = dir;
            //dir = firePos.transform.forward;
            bullet.transform.position = firePos.transform.position; //firePos에 무기 장착한다
            bullet.transform.forward = firePos.transform.forward;


            Destroy(bullet, 3);
            currTime = 0;
        }
        
    }

    void look()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        transform.LookAt(target.transform);
        //transform.rotation = Quaternion.Lerp(transform.rotation,
        //    Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
    }
}
