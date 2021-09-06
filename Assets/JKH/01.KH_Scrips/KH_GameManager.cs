using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_GameManager : MonoBehaviour
{
    public static KH_GameManager instance;

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


    //Shuffle, Swap을 위한 필요속성
    public GameObject[] enemyStart; //enemy 배열(4개)
    int[] numbers = { 0, 1, 2, 3 }; //순서 
    public int num1;
    public int num2;
    public int i = 0;


    // Start is called before the first frame update
    public void Start()
    {
        enemyStart[numbers[3]].SetActive(true);
        shuffle(); //처음에 배열 무작위로 섞는다
        print(numbers[0]); //순서확인
        print(numbers[1]);
        print(numbers[2]);
        print(numbers[3]);

        //enemyStart[numbers[i]].SetActive(true);
        //i++;
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

    } //순서 무작위로 섞는다

    void Swap(int m, int n) //2개의 숫자를 바꾼다
    {

        int temp = numbers[m];
        numbers[m] = numbers[n];
        numbers[n] = temp;
    }

    public bool isChoco;
    // Update is called once per frame
    void Update()
    {
        if (isChoco)
        {
            enemyStart[numbers[i]].SetActive(false);
            i++;
            enemyStart[numbers[i]].SetActive(true);
            isChoco = false;
        }
    }
}
