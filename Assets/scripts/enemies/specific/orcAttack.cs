using UnityEngine;
using System.Collections;

public class orcAttack : MonoBehaviour
{
    [SerializeField] private GameObject hitbox; // Reference to the hitbox GameObject
    [SerializeField] private float attackDelay; // Delay before the attack
    [SerializeField] private float hitboxActiveTime; // Duration the hitbox stays active
    [SerializeField] private float minimumAttackRate; // Minimum time between attacks
    [SerializeField] private float maximumAttackRate; // Maximum time between attacks
    [SerializeField] private Color prepareAttackColor = Color.red; // Color during attack preparation
    private Color originalColor; // To store the original color of the GameObject
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    private float attackRate; // Current time between attacks

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Store the original color
        }

        if (hitbox != null)
        {
            hitbox.SetActive(false); // Ensure the hitbox is initially disabled
        }

        attackRate = Random.Range(minimumAttackRate, maximumAttackRate); // Set initial random attack rate
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            // Wait for the attack rate duration
            yield return new WaitForSeconds(attackRate - attackDelay);

            prepareAttack();

            // Wait for the attack delay
            yield return new WaitForSeconds(attackDelay);

            attack();

            // Randomize the attack rate for the next attack
            attackRate = Random.Range(minimumAttackRate, maximumAttackRate);
        }
    }

    private void prepareAttack()
    {
        Debug.Log("Preparing to attack!");

        // Change the color of the GameObject to indicate attack preparation
        if (spriteRenderer != null)
        {
            spriteRenderer.color = prepareAttackColor;
        }
    }

    private void attack()
    {
        Debug.Log("Attacking!");

        // Enable the hitbox
        if (hitbox != null)
        {
            hitbox.SetActive(true);
            StartCoroutine(DisableHitboxAfterDelay());
        }

        // Reset the color back to the original
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private IEnumerator DisableHitboxAfterDelay()
    {
        // Wait for the hitbox active time
        yield return new WaitForSeconds(hitboxActiveTime);

        // Disable the hitbox
        if (hitbox != null)
        {
            hitbox.SetActive(false);
        }
    }
}