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
// Damage �� ������ hp�� ��� �ʹ�.
// hp�� 0���ϸ� ���̰� �ʹ�.
// ������ �԰� milkContainer �� �ְ�ʹ�.

public class Na_Player : MonoBehaviour
{
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

    //������ ������ �ϰ�ʹ�.
    // �ʿ�Ӽ� : ���� ó�� ��ġ
    Vector3 startMilkPos;
    Vector3 startPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾��� CharacterController �� �����´�
        cc = GetComponent<CharacterController>();
        // �������� text �ʱ�ȭ
        milkCntUI.text = 0 + "/4";
        // �÷��̾��� ó�� ��ġ ����
        startPlayerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        Move();

        Milk();
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
    public void Damaged(float damage)
    {
        currHP -= damage; //HP�����Ѵ�
        hpUI.fillAmount = currHP / maxHP; //HP percentage

        if (currHP <= 0) //currHp�� 0�̶�� 
        {

            Die();
        }
    }

    // ������ ������ �ϰ� �ʹ�. 
    // ������
    void Die()
    {
        transform.position = startPlayerPos;
        Respawn();

        if (isMilk != null)
        {
            isMilk.transform.position = startMilkPos;
            isMilk = null;
        }
    }

    void Respawn()
    {
        Camera.main.transform.position = aimingPoint.position;
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
                startMilkPos = isMilk.transform.position;
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
