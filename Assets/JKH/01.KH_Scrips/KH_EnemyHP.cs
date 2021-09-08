using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KH_EnemyHP : MonoBehaviour
{
    float respawnTime = 10
;    //현재 HP
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
        //현재 HP을 damage만큼 줄여준다
        currHP -= damage;

        hpUI.fillAmount = currHP / maxHP;


        //만약에 현재 HP가 0보다 같거나 작으면
        if (currHP <= 0)
        {
            int i = KH_GameManager.instance.i;
            KH_GameManager.instance.enemyStart[i].SetActive(false);
            currTime += Time.deltaTime;
            if (currTime > respawnTime)
            {
                KH_GameManager.instance.enemyStart[i].SetActive(true);
            }
            currTime = 0;
            currHP = maxHP;
            
            // instance로만든다
            // get Compomponent
            
        }
    }

    
}
