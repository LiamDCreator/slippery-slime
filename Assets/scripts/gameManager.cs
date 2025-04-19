using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] private int score = 0;

    // Method to increment the score
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score); // Log the updated score
    }
}
