using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;

    public GameObject deathEffect;
    public HealthBar healthBar;

    private GameManager gameManager;

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameManager = FindObjectOfType<GameManager>(); // Get reference to the GameManager
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthBar.SetHealth(health);

        StartCoroutine(DamageAnimation());

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        // Increase health by the specified amount, but ensure it doesn't exceed the maximum health
        health = Mathf.Min(health + amount, maxHealth);

        // Update the health bar to reflect the healed health
        healthBar.SetHealth(health);
    }

    void Die()
    {
        SceneManager.LoadScene("Pantalla_muerte");
    }

    IEnumerator DamageAnimation()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 0;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);

            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 1;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}
