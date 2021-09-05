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
// 무기를 바꾸고 싶다.
// 캐릭터 idex 를 받아서 모자를 쓰고싶다.

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

        // 플레이어의 CharacterController 를 가져온다
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();
    }

    Animator anim;

    public GameObject[] Hats;

    // Start is called before the first frame update
    void Start()
    {
        //  현재 hp 를 최대 hp로 초기화
        currHP = maxHP;

        fireCurrTime = fireTime - 0.5f;
        fireCount = maxFire;

        speed -= weight;

        bulletCountUI = GameObject.Find("BulletCount").GetComponent<Text>();

        // damage 투명도 으로
        iTween.ColorTo(damage, iTween.Hash("a", 0f, "time", 0f));

        weapons[0].SetActive(true);

        y = transform.localEulerAngles.y;

        Hats[Na_Center.instance.chIdx].SetActive(true);

        line.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            Respawn();
            //transform.position = startPos.position;
            currTime += Time.deltaTime;
            if (currTime > respawnTime - 1)
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
            Swap();
            Rotate();
        }

        lr = line.GetComponent<LineRenderer>();
        lr.SetPosition(0, Gun.transform.position);
        lr.SetPosition(1, hitInfo.point);

    }



    // 플레이어 W, S, A, D 로 이동하고 싶다.
    // 필요속성 : 속도, CharacterController, 방향
    [HideInInspector]
    public float speed = 7f;
    CharacterController cc;
    [HideInInspector]
    public Vector3 dir;   
    void Move()
    {
      
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Dodge(ref v);

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;

        
        dir = dirH + dirV;
        dir.Normalize();

        if (isDodge)
            dir = dodgeVecor;

        Jump(out dir.y);

        anim.SetBool("isWalk", dirH + dirV != Vector3.zero);
       
        cc.Move(dir * speed * Time.deltaTime);
        
    }

    // 플레이어 스페이스바로 1단 점프하고 싶다.
    // 필요속성 : 점프파워, 중력, y속도, 점프횟수, 최대 점프 가능 횟수
    float jumpPower = 3f;
    float yVelocity;
    float gravity = 7f;
    int jumpCount;
    int MaxJumpCount = 1;
    bool isJumpZone;
    float jumpZonePower = 8f;
    bool isJump;
    void Jump(out float dirY)
    {

        isJump = true;
        if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
            anim.SetBool("isJump", false);
            isJump = false;

        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < MaxJumpCount)
            {
                anim.SetBool("isJump", true);
                anim.SetTrigger("doJump");
                yVelocity = jumpPower;
                jumpCount++;
            }
        }
        if (isJumpZone)
        {
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            yVelocity = jumpZonePower;
            jumpCount++;
            isJumpZone = false;
        }

        dirY = yVelocity;
        yVelocity -= gravity * Time.deltaTime;


    }

    // LeftShift 를 누르면 슬라이딩 하고싶다.
    [HideInInspector]
    public bool isDodge;
    int dodgeCount;
    int MaxDodgeCount = 1;
    float currDodgeTime;
    [HideInInspector]
    public float dodgeCoolTime = 10;
    Vector3 dodgeVecor;
    void Dodge(ref float v)
    {

        if (dodgeCount < MaxDodgeCount)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && cc.isGrounded)
            {
                dodgeVecor = dir;
                speed *= 2;
                anim.SetTrigger("doDodge");
                isDodge = true;
                Invoke("DodgeOut", 1.5f);
                dodgeCount++;
                v = 1;


            }
        }
        else
        {
            currDodgeTime += Time.deltaTime;
            if (currDodgeTime > dodgeCoolTime)
            {
                dodgeCount = 0;
                currDodgeTime = 0;
            }
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    public float rotSpeed = 2000f;
    float y;
    void Rotate()
    {
        float h = Input.GetAxis("Mouse X");

        if (isDie || isDodge) return;
        y += h * rotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }


    // 무기를 바꾸고 싶다.
    // 필요속성 : 무기 배열
    public GameObject[] weapons;
    int weaponIdx;
    bool isSwap;
    void Swap()
    {
        if (isJump || isDodge) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (weaponIdx == 1)
            {
                weapons[0].SetActive(true);
                weapons[1].SetActive(false);
                weaponIdx = 0;
            }
            else if (weaponIdx == 0)
            {
                weapons[0].SetActive(false);
                weapons[1].SetActive(true);
                weaponIdx = 1;
            }

            anim.SetTrigger("doSwap");
            Invoke("SwapOut", 0.4f);

            isSwap = true;
        }     
    }

    void SwapOut()
    {
        isSwap = false;
    }

    // Damage 를 받으면 hp를 깎고 싶다.
    // 필요속성 : 현재hp, 최대hp, hpUI, damage
    float currHP;
    [HideInInspector]
    public float maxHP = 100;
    public Image hpUI;
    public GameObject damage;
    [HideInInspector]
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


    //플레이어를 리스폰 하고싶다. 
    //우유가 있다면 우유도
    // 필요속성 : 플레이어 처음 위치, 우유, 우유 처음 위치, 현재시간, 리스폰 시간
    Vector3 startMilkPos;
    public Transform startPos;
    float currTime;
    [HideInInspector]
    float respawnTime = 10f;
    float reCurrTime;
    public Transform milkPos;
    GameObject isMilk;
    public Text dieCount;
    public void Respawn()
    {
        reCurrTime += Time.deltaTime;
        int count = 10 - (int)reCurrTime;
        dieCount.text = "" + count;
        if (reCurrTime > respawnTime)
        {
            //  현재 hp 를 최대 hp로 초기화
            currHP = maxHP;            
            //enemyCam = null;
            reCurrTime = 0;
            count = 0;
            isDie = false;

        }
        if (isMilk != null)
        {
            isMilk.transform.position = startMilkPos;
            isMilk = null;
        }
    }



    // 에너미 공격
    [HideInInspector]
    public int maxFire = 20;
    int fireCount;
    [HideInInspector]
    public float reloadTime = 3;
    float reloadCurrTime;
    Text bulletCountUI;
    void Attack()
    {
        if (isSwap) return;

        if (fireCount > 0)
        {
            // 자동 발사
            Fire();
            bulletCountUI.text = "총알개수 : " + fireCount;
        }
        else
        {
            bulletCountUI.text = "장전중...";
            Reload();
        }
        Scope();
    }

    // 필요속성 : 에임포인트, 세기, 사거리, 반동, 무게, 슛라인, 재장전, 조준경, 총알개수 등    
    public Transform aimingPoint;
    public GameObject line;
    [HideInInspector]
    public float firePower = 10f;
    [HideInInspector]
    public float fireTime = 0.2f;
    [HideInInspector]
    public float crossroad = 30;
    [HideInInspector]
    public float reboundPower = 0.2f;
    float reboundTime = 30f;
    [HideInInspector]
    public float weight = 1;
    public Transform myCamera;
    [HideInInspector]
    public GameObject enemy;
    float fireCurrTime;
    public GameObject Gun;
    RaycastHit hitInfo;
    void Fire()
    {
        // 반동 후 원래 위치로
        myCamera.localPosition = Vector3.Lerp(myCamera.localPosition, new Vector3(0, 6, -15), Time.deltaTime * reboundTime);
        //LineRenderer lr = null;

        Ray ray = new Ray();
        ray.origin = aimingPoint.position;
        ray.direction = aimingPoint.forward;
        
        if (Physics.Raycast(ray, out hitInfo, crossroad))
        {

            if (hitInfo.transform.gameObject.tag == "Enemy")
            {
                enemy = hitInfo.transform.gameObject;

                fireCurrTime += Time.deltaTime;
                if (fireCurrTime > fireTime)
                {
                    if(weaponIdx == 0)
                    {
                        //GameObject line = Instantiate(LineF);
                        //lr = line.GetComponent<LineRenderer>();
                        //lr.SetPosition(0, Gun.transform.position);
                        //lr.SetPosition(1, hitInfo.point);
                        //Destroy(line, 0.1f);

                        //AudioSource audio = GetComponent<AudioSource>();
                        //audio.Play();

                        //myCamera.Translate(new Vector3(-1, 1, 0) * reboundPower);
                        
                        //fireCount--;
                        //enemy.GetComponent<Na_Enemy_hp>().Damaged(firePower);

                        anim.SetTrigger("doShot");
                    }
                    else
                    {
                        anim.SetTrigger("doSwing");
                    }
                    fireCurrTime = 0;
                }
            }
            else
            {
                fireCurrTime += Time.deltaTime;
                if (fireCurrTime > 0.1f)
                {
                    fireCurrTime = fireTime - 0.5f;
                }
            }

            //if (lr != null)
            //    lr.SetPosition(1, hitInfo.point);
        }
    }

    LineRenderer lr;
    public void Shot()
    {
        line.SetActive(true);

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        myCamera.Translate(new Vector3(-1, 1, 0) * reboundPower);

        fireCount--;
        enemy.GetComponent<Na_Enemy_hp>().Damaged(firePower);

        Invoke("ShotOut", 0.2f);
    }

    void ShotOut()
    {
        line.SetActive(false);
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
    [HideInInspector]
    public float scope = 50;
    int i = 5;
    void Scope()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView -= scope;
            reboundTime += scope;
            reboundPower += scope * 0.02f;
            rotSpeed -= scope * i;
            GameObject cam = GameObject.Find("CameraHinge");
            Na_Rotate camRot =  cam.GetComponent<Na_Rotate>();
            camRot.rotSpeed -= scope * i;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView += scope;
            reboundTime -= scope;
            reboundPower -= scope * 0.02f;
            rotSpeed += scope * i;
            GameObject cam = GameObject.Find("CameraHinge");
            Na_Rotate camRot = cam.GetComponent<Na_Rotate>();
            camRot.rotSpeed += scope * i;
        }
    }


    // 단거리 무기 공격
    public void SwingAttack()
    {
        enemy.GetComponent<Na_Enemy_hp>().Damaged(firePower);
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
