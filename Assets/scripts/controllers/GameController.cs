using System;
using System.Collections;
using System.Collections.Generic;
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
        try
        {
            SceneManager.LoadScene(level);
        } catch(Exception)
        {
            GoToMain();
        }
    }
}
