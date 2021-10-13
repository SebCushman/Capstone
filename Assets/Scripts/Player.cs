using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject melee;

    public int level = 1;
    public int currentXP = 0;

    public int maxHealth = 20;
    public int health;
    public HealthBar healthBar;
    public ExperienceBar xpBar;
    
    public float moveSpeed = 5.0f;
    public float meleeDamage = 2.0f;
    public float rangedDamage = 2.0f;
    public bool isMelee = false;
    public bool isDialogue = false;

    public float fireRate;
    public float meleeHold;
    public float cooldown;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    private void Start()
    {
        health = maxHealth;
        healthBar.SetmaxHealth(maxHealth);
        xpBar.SetmaxXP(10);
        melee.SetActive(false);
        fireRate = 0.5f;
    }

    void Update()
    {
        xpBar.SetXP(currentXP);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (fireRate > cooldown)
        {
            cooldown += Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //melee.SetActive(true);
            if (!isMelee)
            {
                meleeHold = 0.25f;
                isMelee = true;
                cooldown = 0;
            }
        }

        //if (Input.GetButtonUp("Fire1"))
        //{
        //    //melee.SetActive(true);
        //    if (isMelee)
        //    {
        //        isMelee = false;
        //    }
        //}

        if (melee != null && isMelee != melee.activeInHierarchy)
        {
            melee.SetActive(isMelee);
        }

        meleeHold -= Time.deltaTime;
        if (meleeHold <= 0)
        {
            melee.SetActive(false);
            isMelee = false;
            meleeHold = 0;
        }

        if (Input.GetButtonDown("Fire2") && !(fireRate > cooldown))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<RangedAttack>().owner = this.gameObject;
            bullet.GetComponent<RangedAttack>().damage *= rangedDamage;
            Shoot(30, Color.green, bullet);
            cooldown = 0;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(3);
        }

        if(currentXP >= xpBar.slider.maxValue)
        {
            LevelUp();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = angle;
    }

    void Shoot(float bulletForce, Color color, GameObject bullet)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

        spriteRenderer.color = color;

        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void LevelUp()
    {
        currentXP -= (int)xpBar.slider.maxValue;
        xpBar.SetmaxXP((int)xpBar.slider.maxValue * 2);
        meleeDamage *= 3f;
        rangedDamage *= 3f;
        maxHealth = (int)(maxHealth * 1.5f);
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "RangedAttack")
        {
            TakeDamage((int)other.gameObject.GetComponent<RangedAttack>().damage);
        }
        else if(other.gameObject.tag == "Enemy")
        {
            TakeDamage(2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MeleeAttack")
        {
            TakeDamage((int)FindObjectOfType<Enemy>().meleeDamage);
        }
    }

    void TakeDamage(int damage)
    {
        //FindObjectOfType<AudioManager>().Play("Hit");
        health -= damage;
        healthBar.SetHealth(health, maxHealth);

        //Hurt Animation

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        //Display Lose screen
        //FindObjectOfType<Game>().GameOver();
    }
}
