using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject melee;
    public Rigidbody2D rb;
    public Rigidbody2D rbFirePoint;

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

    public Animator animator;
    public int lastDirection = 3;

    void Start()
    {
        target = FindObjectOfType<Player>().gameObject;
        cooldown = fireRate;
        health = maxHealth;
        //xpValue = level * xpValue;
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
        for (int i = 1; i <= 15; i++)
        {
            LevelUp();
        }
    }

    private void Update()
    {
        if ((target.transform.position - transform.position).magnitude <= detectionRange)
        {
            //Vector3 direction = (target.transform.position - transform.position).normalized;

            Vector3 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rbFirePoint.rotation = angle;
            direction.Normalize();
            movement = direction;
        }
        else
        {
            movement = Vector2.zero;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x > 0.01f)
        {
            lastDirection = 2;
            //animator.SetBool("Right", true);
        }
        else if (movement.x < -0.01f)
        {
            lastDirection = 4;
            //animator.SetBool("Right", false);
        }
        else if (movement.y > 0.01f)
        {
            lastDirection = 1;
            //animator.SetBool("Up", true);
        }
        else if (movement.y < -0.01f)
        {
            lastDirection = 3;
            //animator.SetBool("Up", false);
        }

        switch (lastDirection)
        {
            case 1:
                animator.SetBool("Right", false);
                animator.SetBool("Up", true);
                break;
            case 2:
                animator.SetBool("Up", true);
                animator.SetBool("Right", true);
                break;
            case 3:
                animator.SetBool("Right", true);
                animator.SetBool("Up", false);
                break;
            case 4:
                animator.SetBool("Up", false);
                animator.SetBool("Right", false);
                break;
        }

        if (fireRate > cooldown)
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
            //rbFirePoint.MovePosition((Vector2)transform.position - movement * speed * Time.fixedDeltaTime);
            rbFirePoint.position = rb.position;
        }
        else
        {
            rb.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
            //rbFirePoint.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
            rbFirePoint.position = rb.position;
        }
    }

    public void MoveRanged(Vector2 direction)
    {
        if ((FindObjectOfType<Player>().transform.position - transform.position).magnitude <= minRange)
        {
            rb.MovePosition((Vector2)transform.position - movement * speed * Time.fixedDeltaTime);
            //rbFirePoint.MovePosition((Vector2)transform.position - movement * speed * Time.fixedDeltaTime);
            rbFirePoint.position = rb.position;
        }
        else
        {
            rb.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
            //rbFirePoint.MovePosition((Vector2)transform.position + movement * speed * Time.fixedDeltaTime);
            rbFirePoint.position = rb.position;
        }
    }

    public void AttackM()
    {
        meleeHold = 0.1f;
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
        FindObjectOfType<AudioManager>().Play("Fire");
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
            FindObjectOfType<AudioManager>().Play("Melee");
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

    void LevelUp()
    {
        meleeDamage += 3f;
        rangedDamage += 3f;
        maxHealth = (int)(maxHealth * 1.25f);
        health = maxHealth;
        xpValue = (int)(xpValue * 1.25f);
    }

    void Die()
    {
        FindObjectOfType<Player>().currentXP += xpValue;
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
