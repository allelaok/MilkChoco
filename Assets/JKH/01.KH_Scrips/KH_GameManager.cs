using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KH_GameManager : MonoBehaviour
{
    public int chocoCount;
    public static KH_GameManager instance;

    Text ChocoCountUI;
    //public GameObject respawnPos;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //Shuffle, Swap�� ���� �ʿ�Ӽ�
    public GameObject[] enemyStart; //enemy �迭(4��)
    public int[] numbers = { 0, 1, 2, 3 }; //���� 
    public int num1;
    public int num2;
    public int i = 0;

    float currTime;
    float A_currTime;
    float respawnTime = 4;
    float ATime;

    //Text ChocoCountUI;
    // Start is called before the first frame update
    public void Start()
    {
        //enemyStart[numbers[3]].SetActive(true);
        shuffle(); //ó���� �迭 �������� ���´�
        print(numbers[0]); //����Ȯ��
        print(numbers[1]);
        print(numbers[2]);
        print(numbers[3]);

        enemyStart[numbers[i]].SetActive(true);
        //i++;

        
        ChocoCountUI = GameObject.Find("ChocoCnt").GetComponent<Text>();
    }

    void shuffle()
    {
        for (int i = 0; i < 10; i++)
        {
            int[] numbers = { 0, 1, 2, 3 };

            int nums1 = Random.Range(0, numbers.Length);
            int nums2 = Random.Range(0, numbers.Length);

            Swap(nums1, nums2);
        }

    } //���� �������� ���´�

    void Swap(int m, int n) //2���� ���ڸ� �ٲ۴�
    {

        int temp = numbers[m];
        numbers[m] = numbers[n];
        numbers[n] = temp;
    }

    public bool isChoco;
    public bool isDie;
    // Update is called once per frame
    void Update()
    {
        if (isChoco)
        {
            enemyStart[numbers[i]].SetActive(false);
            i++;
            enemyStart[numbers[i]].SetActive(true);
            isChoco = false;


            //���� �����̳� ��� ä�����Ѵ�
            //attack move���� ������ chocoCount�� ������ͼ�
            //���������̳ʿ��ٰ� ���ڸ� ä���
            //���� ����ī��Ʈ�� 3�̵ȴٸ�?
            //END Scene�� �̴´�.
        }

        if (isDie)
        {
            //�������Ѵ�
            //����ġ��Ų��(��ǥ�����)
            //Move�Լ��� i�� 0 ���� �ʱ�ȭ�Ѵ�
            //�̰͸� �ϸ� �Ǵµ� ��¥ ������ �𸣁ٳ�

            //A_currTime += Time.deltaTime;
            //if (A_currTime > ATime)
            //{
            //    GetComponent<KH_EnemyAttackMove>().DieAnim();
            //}

            enemyStart[numbers[i]].SetActive(false);
            currTime += Time.deltaTime;
            print("����ð�: " + currTime);
            if (currTime > respawnTime)
            {
                enemyStart[numbers[i]].SetActive(true);
                isDie = false;
                currTime = 0;
                
            }
        }

        ChocoCountUI.text = chocoCount + "";
        if(chocoCount == 4)
        {
            print("��");
        }
    }
}
