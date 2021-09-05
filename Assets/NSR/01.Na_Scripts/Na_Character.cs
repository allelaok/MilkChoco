using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Character : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.forward == Vector3.forward)
        {
            anim.SetBool("isWalk", true);
            transform.localScale = new Vector3(1, 1, 1) * 1.3f;
        }
        else
        {
            anim.SetBool("isWalk", false);
            transform.localScale = new Vector3(1, 1, 1) * 1f;
        }
    }
}
