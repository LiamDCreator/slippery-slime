using UnityEngine;

public class nukePowerUpScript : MonoBehaviour
{

    public gameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the gameManager in the scene
        gameManager = FindFirstObjectByType<gameManager>();
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
