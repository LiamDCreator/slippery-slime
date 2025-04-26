using UnityEngine;

public class straightEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    void Start()
    {
        // Rotate the enemy to face the correct direction based on its spawn position
        if (transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
        }

        // Ignore collisions with other enemies
        IgnoreEnemyCollisions();
    }

    void Update()
    {
        // Move the enemy forward based on its current rotation
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // Destroy the enemy if it moves out of bounds
        if (transform.position.x < -30 || transform.position.x > 30)
        {
            Destroy(gameObject);
        }
    }

    private void IgnoreEnemyCollisions()
    {
        Collider2D[] allEnemies = FindObjectsOfType<Collider2D>();
        Collider2D thisCollider = GetComponent<Collider2D>();

        foreach (Collider2D enemyCollider in allEnemies)
        {
            if (enemyCollider != thisCollider && enemyCollider.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(thisCollider, enemyCollider);
            }
        }
    }
}