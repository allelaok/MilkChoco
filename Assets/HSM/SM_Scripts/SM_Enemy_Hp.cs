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
        
        //현재 HP를 Max HP로
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
            //나를 파괴하자
            //gameObject.SetActive(false);
            SM_EnemyRespawn Enemy =GameObject.Find("SM_Manager").GetComponent<SM_EnemyRespawn>(); //여기부분 일괄수정?
            Enemy.isDie = true;
            currHP = maxHP;
            hpUI.fillAmount = 1;
            
            

        }
    }
}
