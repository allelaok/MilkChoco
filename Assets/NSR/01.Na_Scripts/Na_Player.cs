using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// idle ������ �� ī�޶��� ��ġ�� AimingPoint �� �ϰ� �ʹ�.
// �÷��̾� W, S, A, D �� �̵��ϰ� �ʹ�.
// �÷��̾� �����̽��ٷ� �����ϰ� �ʹ�.
// 1�� ������ �ϰ�ʹ�.
// Damage �� ������ hp�� ��� �ʹ�. UI �� ǥ���ϰ� �ʹ�.
// hp�� 0���ϸ� ���̰� �ʹ�.
// ������ �԰� milkContainer �� �ְ� �ʹ�.
// ������� �ε����� ���� �����ϰ�ʹ�.
// ���ʹ̿� ������ ���߸� �����ϰ�ʹ�.
// ������ 10�ʸ� ���� �ʹ�.

public class Na_Player : MonoBehaviour
{
    public static Na_Player instace;

    private void Awake()
    {
        if(instace == null)
        {
            instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �ʿ�Ӽ� : AimingPoint
    public Transform aimingPoint;

    // �ʿ�Ӽ� : �ӵ�, CharacterController
    public float speed = 7f;
    CharacterController cc;

    // �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����
    public float jumpPower = 3f;
    float yVelocity;
    public float gravity = 7f;
    public Vector3 dir;

    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
    public int jumpCount;
    public int MaxJumpCount = 1;

    // �ʿ�Ӽ� : ����hp, �ִ�hp, hpUI
    public float currHP;
    public float maxHP = 100;
    public Image hpUI;

    // �ʿ�Ӽ� : ������ġ, ����, milkContainer, ��������, ��������UI
    public Transform milkPos;
    GameObject isMilk;
    public GameObject[] milkContainer;
    int milkCount;
    public Text milkCntUI;

    //�÷��̾ ������ �ϰ�ʹ�.
    //������ �ִٸ� ������
    // �ʿ�Ӽ� : �÷��̾� ó�� ��ġ, ���� ó�� ��ġ, ����ð�, ������ �ð�
    Vector3 startMilkPos;
    Vector3 startPlayerPos;
    float currTime;
    public float respawnTime = 10f;

    public bool isDie;
    GameObject enemyCam;

    bool isJumpZone;
    float jumpZonePower = 8f;

    Animator anim;

    // �ʿ�Ӽ� : ��������Ʈ, ����, ��Ÿ�, �ݵ�, ����, ������, ������, ���ذ�, �Ѿ˰��� ��    
    public Transform myCamera;

    public float firePower = 10f;
    public float fireTime = 0.2f;
    public float crossroad = 30;
    public float reboundPower = 0.2f;
    public float reboundTime = 30f;
    public float weight = 1;

    public GameObject LineF;


    public int maxFire = 20;
    int fireCount;
    public float reloadTime = 3;
    float reloadCurrTime;

    public float scope = 50;

    Text bulletCountUI;

    float reCurrTime;

    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾��� CharacterController �� �����´�
        cc = GetComponent<CharacterController>();        
        // �÷��̾��� ó�� ��ġ ����
        startPlayerPos = transform.position;
        //  ���� hp �� �ִ� hp�� �ʱ�ȭ
        currHP = maxHP;

        anim = GetComponentInChildren<Animator>();

        currTime = fireTime;
        fireCount = maxFire;

        Na_Player_move playerMove = GetComponentInParent<Na_Player_move>();
        Na_Player.instace.speed -= weight;

        bulletCountUI = GameObject.Find("BulletCount").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            Respawn();
            Camera.main.transform.position = enemyCam.transform.position;
            Camera.main.transform.forward = enemyCam.transform.forward;
        }
        else
        {          
            Move();
            Milk();
            Attack();
        }

    }

    private void LateUpdate()
    {
        if (isDie)
        {
            //Camera.main.transform.position = enemyCam.transform.position;
            //Camera.main.transform.forward = enemyCam.transform.forward;
        }
        else
        {
            Camera.main.transform.position = aimingPoint.position;
            Camera.main.transform.forward = aimingPoint.forward;
        }
    }



    // �÷��̾� W, S, A, D �� �̵��ϰ� �ʹ�.
    // �ʿ�Ӽ� : �ӵ�, CharacterController
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;

        dir = dirH + dirV;
        dir.Normalize();

        Jump(out dir.y);

        //if(h + v > 0)
        //{
        //    anim.SetTrigger("Walk");
        //}
       

