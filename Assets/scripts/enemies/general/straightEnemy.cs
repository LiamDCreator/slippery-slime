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
        // Set y velocity to 0 to stop jumping/falling
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }

        // Set the object's Y position to -3
        Vector3 pos = transform.position;
        pos.y = -3.2f;
        transform.position = pos;

    }
}