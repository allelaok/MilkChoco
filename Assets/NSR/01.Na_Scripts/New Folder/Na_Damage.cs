using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Damage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        iTween.ColorTo(gameObject, iTween.Hash(
          "a", 0f,
          "time", 0f

            ));
        //ColorA();

    }

    // Update is called once per frame
    void Update()
    {
        if (Na_Player.instace.isDie) return;

        //ColorBack();
        if (Input.GetMouseButtonDown(0))
        ColorA();
    }

    void ColorA()
    {
        iTween.ColorTo(gameObject, iTween.Hash(
        "a", 1f,
        "time", 0.15f,

        "oncompletetarget", gameObject,
        "oncomplete", "ColorBack"
        ));
    }

    void ColorBack()
    {
        iTween.ColorTo(gameObject, iTween.Hash(
          "a", 0f,
          "time", 0.3f
           ));
    }
}
