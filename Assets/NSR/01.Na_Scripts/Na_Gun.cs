using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Gun : MonoBehaviour
{
    public Transform aimingPoint;   //�߻�Ǵ� ����

    public float firePower = 10f;   //���� ����
    public float fireTime = 0.1f;   // ����ӵ�
    float currTime;

    public GameObject LineF;        //�Ѿ��� �߻�Ǵ� ����

    void Start()
    {
        currTime = fireTime;        // ����ð��� �߻�Ǵ� �ð����� ����

    }

    void Update()
    {

        Ray ray = new Ray();    //���� ����
        ray.origin = aimingPoint.transform.position;    //���̰� ������ ��ġ aimingPoint�� ����
        ray.direction = aimingPoint.transform.forward;  //������ ������ aimingPoint�� �չ������� ����

        RaycastHit hitInfo; //���̰� ���� ���� ������ ���� ����

        //���� ���̰� �߻�Ǹ�
        if (Physics.Raycast(ray, out hitInfo)) 
        {
            LineRenderer lr = null;

            //hitInfo�� ��ġ�� �ִ� ���ӿ�����Ʈ�� �±װ� Enemy���
            if (hitInfo.transform.gameObject.tag == "Enemy")
            {
                //fireTime�Ŀ�
                currTime += Time.deltaTime;
                if(currTime > fireTime)  
                {                    
                    GameObject line = Instantiate(LineF);   //Line�� �����ϰ� 
                    lr = line.GetComponent<LineRenderer>(); //Line�� �ִ� LineRenderer������Ʈ�� ������ ���� lr�� ����
                    lr.SetPosition(0, transform.position);  //lr�� �������� �� ������Ʈ(Gun)�� ��ġ���ϰ�
                    lr.SetPosition(1, hitInfo.point);       //lr�� ������ ���̰� ���� ���� ��ġ����
                    Destroy(line, 0.1f);                    //0.1�� �Ŀ� ���� �ı�

                    AudioSource audio = GetComponent<AudioSource>(); //����� �ҽ��� ������
                    audio.Play();                                    //�÷���

                    hitInfo.transform.gameObject.GetComponent<Na_Enemy_hp>().Damaged(firePower);
                  

                    currTime = 0;      //����ð��� 0���� 
                }

            }
            // hitInfo�� ��ġ�� �ִ� ���ӿ�����Ʈ�� �±װ� Enemy�� �ƴ϶��
            else
            {
                currTime = fireTime;    //����ð��� �߻�ð����� ����
            }

            if(lr != null)
            lr.SetPosition(1, hitInfo.point);   //hitInfo�� ��ġ�� �ִ� ���ӿ�����Ʈ�� ��� ���� �׻� lr�� ������ ���̰� ���� ������ �� 
        }

    }

   
}
