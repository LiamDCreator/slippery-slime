using UnityEngine;

public class arrowScript : MonoBehaviour
{

    private Rigidbody2D rb;
    private Collider2D arrowCollider;
    [SerializeField] private int arrowStrength;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float collisionDelay = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<Collider2D>();

        // Disable collision initially
        arrowCollider.enabled = false;

        // Add force to move the arrow
        rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Impulse);

        // Enable collision after delay
        Invoke("EnableCollision", collisionDelay);
    }

    private void EnableCollision()
    {
        if (arrowCollider != null)
        {
            arrowCollider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the arrow to match its velocity direction
        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the arrow hit an enemy
        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            // Damage the enemy by reducing its strength
            enemy.strength -= arrowStrength;

            // If enemy strength drops to 0 or below, destroy the enemy
            if (enemy.strength <= 0)
            {
                Destroy(collision.gameObject);
            }
        }

        // Destroy the arrow regardless of what it hit
        Destroy(gameObject);
    }
}
