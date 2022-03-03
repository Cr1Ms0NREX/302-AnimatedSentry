using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    // Start is called before the first frame update
    public HealthSystem(int healthMax)
    {
        this.maxHealth = healthMax;
        currentHealth = healthMax;    
    }

    public int GetHealth()
    {
        return currentHealth;
    }
    public float GetHealthPercent()
    {
        return (float) currentHealth / maxHealth;
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void GainHealth(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth >= 100)
        {
            currentHealth = 100;
        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

}
