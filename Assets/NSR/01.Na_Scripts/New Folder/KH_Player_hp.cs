using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KH_Player_hp : MonoBehaviour
{
    public float currHP;
    public float maxHP = 100;
    public Image hpUI;
    GameObject ismilk; //내가 먹은우유
    public Transform milkPos; //근데 이거 4개있잖음... 나중에 조정한다
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
        currHP -= damage; //HP감소한다
        hpUI.fillAmount = currHP / maxHP; //HP percentage

        if (currHP <= 0) //currHp가 0이라면 
        {
            gameObject.transform.position = EnemyStartPos.transform.position;
            if (ismilk)
            {
                //우유정위치
            }
            gameObject.SetActive(false);
            currTIme += Time.deltaTime;
            if (currTIme > bornTime)
            {
                gameObject.SetActive(false);
                //다른스크립트에서 이거 state바꾼다.
            }
            
        }
    }
}
