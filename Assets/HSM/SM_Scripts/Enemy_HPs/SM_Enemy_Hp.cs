using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SM_Enemy_Hp : MonoBehaviour
{
    float currHP;
    
    public float maxHP = 100;

    public Image hpUI;

    Animation anim;

    private void Awake()
    {
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //���� HP�� Max HP��
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
            //���� �ı�����
            //gameObject.SetActive(false);
            SM_EnemyRespawn Enemy =GameObject.Find("SM_Manager").GetComponent<SM_EnemyRespawn>(); //����κ� �ϰ�����?
            Enemy.isDie = true;
            currHP = maxHP;
            hpUI.fillAmount = 1;
            
            

        }
    }
}
