using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverScreenScript : MonoBehaviour
{
  public Button restartGameButton;
  [SerializeField] private Canvas gameOverCanvas;

  [SerializeField] private Text finalScoreText;
  [SerializeField] private gameManager gameManager;



  void Start()
  {
    Button btn = restartGameButton.GetComponent<Button>();
    btn.onClick.AddListener(restartGame);
  }
  public void gameOver()
  {
    finalScoreText.text = "Score: " + gameManager.score;

    gameOverCanvas.gameObject.SetActive(true);

  }
  void restartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

  }
  public void mainMenu()
  {
    SceneManager.LoadScene("Main Menu");

  }


}
