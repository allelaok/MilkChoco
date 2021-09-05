using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KH_EnemyHP : MonoBehaviour
{
    //현재 HP
    float currHP;
    // Max HP
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
        //현재 HP을 damage만큼 줄여준다
        currHP -= damage;

        hpUI.fillAmount = currHP / maxHP;


        //만약에 현재 HP가 0보다 같거나 작으면
        if (currHP <= 0)
        {
            // instance로만든다
            // get Compomponent
        }
    }

    
}
