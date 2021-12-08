using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject wavePrefab;
    public GameObject aoePrefab;
    public GameObject melee;

    public int level = 1;
    public int currentXP = 0;
    public int toLevel = 0;

    public int maxHealth = 20;
    public int health;
    public HealthBar healthBar;
    public ExperienceBar xpBar;
    public AbilityBar waveBar;
    public AbilityBar aoeBar;
    public AbilityBar healBar;

    public Quest quests;
    
    public float moveSpeed = 5.0f;
    public float meleeDamage = 2.0f;
    public float rangedDamage = 2.0f;
    public bool isMelee = false;
    public bool isDialogue = false;

    public float fireRate;
    public float waveRate;
    public float aoeRate;
    public float healRate;
    public float meleeHold;
    public float cooldown;
    public float waveCooldown;
    public float aoeCooldown;
    public float healCooldown;

    public Rigidbody2D rb;
    public Rigidbody2D rbFirePoint;
    public Camera cam;
    public Animator animator;

    Vector2 movement;
    Vector2 mousePos;

    public int lastDirection = 3;

    public TMP_Text meleeDamText;
    public TMP_Text rangedDamText;
    public TMP_Text HPText;
    public TMP_Text XPText;

    private void Start()
    {
        health = maxHealth;
        healthBar.SetmaxHealth(maxHealth);
        xpBar.SetmaxXP(10);
        xpBar.levelText.text = level.ToString();
        melee.SetActive(false);
        fireRate = 0.25f;
        cooldown = fireRate;
        waveRate = 2.0f;
        waveCooldown = waveRate;
        waveBar.SetCooldown(waveRate);
        aoeRate = 5.0f;
        aoeCooldown = aoeRate;
        aoeBar.SetCooldown(aoeRate);
        healRate = 5.0f;
        healCooldown = healRate;
        healBar.SetCooldown(healRate);

        meleeDamText.text = $"Melee Damage: {meleeDamage}";
        rangedDamText.text = $"Ranged Damage: {rangedDamage}";
        HPText.text = $"HP: {health}/{maxHealth}";
        XPText.text = $"XP: {currentXP}/{toLevel}";

        for(int i = 1; i < 20; i++)
        {
            LevelUp();
        }
    }

    void Update()
    {
        xpBar.SetXP(currentXP);
        waveBar.SetCurrentTimer(waveCooldown);
        aoeBar.SetCurrentTimer(aoeCooldown);
        healBar.SetCurrentTimer(healCooldown);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        HPText.text = $"HP: {health}/{maxHealth}";
        XPText.text = $"XP: {currentXP}/{toLevel}";

        if (movement.x > 0.01f)
        {
            lastDirection = 2;
            //animator.SetBool("Right", true);
        }
        else if(movement.x < -0.01f)
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

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (fireRate > cooldown)
        {
            cooldown += Time.deltaTime;
        }

        if (waveRate > waveCooldown)
        {
            waveCooldown += Time.deltaTime;
        }

        if (aoeRate > aoeCooldown)
        {
            aoeCooldown += Time.deltaTime;
        }

        if (healRate > healCooldown)
        {
            healCooldown += Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //melee.SetActive(true);
            if (!isMelee)
            {
                meleeHold = 0.1f;
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

        if (Input.GetKeyDown(KeyCode.Alpha1) && !(waveRate > waveCooldown))
        {
            GameObject wave = Instantiate(wavePrefab, firePoint.position, firePoint.rotation);
            wave.GetComponent<RangedAttack>().owner = this.gameObject;
            wave.GetComponent<RangedAttack>().damage *= rangedDamage;
            ShootWave(20, Color.yellow, wave);
            waveCooldown = 0;
            Destroy(wave, 0.25f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !(aoeRate > aoeCooldown))
        {
            GameObject aoe = Instantiate(aoePrefab, mousePos, firePoint.rotation);
            aoe.GetComponent<RangedAttack>().owner = this.gameObject;
            aoe.GetComponent<RangedAttack>().damage *= rangedDamage;
            ShootAOE(Color.yellow, aoe);
            //aoe.transform.localScale += new Vector3(0.2f, 0.2f, 0f);
            aoeCooldown = 0;
            Destroy(aoe, 1f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && !(healRate > healCooldown))
        {
            HealDamage((int)(maxHealth * .25));
            FindObjectOfType<AudioManager>().Play("Heal");
            healCooldown = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentXP += 100;
        }

        if (currentXP >= toLevel)//xpBar.slider.maxValue)
        {
            LevelUp();
        }

        
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        //rbFirePoint.MovePosition(rbFirePoint.position + movement * moveSpeed * Time.fixedDeltaTime);
        rbFirePoint.position = rb.position;

        Vector2 direction = mousePos - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        rbFirePoint.rotation = angle;
        //firePoint.RotateAround(this.transform.position, new Vector3(0f, 1f, 0f), angle);
    }

    void Shoot(float bulletForce, Color color, GameObject bullet)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

        spriteRenderer.color = color;

        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        //rb.AddForce(mousePos * bulletForce, ForceMode2D.Impulse);

        FindObjectOfType<AudioManager>().Play("Fire");
    }

    void ShootWave(float waveForce, Color color, GameObject wave)
    {
        Rigidbody2D rb = wave.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = wave.GetComponent<SpriteRenderer>();

        spriteRenderer.color = color;

        rb.AddForce(firePoint.up * waveForce, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play("Wave");
    }

    void ShootAOE(Color color, GameObject aoe)
    {
        Rigidbody2D rb = aoe.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = aoe.GetComponent<SpriteRenderer>();

        spriteRenderer.color = color;

        FindObjectOfType<AudioManager>().Play("AOE");

        //aoe.gameObject.transform.localScale;

        //rb.AddForce(firePoint.up * waveForce, ForceMode2D.Impulse);
    }

    void LevelUp()
    {
        //currentXP -= (int)xpBar.slider.maxValue;
        currentXP = 0;
        xpBar.SetmaxXP((int)(xpBar.slider.maxValue * 1.25f));
        toLevel = (int)xpBar.slider.maxValue;
        level++;
        meleeDamage += 3f;
        rangedDamage += 3f;
        maxHealth = (int)(maxHealth * 1.25f);
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
        healthBar.SetmaxHealth(maxHealth);
        xpBar.levelText.text = level.ToString();

        meleeDamText.text = $"Melee Damage: {meleeDamage}";
        rangedDamText.text = $"Ranged Damage: {rangedDamage}";
        HPText.text = $"HP: {health}/{maxHealth}";
        XPText.text = $"XP: {currentXP}/{toLevel}";
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
            FindObjectOfType<AudioManager>().Play("Melee");
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

    void HealDamage(int damage)
    {
        //FindObjectOfType<AudioManager>().Play("Hit");
        health += damage;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.SetHealth(health, maxHealth);

        //Heal Animation
    }

    void Die()
    {
        Destroy(gameObject);
        //Display Lose screen
        FindObjectOfType<Game>().GameOver();
    }
}
