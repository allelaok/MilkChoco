using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    // Flash Image
    bool changed;
    public Image mFlashImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(changed)
        {
            mFlashImage.color = flashColour;
        }
        else
        {
            mFlashImage.color = Color.Lerp(mFlashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        changed = false;
    }
}
