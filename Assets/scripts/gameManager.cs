using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private Text scoreText;

    // Method to increment the score
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score); // Log the updated score
        if (scoreText != null)
        {
            scoreText.text = "" + score; // Update the text component
        }
    }
}
