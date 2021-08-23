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
    //CharacterController cc; //�̵��ϴ°� �Ⱦ� ����.��
    // Start is called before the first frame update
    void Start()
    {
        //cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.transform.position - transform.position; //���ʹ̰� �ٶ󺸴¹���
        dir.Normalize(); //����ȭ
        //transform.LookAt(target.transform); //���ʹ̰� Ÿ���� �ٶ󺻴�
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        
        currTime += Time.deltaTime;
        
        if (gunDuration <= currTime) //�����ð��� �Ѵ´�
        {
            GameObject bullet = Instantiate(bulletFactory); //�Ѿ��� �����Ѵ�
            //transform.forward = dir;
            //dir = firePos.transform.forward;
            bullet.transform.position = firePos.transform.position; //firePos�� ���� �����Ѵ�
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
