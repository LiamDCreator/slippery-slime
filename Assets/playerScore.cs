using UnityEngine;

public class playerScore : MonoBehaviour
{
    private gameManager gameManager;

    private void Start()
    {
        // Find the gameManager in the scene
        gameManager = FindObjectOfType<gameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Check if the object is tagged as "Enemy"
        {
            EnemyScore enemyScore = other.GetComponent<EnemyScore>();
            if (enemyScore != null && !enemyScore.HasBeenScored)
            {
                if (gameManager != null)
                {
                    gameManager.AddScore(1); // Increment the score via gameManager
                }
                enemyScore.HasBeenScored = true; // Mark the enemy as scored
            }
        }
    }
}