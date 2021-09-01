using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Na_Enemy_choco : MonoBehaviour
{
    public Transform chocoPos;

    GameObject isChoco;

    public GameObject[] chocoContainer;

    int chocoCount;
    public Text chocoCntUi;

    // Start is called before the first frame update
    void Start()
    {
        chocoCntUi.text = 0 + "/4";
    }

    // Update is called once per frame
    void Update()
    {
        chocoCntUi.text = chocoCount + "/4";
        if (isChoco != null)
            isChoco.transform.position = chocoPos.position;

        if (chocoCount == 4)
        {
            SceneManager.LoadScene("Na_Lose");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isChoco == null)
        {
            if (other.gameObject.tag == "Choco")
            {
                isChoco = other.gameObject;
            }
        }
        else
        {
            if (other.gameObject.name.Contains("ChocoContainer"))
            {
                chocoContainer[chocoCount].SetActive(true);
                chocoCount++;
                Destroy(isChoco.gameObject);
                isChoco = null;
            }
        }


    }
}
