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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime); //����������        
        currTime += Time.deltaTime; //�ð����帣�µ�
        
        if (gunDuration <= currTime) //�Ѿ˹߻�ӵ�
        {
            GameObject bullet = Instantiate(bulletFactory); //�Ѿ��� �����Ѵ�
            //transform.forward = dir;
            //dir = firePos.transform.forward;
            bullet.transform.position = firePos.transform.position; //firePos�� ���� �����Ѵ�
            bullet.transform.forward = firePos.transform.forward; //firePos �����̼ǵ� �ٲ��ش�


            Destroy(bullet, 3); //�����ð� ������ �ı��Ѵ�
            currTime = 0; //����ð� �ʱ�ȭ
        }
        
    }

    void justMemo()
    {
        //Ray?? ���߻� 
        //���ʹ̰� ���������� �����Ѵ� (�������)
        //-�ش�������� �����δ�
        //-�ش���⿡ �����Ѵٸ�
        //-�ݴ�������� �����δ� 
        //-�ݴ���⿡ �����ϸ� �� �ٽ� �ݺ�
        //���� ���������ȿ� ������ �ν��Ѵٸ�(�Ÿ���? ray��? ������Ϲ¤Ф�) 
        //������ �����
        //���� �ѱ��� ���������� ���´�
        //���̰� ���濡�� �����ٸ�? (�� �ȶո��� �ؾ���)
        //(��¦��¦ �����̸鼭) ���� �߻��Ѵ� (�����ڵ�)
        //���� �������� �ۿ� �ִٸ� 
        //������(�������)�� �ٲ۴�

        //-------------------------------------------
        //�׳� ���̷� �ν��ϰ� �� ���� ������? ��

    }
}
