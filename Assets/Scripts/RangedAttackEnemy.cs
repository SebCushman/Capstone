using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackEnemy : MonoBehaviour
{
    public GameObject owner;
    public float damage = 1.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
        //if (collision.gameObject.tag == "Enemy")
        //{
            
        //}
    }
}
