using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public HealthBar healthBar;
    public TextMeshProUGUI gameOver;

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
        if (Input.GetKeyDown(KeyCode.U))
        {
            ReciveHealth(100);
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

    public void ReciveHealth(int heal)
    {
        currentHealth += heal;
        if (currentHealth >= 100)
        {
            currentHealth = 100;
        }
        healthBar.SetHealth(currentHealth);
    }
    public void IsDead()
    {
        if (isDead == true)
        {
            Destroy(gameObject);
            GameOverScreen();
        }
    }
    public void GameOverScreen()
    {
        //Game Over Screen
        if (gameOver != null)
            gameOver.SetText("Game Over");
    }
}
