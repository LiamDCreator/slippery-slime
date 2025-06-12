using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private Text scoreText;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    private bool isPaused = false;
    private bool isSettings = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauzeGame();
        }
    }



    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score); // Log the updated score
        if (scoreText != null)
        {
            scoreText.text = "" + score; // Update the text component
        }
    }
    public void pauzeGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pauses the game
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f; // Resumes the game
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);
        }
    }
    public void openSettings()
    {
        isSettings = !isSettings;
        if (isSettings)
        {
            settingsMenuUI.SetActive(true);
            pauseMenuUI.SetActive(false);
        }
        else
        {
            settingsMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);

        }
    }
}
