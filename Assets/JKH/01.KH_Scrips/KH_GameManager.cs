using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KH_GameManager : MonoBehaviour
{
    public GameObject[] enemyStart; //enemy ¹è¿­(4°³)
    int[] numbers = { 0, 1, 2, 3 };
    public int num1;
    public int num2;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        shuffle();
        print(numbers[0]);
        print(numbers[1]);
        print(numbers[2]);
        print(numbers[3]);

        enemyStart[i].SetActive(true);
        i++;
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

    }

    void Swap(int m, int n)
    {

        int temp = numbers[m];
        numbers[m] = numbers[n];
        numbers[n] = temp;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
