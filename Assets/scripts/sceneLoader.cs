using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    [SerializeField] private string BasicLevel;

    // Call this method from the button's OnClick() event
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(BasicLevel))
        {
            SceneManager.LoadScene(BasicLevel);
        }
        else
        {
            Debug.LogWarning("Scene name is not set in SceneLoader!");
        }
    }
}