        cc.Move(dir * speed * Time.deltaTime);
        
    }

    // �÷��̾� �����̽��ٷ� �����ϰ� �ʹ�.
    // �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����

    // 1�� ������ �ϰ�ʹ�.
    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
    public void Jump(out float dirY)
    {
        if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;

        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < MaxJumpCount)
            {
                yVelocity = jumpPower;
                jumpCount++;
            }
        }
        if (isJumpZone)
        {
            yVelocity = jumpZonePower;
            jumpCount++;
            isJumpZone = false;
        }

        dirY = yVelocity;
        yVelocity -= gravity * Time.deltaTime;


    }

    // Damage �� ������ hp�� ��� �ʹ�.
    public void Damaged(float damage, GameObject enemyCamPos)
    {
        currHP -= damage; //HP�����Ѵ�
        hpUI.fillAmount = currHP / maxHP; //HP percentage

        if (currHP <= 0) //currHp�� 0�̶�� 
        {
            enemyCam = enemyCamPos;
            //Camera.main.transform.position = enemyCamPos.transform.position;
            isDie = true;
        }
    }

    // ������ ������ �ϰ� �ʹ�. 
    // ������

    // ���� �ð��� ������ ������
    public void Respawn()
    {
        reCurrTime += Time.deltaTime;

        if (reCurrTime > respawnTime)
        {
           
            transform.position = startPlayerPos;
            //  ���� hp �� �ִ� hp�� �ʱ�ȭ
            currHP = maxHP;            
            enemyCam = null;
            reCurrTime = 0;
            isDie = false;
        }

        if (isMilk != null)
        {
            isMilk.transform.position = startMilkPos;
            isMilk = null;
        }
    }

    void Attack()
    {
        if (fireCount > 0)
        {
            Fire();
            bulletCountUI.text = "�Ѿ˰��� : " + fireCount;
        }
        else
        {
            bulletCountUI.text = "������...";
            Reload();
        }


        Rebound();

        Scope();
    }

    // ������ �԰� milkContainer �� �ְ�ʹ�.
    void Milk()
    {
        milkCntUI.text = milkCount + "/4";
        if (isMilk != null)
            isMilk.transform.position = milkPos.position;

        if (milkCount == 4)
        {
            SceneManager.LoadScene("Na_EndScene");
        }
    }

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
                    GameObject line = Instantiate(LineF);
                    lr = line.GetComponent<LineRenderer>();
                    lr.SetPosition(0, transform.position);
                    lr.SetPosition(1, hitInfo.point);
                    Destroy(line, 0.1f);

                    AudioSource audio = GetComponent<AudioSource>();
                    audio.Play();

                    hitInfo.transform.gameObject.GetComponent<Na_Enemy_hp>().Damaged(firePower);

                    //int rdx = UnityEngine.Random.Range(1, 2);
                    //int rdy = UnityEngine.Random.Range(1, 2);
                    //int rdz = UnityEngine.Random.Range(1, 2);

                    myCamera.Translate(new Vector3(-1, 1, 0) * reboundPower);


                    fireCount--;

                    currTime = 0;
                }


            }
            else
            {
                currTime += Time.deltaTime;
                if (currTime > 0.1f)
                {
                    currTime = fireTime;
                }
            }

            if (lr != null)
                lr.SetPosition(1, hitInfo.point);
        }

    }


    void Rebound()
    {
        myCamera.localPosition = Vector3.Lerp(myCamera.localPosition, new Vector3(0, 6, -15), Time.deltaTime * reboundTime);
    }



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


    void Scope()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView -= scope;
            reboundTime += scope;
            reboundPower += scope * 0.02f;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView += scope;
            reboundTime -= scope;
            reboundPower -= scope * 0.02f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMilk == null)
        {
            if (other.gameObject.tag == "Milk")
            {
                isMilk = other.gameObject;
                startMilkPos = other.gameObject.transform.position;
            }
        }
        else
        {
            if (other.gameObject.name.Contains("MilkContainer"))
            {
                milkContainer[milkCount].SetActive(true);
                milkCount++;
                Destroy(isMilk.gameObject);
                isMilk = null;
            }
        }

        // ������� �ε����� ���� �����ϰ�ʹ�.
        if(other.gameObject.tag == "JumpZone")
        {
            isJumpZone = true;
        }

        if (other.gameObject.name.Contains("FallZone"))
        {
            jumpCount++;
        }     
    } 

}
