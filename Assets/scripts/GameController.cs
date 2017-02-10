using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController {

    private GameController()
    {
        
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void GoToMain()
    {
        SceneManager.LoadScene("main");
    }

    public static void LoadLevel(string level)
    {
        var temp = SceneManager.GetSceneByName("level");
        if (temp.IsValid())
        {
            SceneManager.LoadScene(temp.name);
        }
        else
        {
            GoToMain();
        }
    }
}
