using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KH_EnemyHP : MonoBehaviour
{
    //float respawnTime = 3;
    //���� HP
    float currHP;
    // Max HP
    public float maxHP = 100;
    public Image hpUI;
    float currTime;

    
    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damaged(float damage)
    {
        //���� HP�� damage��ŭ �ٿ��ش�
        currHP -= damage;

        hpUI.fillAmount = currHP / maxHP;


        //���࿡ ���� HP�� 0���� ���ų� ������
        if (currHP <= 0)
        {
            //int[] numbers = KH_GameManager.instance.numbers;
            //int i = KH_GameManager.instance.i;
            //KH_GameManager.instance.enemyStart[numbers[i]].SetActive(false);
            //currTime += Time.deltaTime;
            //print("����ð�: " + currTime);


            //if (currTime > respawnTime)
            //{
            //    KH_GameManager.instance.enemyStart[numbers[i]].SetActive(true);
            //    currTime = 0;
            //    currHP = maxHP;
            //}
            KH_GameManager.instance.isDie = true;
            currTime += Time.deltaTime;
            if (currTime > 2.5f)
            {
                currHP = maxHP;

            }


            // instance�θ����
            // get Compomponent

        }
    }

    
}
