using System.Reflection;
using UnityEngine;

public class nukePowerUpScript : MonoBehaviour
{

    public gameManager gameManager;
    public float minimumDash;
    public float maximumDash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float spawnpos = 7f;
        float leftorRight;
        Vector3 newPosition = transform.position;
        newPosition.y = spawnpos;
        transform.position = newPosition;

        // Find the gameManager in the scene
        gameManager = FindFirstObjectByType<gameManager>();

        // Dash to the top right
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
                leftorRight = -1;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
                leftorRight = 1;

            }
            Vector2 dashDirection = new Vector2(leftorRight, 1f).normalized; // Top right direction
            float dashForce = Random.Range(minimumDash, maximumDash); // Adjust this value to control dash speed
            rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillAllEnemies();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);

        }
    }

    private void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
            gameManager.score += 1;
            gameManager.scoreText.text = "" + gameManager.score;

        }

    }
}
