using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthFINAL : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public GameObject deathEffect;
    public bool isInvulnerable = false;
    public HealthBar healthBar;

    private Animator animator;
    private bool isDead = false;

    private BoxCollider2D boxCollider2D;

    private GameManager gameManager;

    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        gameManager = FindObjectOfType<GameManager>();

    }

    void Update()
    {
        // Ensure the health bar does not rotate with the enemy
        if (healthBar != null)
        {
            healthBar.transform.rotation = Quaternion.identity;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead)
            return;

        health -= damage;
        healthBar.SetHealth(health);

        // Trigger hit animation
        animator.SetTrigger("HurtBoss");

        if (health <= 0)
        {
            health = 0; // Ensure health does not go below 0
            healthBar.SetHealth(health); // Update health bar to show 0
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        // Trigger death animation
        animator.SetBool("IsDeadBoss", true);

        // Delay the destruction to allow death animation to play
        StartCoroutine(DestroyAfterAnimation());

        gameObject.SetActive(false); // Deactivate the enemy
        gameManager.EnemyKilled();
    }

    IEnumerator ShrinkColliderAfterDelay()
    {
        // Wait for a certain duration before shrinking the collider
        yield return new WaitForSeconds(0.3f); // Adjust the delay time as needed

        // Shrink the size of the BoxCollider2D
        boxCollider2D.size = new Vector2(0.1f, 0.1f); // Adjust the size as needed

        // Optionally, adjust the position to keep the collider above the ground
        Vector2 newPosition = boxCollider2D.offset;
        newPosition.y += boxCollider2D.size.y * 0.5f;
        boxCollider2D.offset = newPosition;
    }

    IEnumerator DestroyAfterAnimation()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Start shrinking the collider after a delay
        StartCoroutine(ShrinkColliderAfterDelay());

        // Wait an additional 5 seconds
        yield return new WaitForSeconds(5f);

        // Destroy the game object
        Destroy(gameObject);
    }
}
