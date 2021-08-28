using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Player_milk1 : MonoBehaviour
{
    public Transform milkPos;

    GameObject isMilk;

    public GameObject[] milkContainer;

    int milkCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMilk != null)
        isMilk.transform.position = milkPos.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isMilk == null)
        {
            if (other.gameObject.tag == "Milk")
            {
                isMilk = other.gameObject;
            }
        }
        else
        {
            if (other.gameObject.name.Contains("MilkContainer"))
            {
                milkContainer[milkCount].SetActive(true);
                milkCount++;
                Destroy(isMilk.gameObject);
                isMilk = null;
            }
        }
      
    
    }
}
