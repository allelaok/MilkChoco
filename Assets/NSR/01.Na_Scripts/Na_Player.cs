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

    // Start is called before the first frame update
    void Start()
    {
        //  ���� hp �� �ִ� hp�� �ʱ�ȭ
        currHP = maxHP;

        currTime = fireTime;
        fireCount = maxFire;
     
        speed -= weight;

        bulletCountUI = GameObject.Find("BulletCount").GetComponent<Text>();

        // damage ���� 0����
        iTween.ColorTo(damage, iTween.Hash("a", 0f,"time", 0f));

        equipWeapon = weapons[0];
        equipWeapon.SetActive(true);


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
            Swap();
        }

    }



    // �÷��̾� W, S, A, D �� �̵��ϰ� �ʹ�.
    // �ʿ�Ӽ� : �ӵ�, CharacterController, ����
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

        if (isDodge)
            dir = dodgeVecor;

        Jump(out dir.y);
        Dodge();

        anim.SetBool("isWalk", dirH + dirV != Vector3.zero);
       
        cc.Move(dir * speed * Time.deltaTime);
        
    }

    // �÷��̾� �����̽��ٷ� 1�� �����ϰ� �ʹ�.
    // �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����Ƚ��, �ִ� ���� ���� Ƚ��
    public float jumpPower = 3f;
    float yVelocity;
    public float gravity = 7f;
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

    // LeftShift �� ������ �����̵� �ϰ�ʹ�.
    public bool isDodge;
    int dodgeCount;
    int MaxDodgeCount = 1;
    float currDodgeTime;
    public float dodgeCoolTime = 10;
    Vector3 dodgeVecor;
    void Dodge()
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

    // ���⸦ �ٲٰ� �ʹ�.
    // �ʿ�Ӽ� : ���� �迭
    public GameObject[] weapons;
    public bool[] hasWeapon;
    GameObject equipWeapon;
    int weaponIdx;
    bool isSwap;

    void Swap()
    {
        if (isJump || isDodge) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weaponIdx == 1)
            {
                weaponIdx = 0;
            }
            else
            {
                fireTime = 1;
                weaponIdx = 1;
            }

            equipWeapon.SetActive(false);
            equipWeapon = weapons[weaponIdx];
            equipWeapon.SetActive(true);

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
    // �ʿ�Ӽ� : ����hp, �ִ�hp, hpUI, damage
    public float currHP;
    public float maxHP = 100;
    public Image hpUI;
    public GameObject damage;
    public bool isDie;
    public void Damaged(float damage, GameObject enemyCamPos)
    {
        if (isDie) return;
        currHP -= damage; //HP�����Ѵ�
        hpUI.fillAmount = currHP / maxHP; //HP percentage
        ColorA(); // damage ǥ��

        if (currHP <= 0) //currHp�� 0�̶�� 
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


    //�÷��̾ ������ �ϰ�ʹ�. 
    //������ �ִٸ� ������
    // �ʿ�Ӽ� : �÷��̾� ó�� ��ġ, ����, ���� ó�� ��ġ, ����ð�, ������ �ð�
    Vector3 startMilkPos;
    public Transform startPos;
    float currTime;
    public float respawnTime = 3f;
    float reCurrTime;
    public Transform milkPos;
    GameObject isMilk;
    public void Respawn()
    {
        reCurrTime += Time.deltaTime;
        if (reCurrTime > respawnTime)
        {
            //  ���� hp �� �ִ� hp�� �ʱ�ȭ
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



    // ���ʹ� ����
    public int maxFire = 20;
    int fireCount;
    public float reloadTime = 3;
    float reloadCurrTime;
    Text bulletCountUI;
    void Attack()
    {
        if (fireCount > 0)
        {
            // �ڵ� �߻�
            Fire();
            bulletCountUI.text = "�Ѿ˰��� : " + fireCount;
        }
        else
        {
            bulletCountUI.text = "������...";
            Reload();
        }
        Scope();
    }

    // �ʿ�Ӽ� : ��������Ʈ, ����, ��Ÿ�, �ݵ�, ����, ������, ������, ���ذ�, �Ѿ˰��� ��    
    public Transform aimingPoint;
    public GameObject LineF; 
    public float firePower = 10f;
    public float fireTime = 0.2f;
    public float crossroad = 30;
    public float reboundPower = 0.2f;
    public float reboundTime = 30f;
    public float weight = 1;
    public Transform myCamera;
    public GameObject enemy;
    void Fire()
    {
        // �ݵ� �� ���� ��ġ��
        myCamera.localPosition = Vector3.Lerp(myCamera.localPosition, new Vector3(0, 6, -15), Time.deltaTime * reboundTime);
        LineRenderer lr = null;

        Ray ray = new Ray();
        ray.origin = aimingPoint.position;
        ray.direction = aimingPoint.forward;
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, crossroad))
        {          
            if (hitInfo.transform.gameObject.tag == "Enemy")
            {
                enemy = hitInfo.transform.gameObject;

                currTime += Time.deltaTime;
                if (currTime > fireTime)
                {
                    if(weaponIdx == 0)
                    {
                        GameObject line = Instantiate(LineF);
                        lr = line.GetComponent<LineRenderer>();
                        lr.SetPosition(0, transform.position);
                        lr.SetPosition(1, hitInfo.point);
                        Destroy(line, 0.1f);

                        AudioSource audio = GetComponent<AudioSource>();
                        audio.Play();

                        myCamera.Translate(new Vector3(-1, 1, 0) * reboundPower);
                        
                        fireCount--;
                        enemy.GetComponent<Na_Enemy_hp>().Damaged(firePower);
                    }
                    else
                    {
                        anim.SetTrigger("doSwing");
                    }                   
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


    // �ܰŸ� ���� ����
    void SwingAttack()
    {
        enemy.GetComponent<Na_Enemy_hp>().Damaged(firePower);
    }

    

    // ������ �԰� milkContainer �� �ְ�ʹ�.
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

        // ������� �ε����� ���� �����ϰ�ʹ�.
        if (other.gameObject.tag == "JumpZone")
        {
            isJumpZone = true;
        }

        if (other.gameObject.name.Contains("FallZone"))
        {
            jumpCount++;
        }

        // DestroyZone �� �ε����� ���̰� �ʹ�.
        if (other.gameObject.name.Contains("DestroyZone"))
        {
            isDie = true;
        }
    } 

}
