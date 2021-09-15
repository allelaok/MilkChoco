using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Na_ReloadTime : MonoBehaviour
{
    Image reload;
    // Start is called before the first frame update
    void Start()
    {
        reload = GetComponent<Image>();
        reload.fillAmount = 0;
        currTime = 0;
    }

    float currTime;
    float reloadTime = 2;


    // Update is called once per frame
    void Update()
    {

        if (reload.fillAmount != 1)
        {
            currTime += Time.deltaTime;
        }

        if(currTime > (reloadTime / 10))
        {
            reload.fillAmount += 0.1f;
            currTime = 0;

            
        }
    }

    private void OnDisable()
    {
        reload.fillAmount = 0.1f;
        currTime = 0;
    }
}
