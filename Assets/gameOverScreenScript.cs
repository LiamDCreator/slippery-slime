using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverScreenScript : MonoBehaviour
{
    public Button restartGameButton;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            Button btn = restartGameButton.GetComponent<Button>();
        btn.onClick.AddListener(restartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      void restartGame()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
