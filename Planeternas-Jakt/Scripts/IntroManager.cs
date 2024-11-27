using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    /// <summary>
    /// Checks for input to start the game and loads the gameplay scene if triggered.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadGameScene();
        }

        if (DancePadInputManager.instance != null && DancePadInputManager.instance.moveUp)
        {
            LoadGameScene();
        }
    }

    /// <summary>
    /// Loads the gameplay scene if it is available in the Build Settings.
    /// </summary>
    private void LoadGameScene()
    {
        if (Application.CanStreamedLevelBeLoaded("Gameplay"))
        {
            Debug.Log("Starting game...");
            SceneManager.LoadScene("Gameplay");
        }
        else
        {
            Debug.LogError("Gameplay scene is not in Build Settings!");
        }
    }
}
