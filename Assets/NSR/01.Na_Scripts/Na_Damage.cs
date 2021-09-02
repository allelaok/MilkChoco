using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Damage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ColorBack();
    }

    // Update is called once per frame
    void Update()
    {
        ColorA();
    }

    void ColorA()
    {
        iTween.ColorTo(gameObject, iTween.Hash(
        "a", 0.8f,
        "time", 0.15f,

        "oncompletetarget", gameObject,
        "oncomplete", "ColorBack"
        ));
    }

    void ColorBack()
    {
        iTween.ColorTo(gameObject, iTween.Hash(
          "a", 0f,
          "time", 0.15f

            ));
    }
}
