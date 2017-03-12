using System;
using UnityEngine.SceneManagement;

public class GameController
{
    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [Obsolete("Method is deprecated, please use SceneLoader.LoadScene() instead.", true)]
    public static void GoToMain()
    {
        SceneLoader.LoadScene("main");
    }

    [Obsolete("Method is deprecated, please use SceneLoader.LoadScene() instead.", true)]
    public static void LoadLevel(string level)
    {
        SceneLoader.LoadScene(level);
    }
}
