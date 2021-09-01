using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int level = 1;
    public int maxHealth = 10;
    public int health;
    public float speed = 5f;
    public bool isRanged = false;

    public int xpValue = 10;

    void Start()
    {
        health = maxHealth;
        xpValue = level * xpValue;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "RangedAttack")
        {
            TakeDamage((int)other.gameObject.GetComponent<RangedAttack>().damage);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MeleeAttack")
        {
            TakeDamage((int)FindObjectOfType<Player>().meleeDamage);
        }
    }

    void TakeDamage(int damage)
    {
        //FindObjectOfType<AudioManager>().Play("Hit");
        health -= damage;
        //Hurt Animation

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<Player>().currentXP += 1;
        Destroy(gameObject);
    }
}
