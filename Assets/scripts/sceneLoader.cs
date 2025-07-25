using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    [SerializeField] private string BasicLevel;
    [SerializeField] private string ClashLevel;

    // Call this method from the button's OnClick() event
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(BasicLevel))
        {
            SceneManager.LoadScene(BasicLevel);
            Time.timeScale = 1f;

        }
        else
        {
            Debug.LogWarning("Scene name is not set in SceneLoader!");
        }
    }
    public void LoadClashScene()
    {
        if (!string.IsNullOrEmpty(ClashLevel))
        {
            SceneManager.LoadScene(ClashLevel);
            Time.timeScale = 1f;

        }
        else
        {
            Debug.LogWarning("Scene name is not set in SceneLoader!");
        }
    }
}
