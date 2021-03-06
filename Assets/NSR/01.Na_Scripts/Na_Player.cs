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
// 캐릭터 index 를 받아서 모자를 쓰고싶다.

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
    AudioSource audioSource;  
    public AudioClip[] clip;
  
    enum of
    {
        jump, 
        dodge, 
        swap,          
        die, 
        respawn,
        rifle, 
        reload, 
        milk, 
        milkcontainer,
        cooltime,
        grenade,
        grenadeBomb
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        maxHP = Na_Center.instance.chStat[0];
        longFirePower = longFirePower * Na_Center.instance.chStat[1] * 0.01f;
        ShortFirePower = ShortFirePower * Na_Center.instance.chStat[1] * 0.01f;
        jumpPower = jumpPower * Na_Center.instance.chStat[2] * 0.01f / (Na_Center.instance.chStat[3] * 0.01f);
        jumpZonePower = jumpZonePower / (Na_Center.instance.chStat[3] * 0.01f);
        speed = speed * Na_Center.instance.chStat[3] * 0.01f;
        
        //  현재 hp 를 최대 hp로 초기화
        currHP = maxHP;

        firePower = longFirePower;

        fireCurrTime = fireTime;
        fireCount = maxFire;

        speed -= weight;

        bulletCountUI = GameObject.Find("BulletCount").GetComponent<Text>();
        milkCntUI = GameObject.Find("MilkCount").GetComponent<Text>();
        dieCountUI = GameObject.Find("DieCount");
        dieCountBGUI = GameObject.Find("DieCountBG");
        dieCnt = dieCountUI.GetComponent<Text>();
        dieCntBG = dieCountBGUI.GetComponent<Text>();
        damage = GameObject.Find("Damage");
        hpUI = GameObject.Find("PlayerHp").GetComponent<Image>();
        startPos = GameObject.Find("PlayerPos");
        milkPos = GameObject.Find("PlayerMilkPos");
        aimingPoint = GameObject.Find("AimingPoint");
        weaponPos = GameObject.Find("WeaponPos");
        DodgeUI = GameObject.Find("Dodge").GetComponent<Image>();
        grenadeUI = GameObject.Find("Grenade").GetComponent<Image>();
        bombAnim = bombObj.GetComponent<Animator>();
        fire = GameObject.Find("Na_Fire");
        ReloadUI = GameObject.Find("Reload");
        ReloadBgUI = GameObject.Find("ReloadBg");

        scopeUI.SetActive(false);
        ReloadUI.SetActive(false);
        ReloadBgUI.SetActive(false);

        // damage 투명도 으로
        iTween.ColorTo(damage, iTween.Hash("a", 0f, "time", 0f));

        weapons[0].SetActive(true);

        y = transform.localEulerAngles.y;

        Hats[Na_Center.instance.chNum].SetActive(true);

        line.SetActive(false);
        fire.SetActive(false);
        runSound.SetActive(false);

        audioSource.PlayOneShot(clip[(int)of.respawn]);

        for(int i = 0; i < 20; i++)
        {
            GameObject shell = Instantiate(shellF);
            shells.Add(shell);
            shell.SetActive(false);

        }
    }

    public bool isDontRot;
    // Update is called once per frame
    void Update()
    {
        if (isDie)
        { 
            Respawn();
            grenadeTime = 3;
            grenadeUI.fillAmount = grenadeTime / 3;
            dodgeCoolTime = 10;
            DodgeUI.fillAmount = 1;
            currTime += Time.deltaTime;
            if (currTime > respawnTime - 1)
            {
                currHP = maxHP;
                isDontRot = true;
                anim.SetTrigger("doIdle");
                transform.position = startPos.transform.position;
                transform.forward = new Vector3(0, 0, 1);
                y = 0;
                audioSource.PlayOneShot(clip[(int)of.respawn]);
                playerObj.SetActive(true);
                dieIMG.SetActive(false);
                currTime = 0;
            }
        }
        else
        {
            Move();
            Milk();
            if (!isSwap && !isDodge)
                Attack();
            if(!isDodge && !isReload)
                Swap();
            Rotate();
  
        }
        hpUI.fillAmount = currHP / maxHP;
        print("장전중 : " + isReload);
    }


    // 플레이어 W, S, A, D 로 이동하고 싶다.   
    # region 필요속성 : 속도, CharacterController, 방향    
    public float speed = 10f;
    CharacterController cc;
    [HideInInspector]
    public Vector3 dir;
    public GameObject runSound;
    #endregion
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

        if(dirH + dirV == Vector3.zero || isJump || isDodge || isFall)
        {
            runSound.SetActive(false);
        }
        else
        {
            runSound.SetActive(true);
        }


        cc.Move(dir * speed * Time.deltaTime);

    }


    // 플레이어 스페이스바로 1단 점프하고 싶다.
    #region 필요속성 : 점프파워, 중력, y속도, 점프횟수, 최대 점프 가능 횟수
    float jumpPower = 3f;
    float yVelocity;
    float gravity = 7f;
    int jumpCount;
    int MaxJumpCount = 1;
    bool isJumpZone;
    float jumpZonePower = 6f;
    bool isJump;
    #endregion
    void Jump(out float dirY)
    {
  
        if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
            anim.SetBool("isJump", false);
            anim.SetBool("isDown", false);
            isJump = false;
            isFall = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < MaxJumpCount)
            {
                isJump = true;
                anim.SetBool("isJump", true);
                anim.SetTrigger("doJump");
                audioSource.PlayOneShot(clip[(int)of.jump]);
                yVelocity = jumpPower;
                jumpCount++;
            }
        }
        if (isJumpZone)
        {
            isJump = true;
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            audioSource.PlayOneShot(clip[(int)of.jump]);
            yVelocity = jumpZonePower;
            jumpCount++;
            isJumpZone = false;
        }

        dirY = yVelocity;
        yVelocity -= gravity * Time.deltaTime;


    }

    // LeftShift 를 누르면 슬라이딩 하고싶다.
    #region
    [HideInInspector]
    public bool isDodge;
    float currDodgeTime;
    [HideInInspector]
    public float dodgeCoolTime = 10;
    Vector3 dodgeVecor;
    Image DodgeUI;
    bool canDodge = true;
    #endregion
    void Dodge(ref float v)
    {   
        if (!isSwap)
        {

            if (canDodge)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && cc.isGrounded)
                {
                    dodgeVecor = dir;
                    speed *= 4;
                    anim.SetTrigger("doDodge");
                    Invoke("DodgeEnd", 0.4f);
                    Invoke("DodgeOut", 1.1f);
                    isDodge = true;
                    canDodge = false;                 
                    v = 1;
                    audioSource.PlayOneShot(clip[(int)of.dodge]);
                }
            }
            else
            {
                DodgeUI.fillAmount = currDodgeTime / dodgeCoolTime;
                currDodgeTime += Time.deltaTime;
                if (currDodgeTime > dodgeCoolTime)
                {
                    canDodge = true;
                    currDodgeTime = 0;
                    audioSource.PlayOneShot(clip[(int)of.cooltime]);
                }
            }
        }
    }
    public void DodgeEnd()
    {
        speed *= 0.25f;        
    }

    public void DodgeOut()
    {
        isDodge = false;
    }

    // 마우스를 움직이면 회전하고 싶다
    public float rotSpeed = 500;
    float y;
    void Rotate()
    {
        float h = Input.GetAxis("Mouse X");

        //if (isDie || isDodge) return;
        y += h * rotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }


    // 무기를 바꾸고 싶다.
    #region 필요속성 : 무기 배열
    public GameObject[] weapons;
    public int weaponIdx;
    bool isSwap;
    #endregion
    void Swap()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (weaponIdx == 1)
            {
                weapons[0].SetActive(true);
                firePower = longFirePower;
                fireTime = 0.2f;
                crossroad = 200;
                weight = 2;
                weapons[1].SetActive(false);
                weaponIdx = 0;
            }
            else if (weaponIdx == 0)
            {
                fire.SetActive(false);
                line.SetActive(false);
                anim.SetBool("isShot", false);
                firePower = ShortFirePower;
                weapons[0].SetActive(false);
                fireTime = 1;
                fireCurrTime = fireTime;
                crossroad = 15;
                weight = 0;
                weapons[1].SetActive(true);
                weaponIdx = 1;
            }

            anim.SetTrigger("doSwap");
            Invoke("SwapOut", 0.4f);
            audioSource.PlayOneShot(clip[(int)of.swap]);

            isSwap = true;
        }     
    }
    void SwapOut()
    {
        isSwap = false;
    }


    // Damage 를 받으면 hp를 깎고 싶다.
    #region 필요속성 : 현재hp, 최대hp, hpUI, damage
    float currHP;
    [HideInInspector]
    public float maxHP;
    Image hpUI;
    GameObject damage;
    [HideInInspector]
    public bool isDie;
    public GameObject playerObj;
    public GameObject bombObj;
    Animator bombAnim;
    
    #endregion
    public void Damaged(float damage)
    {
        if (isDie) return;
        currHP -= damage; //HP감소한다
        hpUI.fillAmount = currHP / maxHP; //HP percentage
        ColorA(); // damage 표현

        if (currHP <= 0) //currHp가 0이라면 
        {          
            isDie = true;
            doDieAnim = true;
            
            
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

    //플레이어를 리스폰 하고싶다. 우유가 있다면 우유도
    #region 필요속성 : 플레이어 처음 위치, 우유, 우유 처음 위치, 현재시간, 리스폰 시간
    Vector3 startMilkPos;
    GameObject startPos;
    float currTime;
    [HideInInspector]
    float respawnTime = 10f;
    float reCurrTime;
    GameObject milkPos;
    GameObject isMilk;
    GameObject dieCountUI;
    GameObject dieCountBGUI;
    Text dieCnt;
    Text dieCntBG;
    public bool doDieAnim;
    int before = 11;
    #endregion
    public void Respawn()
    {
        bombObj.SetActive(true);

        if (doDieAnim)
        {
            audioSource.PlayOneShot(clip[(int)of.die]);
            anim.SetTrigger("doDie");
            bombAnim.SetTrigger("doDie");
            doDieAnim = false;
        }

        reCurrTime += Time.deltaTime;      
        int count = (int)respawnTime - (int)reCurrTime;

        if (count == 0)
        {
            dieCnt.text = "";
            dieCntBG.text = "";
        }
        else
        {
            dieCnt.text = count.ToString();
            dieCntBG.text = count.ToString();
        }

        if (before - count > 1)
        {
            //CountdownUp();
            CountdownBGUp();
            before -= 1;
        }

        if (reCurrTime >= 0.7f)
        {
            bombObj.SetActive(false);
        }

        if (reCurrTime >= respawnTime)
        {
            //  현재 hp 를 최대 hp로 초기화
            currHP = maxHP;
            //enemyCam = null;
            reCurrTime = 0;
            count = 0;
            before = 11;
            
            
            isDie = false;
        }
        if (isMilk != null)
        {
            isMilk.transform.position = startMilkPos;
            isMilk = null;
        }
    }

    public GameObject dieIMG;
    public void DieIMG()
    {
        playerObj.SetActive(false);
        dieIMG.SetActive(true);

    }


    void CountdownBGUp()
    {
        iTween.ScaleTo(dieCountBGUI, iTween.Hash(
          "x", 1,
          "y", 1,
          "z", 1,
          "time", 0f,
          "easetype",
          iTween.EaseType.easeInOutBack,
          "oncompletetarget", gameObject,
          "oncomplete", "AfterCountBGdown"
      ));
    }

    void AfterCountBGdown()
    {
        iTween.ScaleTo(dieCountBGUI, iTween.Hash(
          "x", 0,
          "y", 0,
          "z", 0,
          "time", 1f,
          "easetype",
          iTween.EaseType.easeInOutBack
      ));
    }

    // 에너미 공격
    #region 무기 변수
    [HideInInspector]
    public int maxFire = 20;
    [HideInInspector]
    float reloadTime = 2;
    [HideInInspector]
    float firePower;
    float longFirePower = 10;
    float ShortFirePower = 35;
    [HideInInspector]
    public float fireTime = 1f;
    //[HideInInspector]
    public float crossroad = 100;
    [HideInInspector]
    public float reboundPower = 0.2f;
    [HideInInspector]
    public float weight = 1;
    [HideInInspector]
    public float scope = 50;
    #endregion
    #region 필요속성 : 에임포인트, 세기, 사거리, 반동, 무게, 슛라인, 재장전, 조준경, 총알개수 등    
    int fireCount;   
    float reloadCurrTime;
    Text bulletCountUI;
    GameObject aimingPoint;
    public GameObject line;   
    float reboundTime = 30f;    
    public Transform myCamera;
    [HideInInspector]
    public GameObject enemy;
    float fireCurrTime;
    GameObject weaponPos;
    RaycastHit hitInfo;
    LineRenderer lr;
    GameObject fire;
    List<GameObject> shells = new List<GameObject>();
    public GameObject shellF;
    List<GameObject> Activeshells = new List<GameObject>();
    bool doReload;
    #endregion
    void Attack()
    {
        Grenade();

        if (weaponIdx == 0)
        {
            Scope();

            if (doReload)
            {
                Reload();
                
            }
            else if (fireCount > 0 && fireCount < maxFire)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    doReload = true;
                }

                bulletCountUI.text = fireCount + " / " + maxFire;

            }
            else if(fireCount <= 0)
            {
                Reload();
            }
            
        }      
        else
        {
            bulletCountUI.text = "...";
        }

        // 반동 후 원래 위치로
        myCamera.localPosition = Vector3.Lerp(myCamera.localPosition, aimingPoint.transform.localPosition, Time.deltaTime * reboundTime);
        //LineRenderer lr = null;
        if (!isReload && !isSwap)
        {

            Ray ray = new Ray();
            ray.origin = aimingPoint.transform.position;
            ray.direction = aimingPoint.transform.forward;

            if (Physics.Raycast(ray, out hitInfo, crossroad))
            {

                if (hitInfo.transform.gameObject.tag == "Enemy")
                {
                    enemy = hitInfo.transform.gameObject;

                    if (weaponIdx == 0)
                        if (fireCount > 0)
                            anim.SetBool("isShot", true);

                    fireCurrTime += Time.deltaTime;
                    if (fireCurrTime > fireTime)
                    {
                        // 원거리
                        if (weaponIdx == 0)
                        {
                            if (fireCount > 0)
                                Shot();
                        }
                        // 근거리
                        else
                        {                          
                            anim.SetTrigger("doSwing");
                        }
                        fireCurrTime = 0;
                    }
                }
                else
                {
                    anim.SetBool("isShot", false);
                    line.SetActive(false);
                    fire.SetActive(false);

                    fireCurrTime += Time.deltaTime;
                    if (fireCurrTime > 0.1f)
                    {
                        fireCurrTime = fireTime;
                    }
                }

                //if (lr != null)
                //    lr.SetPosition(1, hitInfo.point);

                lr = line.GetComponent<LineRenderer>();
                lr.SetPosition(0, weaponPos.transform.position);
                lr.SetPosition(1, hitInfo.point);
            }
        }
    }

    float grenadeTime;
    bool canGrenade = true;
    Image grenadeUI;
    void Grenade()
    {
        if (canGrenade && !isReload)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("doThrow");
                audioSource.PlayOneShot(clip[(int)of.grenade]);
                grenadeTime = 0;
                canGrenade = false;
            }            
        }
        else
        {
            grenadeTime += Time.deltaTime;
            grenadeUI.fillAmount = grenadeTime / 3;
            if (grenadeTime > 3f)
            {
                canGrenade = true;
                
            }
        }
    }

    // 원거리 무기 공격
    public void Shot()
    {
        line.SetActive(true);
        fire.SetActive(true);

        shells[0].transform.position = weaponPos.transform.position;
        shells[0].SetActive(true);
        Activeshells.Add(shells[0]);
        shells.RemoveAt(0);

        Invoke("ShellDestroy", 1f);

        audioSource.PlayOneShot(clip[(int)of.rifle]);

        myCamera.Translate(new Vector3(-1, 1, 0) * reboundPower);

        fireCount--;
        if (enemy.gameObject.name.Contains("SM"))
        {
            enemy.GetComponent<SM_Enemy_Hp>().Damaged(firePower);
        }
        else if (enemy.gameObject.name.Contains("Na"))
        {
            enemy.GetComponent<KH_EnemyHP>().Damaged(firePower);
        }
        else if (enemy.gameObject.name.Contains("KH"))
        {
            enemy.GetComponent<KH_EnemyHP>().Damaged(firePower);
        }

    }

    void ShellDestroy()
    {
        Activeshells[0].SetActive(false);
        shells.Add(Activeshells[0]);
        Activeshells.RemoveAt(0);
    }

    // 단거리 무기 공격
    public void SwingAttack()
    {
        if (enemy.gameObject.name.Contains("SM"))
        {
            enemy.GetComponent<SM_Enemy_Hp>().Damaged(firePower);
        }
        else if (enemy.gameObject.name.Contains("Na"))
        {
            enemy.GetComponent<KH_EnemyHP>().Damaged(firePower);
        }
        else if (enemy.gameObject.name.Contains("KH"))
        {
            enemy.GetComponent<KH_EnemyHP>().Damaged(firePower);
        }
    }

    bool isReload;
    GameObject ReloadUI;
    GameObject ReloadBgUI;
    void Reload()
    {
        
        //audioSource.PlayOneShot(clip[(int)of.reload]);
        line.SetActive(false);
        fire.SetActive(false);
        ReloadUI.SetActive(true);
        ReloadBgUI.SetActive(true);

        if (reloadCurrTime == 0)
        {
            bulletCountUI.text = "...";
            anim.SetTrigger("doReload");
            audioSource.PlayOneShot(clip[(int)of.reload]);
        }

        reloadCurrTime += Time.deltaTime;
        isReload = true;
 

        if (reloadCurrTime > reloadTime)
        {
            ReloadUI.SetActive(false);
            ReloadBgUI.SetActive(false);
            bulletCountUI.text = maxFire + " / " + maxFire;
            fireCount = maxFire;
            currTime = fireTime;
            isReload = false;
            doReload = false;
            reloadCurrTime = 0;
        }
    }
   
    int i = 8;
    public GameObject scopeUI;
    public GameObject apUI;
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

            scopeUI.SetActive(true);
            apUI.SetActive(false);
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
            scopeUI.SetActive(false);
            apUI.SetActive(true);
        }
    }
   

    // 우유를 먹고 milkContainer 에 넣고싶다.
    public GameObject[] milkContainer;
    int milkCount;
    Text milkCntUI;
    void Milk()
    {
        milkCntUI.text = milkCount + "";
        if (isMilk != null)
            isMilk.transform.position = milkPos.transform.position;

        if (milkCount == 2)
        {
            SceneManager.LoadScene("SM_ResultSceneWin");
        }
    }

    public bool isFall;
    private void OnTriggerEnter(Collider other)
    {
        if (isMilk == null)
        {
            if (other.gameObject.tag == "Milk")
            {
                isMilk = other.gameObject;
                startMilkPos = other.gameObject.transform.position;
                audioSource.PlayOneShot(clip[(int)of.milk]);
            }
        }
        else
        {
            if (other.gameObject.name.Contains("MilkContainer"))
            {
                milkContainer[milkCount].SetActive(true);
                milkCount++;
                Destroy(isMilk.gameObject);
                audioSource.PlayOneShot(clip[(int)of.milkcontainer]);
                isMilk = null;
            }
        }

        // 점프대와 부딪히면 높이 점프하고싶다.
        if (other.gameObject.tag == "JumpZone")
        {
            isJumpZone = true;
            anim.SetBool("isDown", true);
        }

        if (other.gameObject.name.Contains("FallZone"))
        {
            isFall = true;
            jumpCount++;
            anim.SetBool("isDown", true);
        }

        // DestroyZone 과 부딪히면 죽이고 싶다.
        if (other.gameObject.name.Contains("DestroyZone"))
        {
            isDie = true;
            doDieAnim = true;

        }
    }
}
