using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KH_Player_hp : MonoBehaviour
{
    public float currHP;
    public float maxHP = 100;
    public Image hpUI;
    GameObject ismilk; //���� ��������
    public Transform milkPos; //�ٵ� �̰� 4��������... ���߿� �����Ѵ�
    public Transform EnemyStartPos;
    float currTIme;
    float bornTime=15;
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
        currHP -= damage; //HP�����Ѵ�
        hpUI.fillAmount = currHP / maxHP; //HP percentage

        if (currHP <= 0) //currHp�� 0�̶�� 
        {
            gameObject.transform.position = EnemyStartPos.transform.position;
            if (ismilk)
            {
                //��������ġ
            }
            gameObject.SetActive(false);
            currTIme += Time.deltaTime;
            if (currTIme > bornTime)
            {
                gameObject.SetActive(false);
                //�ٸ���ũ��Ʈ���� �̰� state�ٲ۴�.
            }
            
        }
    }
}
