using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Gun : MonoBehaviour
{
    // �ѽ��� �ʿ� �Ӽ�
    public Transform aimingPoint;   // ���� �߻��Ǵ� ��ġ -> ī�޶�

    public float firePower = 10f;   // ���� ����
    public float fireTime = 0.1f;   // ���� �ӵ�
    float currTime;
    public float crossroad = 30;    // ���Ÿ�
    public float reboundPower = 0.2f;   // �ݵ� ����
    float startReboundPower;
    Vector3 aimPos;
    Vector3 startAimPos;
    public float weight = 1;    // �� ����

    public GameObject LineF;    // �Ѿ� ����

    // ������ �ʿ� �Ӽ�
    public int maxFire = 20;    // źâ ũ��
    int fireCount;  // �Ѿ� ����
    public float reloadTime = 3;    // ������ �ð�
    float reloadCurrTime;

    public float scope = 30; // ���ذ�



    void Start()
    {
        currTime = fireTime;    // �����ð��� �߻��ð����� �ʱ�ȭ -> ���ʹ̰� ���̿� �������� �ٷ� ���� ����
        fireCount = maxFire;    // �Ѿ˰��� źâũ���� �ʱ�ȭ

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

        Rebound(); // �ݵ����� ���� ���� ��ġ��


        Scope();    // ���ذ�

    }

    // �ѽ���
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
                    GameObject line = Instantiate(LineF);   // �Ѿ˱���
                    lr = line.GetComponent<LineRenderer>();
                    lr.SetPosition(0, transform.position);  // ������ ������
                    lr.SetPosition(1, hitInfo.point);   // ������ ����
                    Destroy(line, 0.1f);

                    AudioSource audio = GetComponent<AudioSource>();  // �ѼҸ�
                    audio.Play();

                    hitInfo.transform.gameObject.GetComponent<Na_Enemy_hp>().Damaged(firePower); // �� hp ����

                    aimingPoint.Translate(new Vector3(-1,1,0) * reboundPower); // ī�޶� �ݵ�


                    fireCount--; // źâ�� �Ѿ� ����

                    currTime = 0;
                }

            }

            else
            {
                currTime = fireTime;
            }

            if (lr != null)
                lr.SetPosition(1, hitInfo.point);   // �Ѿ� ���� ���� ���� ����
        }
    }

    // �ݵ� �� ���ڸ���
    void Rebound()
    {
        aimingPoint.localPosition = Vector3.Lerp(aimingPoint.localPosition, aimPos, Time.deltaTime * 10.0f); //������ ��ġ�� Lerp�� õõ�� �ǵ�����.
    }


    // ������
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

    // ���ذ�
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
