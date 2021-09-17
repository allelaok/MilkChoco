using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SM_Enemy_Hp : MonoBehaviour
{
    float currHP;   
    public float maxHP = 100;
    public Image hpUI;


    public bool isDie;
    // Start is called before the first frame update
    void Start()
    {
        //���� HP�� Max HP��
        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        hpUI.fillAmount = currHP / maxHP;
    }

    public void Damaged(float damage)
    {
        //���� HP�� damage��ŭ �ٿ��ش�
        currHP -= damage;

        //���࿡ ���� HP�� 0���� ���ų� ������
        if (currHP <= 0)
        {
            currHP = maxHP;
            isDie = true;
        }
    }
}
