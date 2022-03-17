using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public HealthBar healthBar;

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(100);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet")
        {
            TakeDamage(40);
        }
        if (collision.tag == "Player")
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            IsDead();
        }
        healthBar.SetHealth(currentHealth);
    }

    public void IsDead()
    {
        if (isDead == true)
        {
            Destroy(gameObject);
        }
    }
}
