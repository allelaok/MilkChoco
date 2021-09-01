using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// idle 상태일 때 카메라의 위치를 AimingPoint 로 하고 싶다.
// 플레이어 W, S, A, D 로 이동하고 싶다.
// 플레이어 스페이스바로 점프하고 싶다.
// 1단 점프를 하고싶다.
// Damage 를 받으면 hp를 깎고 싶다. UI 도 표현하고 싶다.
// hp가 0이하면 죽이고 싶다.
// 우유를 먹고 milkContainer 에 넣고 싶다.

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

    // 필요속성 : AimingPoint
    public Transform aimingPoint;

    // 필요속성 : 속도, CharacterController
    public float speed = 7f;
    CharacterController cc;

    // 필요속성 : 점프파워, 중력, y속도, 방향
    public float jumpPower = 3f;
    float yVelocity;
    public float gravity = 7f;
    Vector3 dir;

    // 필요속성 : 점프횟수, 최대 점프 가능 횟수
    int jumpCount;
    public int MaxJumpCount = 1;

    // 필요속성 : 현재hp, 최대hp, hpUI
    public float currHP;
    public float maxHP = 100;
    public Image hpUI;

    // 필요속성 : 우유위치, 우유, milkContainer, 우유개수, 우유개수UI
    public Transform milkPos;
    GameObject isMilk;
    public GameObject[] milkContainer;
    int milkCount;
    public Text milkCntUI;

    //플레이어를 리스폰 하고싶다.
    //우유가 있다면 우유도
    // 필요속성 : 플레이어 처음 위치, 우유 처음 위치, 현재시간, 리스폰 시간
    Vector3 startMilkPos;
    Vector3 startPlayerPos;
    float currTime;
    public float respawnTime = 10f;

    bool isDie;
    GameObject enemyCam;

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어의 CharacterController 를 가져온다
        cc = GetComponent<CharacterController>();        
        // 플레이어의 처음 위치 저장
        startPlayerPos = transform.position;
        //  현재 hp 를 최대 hp로 초기화
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

 

    // 플레이어 W, S, A, D 로 이동하고 싶다.
    // 필요속성 : 속도, CharacterController
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
    // 필요속성 : 점프파워, 중력, y속도, 방향

    // 1단 점프를 하고싶다.
    // 필요속성 : 점프횟수, 최대 점프 가능 횟수
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

    // Damage 를 받으면 hp를 깎고 싶다.
    public void Damaged(float damage, GameObject enemyCamPos)
    {
        currHP -= damage; //HP감소한다
        hpUI.fillAmount = currHP / maxHP; //HP percentage

        if (currHP <= 0) //currHp가 0이라면 
        {
            enemyCam = enemyCamPos;
            //Camera.main.transform.position = enemyCamPos.transform.position;
            isDie = true;
        }
    }

    // 죽으면 리스폰 하고 싶다. 
    // 우유도
    public void Die()
    {

        Camera.main.transform.position = enemyCam.transform.position;
        Camera.main.transform.forward = enemyCam.transform.forward;

        // 일정 시간이 지나면 리스폰
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
        //  현재 hp 를 최대 hp로 초기화
        currHP = maxHP;
        isDie =  false;
        enemyCam = null;
    }

    // 우유를 먹고 milkContainer 에 넣고싶다.
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
