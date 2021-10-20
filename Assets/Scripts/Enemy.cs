using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject melee;
    public Rigidbody2D rb;

    public int level = 1;
    public int maxHealth = 10;
    public int health;
    public float speed = 5f;
    public bool isRanged = false;
    public int attackRange;

    public float meleeDamage = 1.0f;
    public float rangedDamage = 1.0f;
    //public bool isMelee = false;

    public int xpValue = 10;

    public int detectionRange = 5;
    int minRange;
    Vector2 movement;

    public float fireRate = 2.0f;
    public float meleeHold;
    public float cooldown;

    GameObject target;
    void Start()
    {
        target = FindObjectOfType<Player>().gameObject;
        cooldown = fireRate;
        health = maxHealth;
        xpValue = level * xpValue;
        rb = this.GetComponent<Rigidbody2D>();
        if (isRanged)
        {
            attackRange = detectionRange - 1;
            minRange = 3;
        }
        else
        {
            attackRange = 2;
            minRange = 1;
        }
    }

    private void Update()
    {
        if ((target.transform.position - transform.position).magnitude <= detectionRange)
        {
            //Vector3 direction = (target.transform.position - transform.position).normalized;

            Vector3 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
            direction.Normalize();
            movement = direction;
        }
        else
        {
            movement = Vector2.zero;
        }

        if(fireRate > cooldown)
        {
            cooldown += Time.deltaTime;
        }

        if ((target.transform.position - transform.position).magnitude <= attackRange && !isRanged && !(fireRate > cooldown))
        {
            AttackM();
            cooldown = 0;
        }

        meleeHold -= Time.deltaTime;
        if (meleeHold <= 0)
        {
            melee.SetActive(false);
            meleeHold = 0;
        }

        if ((target.transform.position - transform.position).magnitude <= attackRange && isRanged && !(fireRate > cooldown))
        {
            AttackR();
            cooldown = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isRanged)
        {
            MoveRanged(movement);
        }
        else
        {
            MoveMelee(movement);
        }
        
    }

    public void MoveMelee(Vector2 direction)
    {
        //rb.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
        if ((FindObjectOfType<Player>().transform.position - transform.position).magnitude <= minRange)
        {
            rb.MovePosition((Vector2)transform.position - movement * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    public void MoveRanged(Vector2 direction)
    {
        if ((FindObjectOfType<Player>().transform.position - transform.position).magnitude <= minRange)
        {
            rb.MovePosition((Vector2)transform.position - movement * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    public void AttackM()
    {
        meleeHold = 0.5f;
        melee.SetActive(true);
    }

    public void AttackR()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<RangedAttack>().owner = this.gameObject;
        bullet.GetComponent<RangedAttack>().damage *= rangedDamage;
        Shoot(10, Color.green, bullet);
    }

    void Shoot(float bulletForce, Color color, GameObject bullet)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

        spriteRenderer.color = color;

        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
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

        if(collision.gameObject.tag == "RangedAttack" || collision.gameObject.tag == "AOE")
        {
            TakeDamage((int)collision.gameObject.GetComponent<RangedAttack>().damage);
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
        if (FindObjectOfType<Player>().quests.isActive)
        {
            FindObjectOfType<Player>().quests.goal.EnemyKilled();
            if (FindObjectOfType<Player>().quests.goal.IsFinished())
            {
                FindObjectOfType<Player>().currentXP += FindObjectOfType<Player>().quests.xpReward;
                FindObjectOfType<Player>().quests.Complete();
            }
        }
        Destroy(gameObject);
    }
}
