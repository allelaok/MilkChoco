using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_AttackEnemy : MonoBehaviour
{
    // �ʿ�Ӽ� : �����Ŀ�, �߷�, y�ӵ�, ����
    //public float jumpPower = 3f;
    public float yVelocity;
    //public float gravity = 7f;
    //public Vector3 dir;
    public GameObject[] enemyStart; //enemy �迭(4��)
    int[] numbers= new int[4];
    public int num1;
    public int num2;
    //public List<GameObject> Enemies;
    public Transform[] pos;
    
    Vector3 dir;
    public float speed = 5;
    int i;
    int k;
    public float gravity = 1;
    bool isJump = false;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        shuffle();
        print(numbers);

        i = 0;
        cc = GetComponent<CharacterController>();
        startEnemyPos = transform.position;
        //print(enemyStart.Length);
    }
    void shuffle()
    {
        for(int j=0; i < 10; i++)
        {
            int nums1 = Random.Range(0, numbers.Length);
            int nums2 = Random.Range(0, numbers.Length);

            Swap(num1, num2);
        }
        
    }

    void Swap(int m,int n)
    {
        int temp = numbers[m];
        numbers[m] = numbers[n];
        numbers[n] = temp;
    }

    
    public float jumpForwardSpeed = 10;
    float localSpeed;
    // Update is called once per frame
    void Update()
    {
        shuffle();

        //print("�ֳʹ� ��:"+ Enemies);
        if (cc.isGrounded)
        {
            //i++;
            isJump = false;
            localSpeed = speed;
        }
        else
        {
        }
        dir = pos[i + 1].position - transform.position;
        dir.Normalize();
        dir.y = 0;
        float y = 0;
        //print("local speed 111111111111111 " + localSpeed);
        Jump(out y);
        //print("local speed --------------> " + localSpeed);

        dir *= localSpeed * Time.deltaTime;
        //Debug.DrawLine(transform.position, transform.position + dir * 100, Color.red);
        dir.y = y;
        cc.Move(dir);

        Choco();
        #region memo
        //-------------------------------------------------------------------------------------------------------------
        //print("������: " + i);
        //dir = pos[i + 1].position - transform.position;
        //dir.Normalize();


        ////Vector3 distance = transform.position - pos[i + 1].position;
        ////if (distance.magnitude < 0.1f)
        ////{
        ////    i++;
        ////}

        //if (i >= pos.Length - 1)
        //{
        //    dir = Vector3.forward;
        //}



        //transform.position += dir * speed * Time.deltaTime;
        #endregion

        if (isJump == false)
        {
            

        }
        else
        {
            //dir = transform.forward;
            
            
        }



        if (i < pos.Length - 1)
        {
            //cc.Move(Vector3.down * gravity * Time.deltaTime);
            //transform.position += dir * speed * Time.deltaTime;
        }
        else
        {
            //isJump = true;
        }

        //transform.forward = dir;
    }
    Vector3 startChocoPos;
    Vector3 startEnemyPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pos")
        {
            if (other.gameObject.tag == "Pos")
            {
                i++;
            }
            

            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                //�ٸ� ĳ���� �ٲ۴�..?
                i = 0;
            }
        }
        //===========

        if(isChoco == null)
        {
            if (other.gameObject.tag == "Choco")
            {
                print("���ڸ���");
                isChoco = other.gameObject;
                startChocoPos = other.gameObject.transform.position;
                //startChocoPos = isChoco.transform.position;
            }
        }
        if (isChoco != null)
        {
            print("���ڸԹ���");
            //if (other.gameObject.tag =="ChocoContainer")
            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                print("���ھ߹̾߹̾߹̾߹�");
                chocoContainer[chocoCount].SetActive(true);
                chocoCount++;
                Destroy(isChoco.gameObject);
                isChoco = null;
            }
        }

        if (other.gameObject.tag == "JumpZone")
        {
            //print("JJJ");
            isJumpZone = true;
            isJump = true;
        }

    }

    // �ʿ�Ӽ� : ������ġ, ����, milkContainer, ��������, ��������UI
    public Transform chocoPos;
    GameObject isChoco;
    public GameObject[] chocoContainer;
    int chocoCount;
   
    

    void Choco()
    {
        if (isChoco != null)
            isChoco.transform.position = chocoPos.position;

        if (chocoCount == 4)
        {
            print("chocoMax");
        }
    }

    // �ʿ�Ӽ� : ����Ƚ��, �ִ� ���� ���� Ƚ��
    public int jumpCount;
    public int MaxJumpCount = 1;
    bool isJumpZone;
    public float jumpZonePower = 15f;
    public void Jump(out float dirY)
    {
        if (cc.isGrounded)
        {
            //print("��");
            yVelocity = 0;
            //jumpCount = 0;

        }
        
        if (isJumpZone)
        {
            //print("�پ�");
            yVelocity = jumpZonePower;
            //jumpCount++;
            isJumpZone = false;
            localSpeed = jumpForwardSpeed;
            //yVelocity -= gravity * Time.deltaTime;
        }

        yVelocity -= gravity * Time.deltaTime;
        dirY = yVelocity;


    }
}
