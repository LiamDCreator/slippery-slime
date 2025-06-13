using UnityEngine;

public class straightEnemy : MonoBehaviour
{
    public float moveSpeed;
    public float originalmovespeed;

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
        originalmovespeed = moveSpeed;
        // Ignore collisions with other enemies
        // IgnoreEnemyCollisions();
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

    public void stopEnemy()
    {
        moveSpeed = 0;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop all movement
            rb.constraints = RigidbodyConstraints2D.FreezePosition; // Freeze X and Y position
        }
    }
}