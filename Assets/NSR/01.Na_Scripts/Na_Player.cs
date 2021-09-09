using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// �÷��̾� W, S, A, D �� �̵��ϰ� �ʹ�.
// �÷��̾� �����̽��ٷ� 1�� �����ϰ� �ʹ�.
// Damage �� ������ hp�� ��� �ʹ�. UI �� ǥ���ϰ� �ʹ�.
// hp�� 0���ϸ� ������ �ϰ�ʹ�
// ������ �԰� milkContainer �� �ְ� �ʹ�.
// ������� �ε����� ���� �����ϰ�ʹ�.
// ���ʹ̿� ������ ���߸� �����ϰ�ʹ�.
// �÷��̾� �ִϸ��̼��� �־��ְ� �ʹ�.
// ���⸦ �ٲٰ� �ʹ�.
// ĳ���� idex �� �޾Ƽ� ���ڸ� ����ʹ�.

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

        // �÷��̾��� CharacterController �� �����´�
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();
    }

    Animator anim;

    public GameObject[] Hats;

    // Start is called before the first frame update
    void Start()
    {
        //  ���� hp �� �ִ� hp�� �ʱ�ȭ
        currHP = maxHP;

        fireCurrTime = fireTime;
        fireCount = maxFire;

        speed -= weight;

        bulletCountUI = GameObject.Find("BulletCount").GetComponent<Text>();
        milkCntUI = GameObject.Find("MilkCount").GetComponent<Text>();
        dieCountUI = GameObject.Find("DieCount").GetComponent<Text>();
        damage = GameObject.Find("Damage");
        hpUI = GameObject.Find("PlayerHp").GetComponent<Image>();
        startPos = GameObject.Find("PlayerPos");
        milkPos = GameObject.Find("PlayerMilkPos");
        aimingPoint = GameObject.Find("AimingPoint");
        weaponPos = GameObject.Find("WeaponPos");

        // damage ���� ����
        iTween.ColorTo(damage, iTween.Hash("a", 0f, "time", 0f));

        weapons[0].SetActive(true);

        y = transform.localEulerAngles.y;

        Hats[Na_Center.instance.chNum].SetActive(true);

        line.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        print(fireTime);

        if (isDie)
        {
            
            Respawn();
            currTime += Time.deltaTime;
            if (currTime > respawnTime - 1)
            {
                anim.SetTrigger("doIdle");
                transform.position = startPos.transform.position;
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
        lr.SetPosition(0, weaponPos.transform.position);
        lr.SetPosition(1, hitInfo.point);

    }

    // �÷��̾� W, S, A, D �� �̵��ϰ� �ʹ�.   
    # region �ʿ�Ӽ� : �ӵ�, CharacterController, ����    
    [HideInInspector]
    public float speed = 7f;
    CharacterController cc;
    [HideInInspector]
    public Vector3 dir;
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
       
        cc.Move(dir * speed * Time.deltaTime);
        
    }


    // �÷��̾� �����̽��ٷ� 1�� �����ϰ� �ʹ�.
    #region �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����Ƚ��, �ִ� ���� ���� Ƚ��
    float jumpPower = 3f;
    float yVelocity;
    float gravity = 7f;
    int jumpCount;
    int MaxJumpCount = 1;
    bool isJumpZone;
    float jumpZonePower = 8f;
    bool isJump;
    #endregion
    void Jump(out float dirY)
    {

        isJump = true;
        if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
            anim.SetBool("isJump", false);
            anim.SetBool("isDown", false);
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

    // LeftShift �� ������ �����̵� �ϰ�ʹ�.
    #region
    [HideInInspector]
    public bool isDodge;
    int dodgeCount;
    int MaxDodgeCount = 1;
    float currDodgeTime;
    [HideInInspector]
    public float dodgeCoolTime = 10;
    Vector3 dodgeVecor;
    #endregion
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

    // ���콺�� �����̸� ȸ���ϰ� �ʹ�
    public float rotSpeed = 500;
    float y;
    void Rotate()
    {
        float h = Input.GetAxis("Mouse X");

        //if (isDie || isDodge) return;
        y += h * rotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }


    // ���⸦ �ٲٰ� �ʹ�.
    #region �ʿ�Ӽ� : ���� �迭
    public GameObject[] weapons;
    int weaponIdx;
    bool isSwap;
    #endregion
    void Swap()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (weaponIdx == 1)
            {
                weapons[0].SetActive(true);
                firePower = 5;
                fireTime = 0.2f;
                crossroad = 30;
                weight = 2;
                weapons[1].SetActive(false);
                weaponIdx = 0;
            }
            else if (weaponIdx == 0)
            {
                weapons[0].SetActive(false);
                firePower = 20;
                fireTime = 1;
                crossroad = 10;
                weight = 0;
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

    // Damage �� ������ hp�� ��� �ʹ�.
    #region �ʿ�Ӽ� : ����hp, �ִ�hp, hpUI, damage
    float currHP;
    [HideInInspector]
    public float maxHP = 100;
    Image hpUI;
    GameObject damage;
    [HideInInspector]
    public bool isDie;
    #endregion
    public void Damaged(float damage)
    {
        if (isDie) return;
        currHP -= damage; //HP�����Ѵ�
        hpUI.fillAmount = currHP / maxHP; //HP percentage
        ColorA(); // damage ǥ��

        if (currHP <= 0) //currHp�� 0�̶�� 
        {          
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


    //�÷��̾ ������ �ϰ�ʹ�. ������ �ִٸ� ������
    #region �ʿ�Ӽ� : �÷��̾� ó�� ��ġ, ����, ���� ó�� ��ġ, ����ð�, ������ �ð�
    Vector3 startMilkPos;
    GameObject startPos;
    float currTime;
    [HideInInspector]
    float respawnTime = 10f;
    float reCurrTime;
    GameObject milkPos;
    GameObject isMilk;
    Text dieCountUI;
    #endregion
    public void Respawn()
    {
        reCurrTime += Time.deltaTime;

        if(reCurrTime <= 0.1)
            anim.SetTrigger("doDie");
        //Invoke("DieOut", 1.5f);

        int count = 10 - (int)reCurrTime;
        dieCountUI.text = "" + count;



        if (reCurrTime > respawnTime)
        {
            //  ���� hp �� �ִ� hp�� �ʱ�ȭ
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

    void DieOut()
    {
        anim.SetBool("isDie", false);
    }

    // ���ʹ� ����
    #region ���� ����
    [HideInInspector]
    public int maxFire = 20;
    [HideInInspector]
    public float reloadTime = 3;
    [HideInInspector]
    public float firePower = 10f;
    [HideInInspector]
    public float fireTime = 1f;
    [HideInInspector]
    public float crossroad = 30;
    [HideInInspector]
    public float reboundPower = 0.2f;
    [HideInInspector]
    public float weight = 1;
    [HideInInspector]
    public float scope = 50;
    #endregion
    #region �ʿ�Ӽ� : ��������Ʈ, ����, ��Ÿ�, �ݵ�, ����, ������, ������, ���ذ�, �Ѿ˰��� ��    
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
    #endregion
    void Attack()
    {
        if (isSwap || isDodge) return;

        Scope();

        if (weaponIdx == 0)
        {
            if (fireCount > 0)
            {
                bulletCountUI.text = "�Ѿ˰��� : " + fireCount;
            }
            else
            {
                bulletCountUI.text = "������...";
                Reload();
            }
        }
        else
        {
            bulletCountUI.text = "...";
        }

        // �ݵ� �� ���� ��ġ��
        myCamera.localPosition = Vector3.Lerp(myCamera.localPosition, new Vector3(0, 6, -15), Time.deltaTime * reboundTime);
        //LineRenderer lr = null;

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
                    // ���Ÿ�
                    if(weaponIdx == 0)
                    {
                        if (fireCount > 0)  
                            Shot();
                    }                  
                    // �ٰŸ�
                    else
                    {
                        bulletCountUI.text = "...";
                        anim.SetTrigger("doSwing");
                    }
                    fireCurrTime = 0;
                }
            }
            else
            {
                anim.SetBool("isShot", false);
                line.SetActive(false);

                fireCurrTime += Time.deltaTime;
                if (fireCurrTime > 0.1f)
                {
                    fireCurrTime = fireTime;
                }
            }

            //if (lr != null)
            //    lr.SetPosition(1, hitInfo.point);
        }
    }

    // ���Ÿ� ���� ����
    public void Shot()
    {
        line.SetActive(true);

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        myCamera.Translate(new Vector3(-1, 1, 0) * reboundPower);

        fireCount--;
        enemy.GetComponent<Na_Enemy_hp>().Damaged(firePower);
    }
    // �ܰŸ� ���� ����
    public void SwingAttack()
    {
        enemy.GetComponent<Na_Enemy_hp>().Damaged(firePower);
    }

    bool isReload;
    void Reload()
    {

        reloadCurrTime += Time.deltaTime;

        isReload = true;
        //anim.SetBool("isReload", true);
        if (reloadCurrTime <= 0.1)
            //anim.SetBool("isReload", false);
            anim.SetTrigger("doReload");

        
        if (reloadCurrTime > reloadTime)
        {
            
            fireCount = maxFire;
            currTime = fireTime;
            reloadCurrTime = 0;
        }
    }
   
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
   

    // ������ �԰� milkContainer �� �ְ�ʹ�.
    public GameObject[] milkContainer;
    int milkCount;
    Text milkCntUI;
    void Milk()
    {
        milkCntUI.text = milkCount + "/4";
        if (isMilk != null)
            isMilk.transform.position = milkPos.transform.position;

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

        // ������� �ε����� ���� �����ϰ�ʹ�.
        if (other.gameObject.tag == "JumpZone")
        {
            isJumpZone = true;
            anim.SetBool("isDown", true);
        }

        if (other.gameObject.name.Contains("FallZone"))
        {
            jumpCount++;
            anim.SetBool("isDown", true);
        }

        // DestroyZone �� �ε����� ���̰� �ʹ�.
        if (other.gameObject.name.Contains("DestroyZone"))
        {
            isDie = true;
        }
    } 
}
