using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 플레이어 W, S, A, D 로 이동하고 싶다.
// 플레이어 스페이스바로 1단 점프하고 싶다.
// Damage 를 받으면 hp를 깎고 싶다. UI 도 표현하고 싶다.
// hp가 0이하면 리스폰 하고싶다
// 우유를 먹고 milkContainer 에 넣고 싶다.
// 점프대와 부딪히면 높이 점프하고싶다.
// 에너미에 에임을 맞추면 공격하고싶다.
// 플레이어 애니메이션을 넣어주고 싶다.

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

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어의 CharacterController 를 가져온다
        cc = GetComponent<CharacterController>();        

        //  현재 hp 를 최대 hp로 초기화
        currHP = maxHP;

        anim = GetComponentInChildren<Animator>();

        currTime = fireTime;
        fireCount = maxFire;
     
        speed -= weight;

        bulletCountUI = GameObject.Find("BulletCount").GetComponent<Text>();

        iTween.ColorTo(damage, iTween.Hash(
          "a", 0f,
          "time", 0f

            ));
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            Respawn();
            currTime += Time.deltaTime;
            if(currTime > respawnTime)
            {
                transform.position = startPos.position;
                currTime = 0;
            }

        }
        else
        {
            Move();
            Milk();
            Attack();
        }

    }



    // 플레이어 W, S, A, D 로 이동하고 싶다.
    // 필요속성 : 속도, CharacterController, 방향
    public float speed = 7f;
    CharacterController cc;
    public Vector3 dir;

    
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;

        dir = dirH + dirV;
        dir.Normalize();

        Jump(out dir.y);      
       
        cc.Move(dir * speed * Time.deltaTime);
        
    }

    // 플레이어 스페이스바로 점프하고 싶다.
    // 필요속성 : 점프파워, 중력, y속도
    public float jumpPower = 3f;
    float yVelocity;
    public float gravity = 7f;

    // 1단 점프를 하고싶다.
    // 필요속성 : 점프횟수, 최대 점프 가능 횟수
    public int jumpCount;
    public int MaxJumpCount = 1;

    bool isJumpZone;
    float jumpZonePower = 8f;
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

    // Damage 를 받으면 hp를 깎고 싶다.
    // 필요속성 : 현재hp, 최대hp, hpUI, damage
    public float currHP;
    public float maxHP = 100;
    public Image hpUI;
    public GameObject damage;
    public bool isDie;
    public void Damaged(float damage, GameObject enemyCamPos)
    {
        if (isDie) return;
        currHP -= damage; //HP감소한다
        hpUI.fillAmount = currHP / maxHP; //HP percentage
        ColorA(); // damage 표현

        if (currHP <= 0) //currHp가 0이라면 
        {          
            //Camera.main.transform.position = enemyCamPos.transform.position;
            isDie = true;
        }
    }

    void ColorA()
    {
        iTween.ColorTo(damage, iTween.Hash(
        "a", 1f,
        "time", 0f,

        "oncompletetarget", damage,
        "oncomplete", "ColorBack"
        ));
    }
    void ColorBack()
    {
        iTween.ColorTo(damage, iTween.Hash(
          "a", 0f,
          "time", 0.5f
           ));
    }

    // 죽으면 리스폰 하고 싶다. 
    // 우유도

    // 일정 시간이 지나면 리스폰
    // 필요속성 : 우유위치, 우유
    public Transform milkPos;
    GameObject isMilk;
    //플레이어를 리스폰 하고싶다.
    //우유가 있다면 우유도
    // 필요속성 : 플레이어 처음 위치, 우유 처음 위치, 현재시간, 리스폰 시간
    Vector3 startMilkPos;
    public Transform startPos;
    float currTime;
    public float respawnTime = 3f;

    float reCurrTime;
    public void Respawn()
    {
        reCurrTime += Time.deltaTime;

        if (reCurrTime > respawnTime)
        {

            //transform.position = startPos.position;
            //  현재 hp 를 최대 hp로 초기화
            currHP = maxHP;            
            //enemyCam = null;
            reCurrTime = 0;
            isDie = false;

        }

        if (isMilk != null)
        {
            isMilk.transform.position = startMilkPos;
            isMilk = null;
        }
    }

    public int maxFire = 20;
    int fireCount;
    public float reloadTime = 3;
    float reloadCurrTime;



    Text bulletCountUI;
    void Attack()
    {
        if (fireCount > 0)
        {
            Fire();
            bulletCountUI.text = "총알개수 : " + fireCount;
        }
        else
        {
            bulletCountUI.text = "장전중...";
            Reload();
        }


        Rebound();

        Scope();
    }

    // 우유를 먹고 milkContainer 에 넣고싶다.
    public GameObject[] milkContainer;
    int milkCount;
    public Text milkCntUI;
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

    // 필요속성 : AimingPoint
    public Transform aimingPoint;
    public GameObject LineF;

    // 필요속성 : 에임포인트, 세기, 사거리, 반동, 무게, 슛라인, 재장전, 조준경, 총알개수 등    
    public Transform myCamera;

    public float firePower = 10f;
    public float fireTime = 0.2f;
    public float crossroad = 30;
    public float reboundPower = 0.2f;
    public float reboundTime = 30f;
    public float weight = 1;
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

    public float scope = 50;
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
            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                milkContainer[milkCount].SetActive(true);
                milkCount++;
                Destroy(isMilk.gameObject);
                isMilk = null;
            }
        }

        // 점프대와 부딪히면 높이 점프하고싶다.
        if (other.gameObject.tag == "JumpZone")
        {
            isJumpZone = true;
        }

        if (other.gameObject.name.Contains("FallZone"))
        {
            jumpCount++;
        }

        // DestroyZone 과 부딪히면 죽이고 싶다.
        if (other.gameObject.name.Contains("DestroyZone"))
        {
            isDie = true;
        }
    } 

}
