using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KH_GameManager : MonoBehaviour
{
    public int chocoCount;
    public static KH_GameManager instance;
    public GameObject timeUI;
    

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


    //Shuffle, Swap을 위한 필요속성
    public GameObject[] enemyStart; //enemy 배열(4개)
    public int[] numbers = { 0, 1, 2, 3 }; //순서 
    public int num1;
    public int num2;
    public int i = 0;

    float currTime;
    float A_currTime;
    float respawnTime = 10;
    float ATime;

    //Text ChocoCountUI;
    // Start is called before the first frame update
    public void Start()
    {
        //enemyStart[numbers[3]].SetActive(true);
        shuffle(); //처음에 배열 무작위로 섞는다
        print(numbers[0]); //순서확인
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

    } //순서 무작위로 섞는다

    void Swap(int m, int n) //2개의 숫자를 바꾼다
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
        //timeUIs();

        if (isChoco)
        {
            enemyStart[numbers[i]].SetActive(false);
            i++;
            if (i == 2)
            {
                print("끝"); //여기다 씬 넣어야하나
                SceneManager.LoadScene("SM_ResultSceneLose");
            }
            enemyStart[numbers[i]].SetActive(true);
            isChoco = false;



            //초코 컨테이너 계속 채워야한다
            //attack move에서 더해진 chocoCount를 가지고와서
            //초코컨테이너에다가 초코를 채운다
            //만약 초코카운트가 3이된다면?
            //END Scene을 뽑는다.
        }

        if (isDie)
        {
            //리스폰한다
            //원위치시킨다(좌표찍어줌)
            //Move함수에 i값 0 으로 초기화한다
            //이것만 하면 되는데 진짜 위에걸 모르곘네

            //A_currTime += Time.deltaTime;
            //if (A_currTime > ATime)
            //{
            //    GetComponent<KH_EnemyAttackMove>().DieAnim();
            //}
            //GetComponent<KH_EnemyAttackMove>().DieAnim();
            enemyStart[numbers[i]].SetActive(false);
            currTime += Time.deltaTime;
            print("현재시간: " + currTime);
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
            //print("끝");
            //SceneManager.LoadScene(""); @@@@@@@@@@@ 성공씬넣는다...
        }
        //Na_Player.instace.weaponIdx 멀리 돌아간 KH
        Na_Player weapon = GameObject.Find("Na_Player").GetComponent<Na_Player>();
        if (weapon.weaponIdx == 0)
        {
            HammerToGun();
        }

        if(weapon.weaponIdx == 1)
        {
            
            GunToHammer();
        }

    }

    public void timeUIs()
    {
        currTime += Time.deltaTime;
        if (currTime > 1f)
        {
            //itween
            iTween.ScaleTo(timeUI, iTween.Hash(
                "x", 1,
                "y", 1,
                "z", 1,
                "time", 0.5f,
                "easetype", iTween.EaseType.easeInOutBack

                ));
            iTween.ScaleTo(timeUI, iTween.Hash(
                "x", 1.2,
                "y", 1.2,
                "z", 1.2,
                "time", 0.5f,
                "delay", 0.5f,
                "easetype", iTween.EaseType.easeInOutBack
                ));
            currTime = 0;
        }

    }

    public GameObject Gun;
    public GameObject Hammer;

    public void GunToHammer()
    {
        Gun.SetActive(false);
        Hammer.SetActive(true);
    }
    
    public void HammerToGun()
    {
        Hammer.SetActive(false);
        Gun.SetActive(true);
    }
    
}
