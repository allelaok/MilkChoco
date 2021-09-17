using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Na_Grenade1 : MonoBehaviour
{
    public GameObject grenade;
    public GameObject effect;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        grenade.SetActive(false);
        effect.SetActive(true);

        RaycastHit[] rays = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rays)
        {
            if (hitObj.transform.name.Contains("SM"))
            {
                hitObj.transform.GetComponent<SM_Enemy_Hp>().Damaged(0.1f);
            }
            else if (hitObj.transform.name.Contains("Na"))
            {
                hitObj.transform.GetComponent<KH_EnemyHP>().Damaged(0.1f);
            }
            else if (hitObj.transform.name.Contains("KH"))
            {
                hitObj.transform.GetComponent<SM_Enemy_Hp>().Damaged(0.1f);
            }

        }



    }
}
