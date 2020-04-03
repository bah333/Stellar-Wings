using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Maximum health and damage values, these will scale up
    public float maxHealth;
    public float maxDamage;

    // Current health and damage at whatever time
    float currentHealth;
    float currentDamage;

    // Values to scale health and damage
    public float scaleHealth;
    public float scaleDamage;

    // Countdown timer until health and damage is scaled up
    float timeUntilScale;
    public float baseTimeAmount;

    // Damage taken when colliding with an asteroid or some other source
    public float asteroidDamage;
    public float randomDamage;


    // For global variables
    GameObject globalObject;
    GlobalVariables globVars;

    Transform thisEnemy;

    GameObject leftTurret;
    GameObject rightTurret;

    float invincibleDuration;
    bool invincible;

    public bool inRange;
    public float fireRate;

    void Awake()
    {
        maxHealth = 1000.0f;
        maxDamage = 50.0f;

        currentHealth = maxHealth;
        currentDamage = maxDamage;

        scaleHealth = 0.25f;
        scaleDamage = 0.10f;

        baseTimeAmount = 120.0f;
        timeUntilScale = baseTimeAmount;

        // asteroidDamage = 100.0f;
        asteroidDamage = maxHealth * 0.20f;
        randomDamage = maxHealth * 0.10f;

        globalObject = GameObject.FindWithTag("GlobalVariables");
        globVars = globalObject.GetComponent<GlobalVariables>();

        thisEnemy = this.GetComponent<Transform>();

        leftTurret = transform.Find("leftTurret").gameObject;
        rightTurret = transform.Find("rightTurret").gameObject;

        invincibleDuration = 1.0f;
        invincible = true;

        inRange = false;
        fireRate = 0.5f;
    }

    void Update()
    {


        // Spawn invincibility, only against asteroids for 1 second 
        if (invincible)
        {
            invincibleDuration -= Time.deltaTime;

            if (invincibleDuration <= 0.0f)
                invincible = false;
        }

        // If either the current health or damage somehow exceed the max
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentDamage > maxDamage)
            currentDamage = maxDamage;

        // Countdown every update
        timeUntilScale -= Time.deltaTime;

        if (timeUntilScale <= 1.0f)
            invincible = false;

        // Once countdown reaches zero, upgrade
        if (timeUntilScale <= 0.0f)
        {
            upgradeEnemyStats();
            timeUntilScale = baseTimeAmount;
        }

        // status set in EnemyMovement.cs
        if (inRange)
            StartCoroutine(fireMG());

    }

    void OnCollisionEnter(Collision col)
    {
        if (!invincible)
        {
            // Asteroid collision, take damage
            if (col.gameObject.tag == "Asteroid")
                currentHealth -= asteroidDamage;

            else
              //  currentHealth -= randomDamage;

            Debug.Log(currentHealth);
            //  Destroyed if health is zero, and add to global shipScrap count
            if (currentHealth <= 0.0f)
            {
                // globVars.shipScrapsAmount += 1.0;
                Destroy(this.gameObject);
            }

        }
    }

    // Function to receive damage from source
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        //  Destroyed if health is zero, and add to global shipSrap count
        if (currentHealth <= 0.0f)
        {
            Debug.Log("I ded");
            globVars.shipScrapsAmount += 1.0;
            Destroy(this.gameObject);
        }
    }


    void upgradeEnemyStats()
    {
        // Calculate scaled values from max values
        float healthUp = maxHealth * scaleHealth;
        float damageUp = maxDamage * scaleDamage;

        // Add scaled values to new max values
        maxHealth += healthUp;
        maxDamage += damageUp;
        asteroidDamage = maxHealth * 0.20f;
        randomDamage = maxHealth * 0.10f;

        // If we want to add scaled values to current health and damage
        /*
        currentHealth += healthUp;
        currentDamage += damageUp;
        */
    }

    IEnumerator fireMG()
    {
        Debug.Log("Pew Pew!");
        Debug.Log(leftTurret);
        Debug.Log(rightTurret);
        yield return new WaitForSeconds(fireRate);
    }

}
