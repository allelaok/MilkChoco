using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KH_Player_hp : MonoBehaviour
{
    public float currHP;
    public float maxHP = 100;
    public Image hpUI;
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
            gameObject.SetActive(false);
            Na_DestroyZone.instance.playerDie = true;
        }
    }
}
