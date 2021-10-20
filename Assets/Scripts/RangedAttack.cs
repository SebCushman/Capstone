using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject owner;
    public float damage = 1.0f;

    public bool isWave = false;

    private void FixedUpdate()
    {
        if(gameObject.tag == "AOE")
        {
            transform.localScale += new Vector3(0.15f, 0.15f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isWave)
        {
            Destroy(this.gameObject);
        }
    }

    
}
