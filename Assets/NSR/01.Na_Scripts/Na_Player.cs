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
    Vector3 dir;

    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
    int jumpCount;
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

    bool isDie;
    GameObject enemyCam;

    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾��� CharacterController �� �����´�
        cc = GetComponent<CharacterController>();        
        // �÷��̾��� ó�� ��ġ ����
        startPlayerPos = transform.position;
        //  ���� hp �� �ִ� hp�� �ʱ�ȭ
        currHP = maxHP;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            Die();
                      
        }
        else
        {
            Camera.main.transform.position = aimingPoint.position;
            Camera.main.transform.forward = aimingPoint.forward;
            Move();
            Milk();
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

        cc.Move(dir * speed * Time.deltaTime);
    }

    // �÷��̾� �����̽��ٷ� �����ϰ� �ʹ�.
    // �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����

    // 1�� ������ �ϰ�ʹ�.
    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
    void Jump(out float dirY)
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
    public void Die()
    {

        Camera.main.transform.position = enemyCam.transform.position;
        Camera.main.transform.forward = enemyCam.transform.forward;

        // ���� �ð��� ������ ������
        currTime += Time.deltaTime;
        if(currTime > respawnTime)
        {
            Respawn();
            currTime = 0;
        }      

        if (isMilk != null)
        {
            isMilk.transform.position = startMilkPos;
            isMilk = null;
        }
    }

    void Respawn()
    {
        transform.position = startPlayerPos;
        //  ���� hp �� �ִ� hp�� �ʱ�ȭ
        currHP = maxHP;
        isDie =  false;
        enemyCam = null;
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
    }
}
