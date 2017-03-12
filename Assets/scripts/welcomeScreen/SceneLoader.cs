using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static string sceneToLoad;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadScene();
        }
    }

    public static void LoadScene(string scene = "")
    {
        if (scene != "")
        {
            sceneToLoad = scene;
        }
        if (sceneToLoad == "" || !Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            int activeSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            string nextScenePath;
            if (activeSceneBuildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            {
                nextScenePath = SceneUtility.GetScenePathByBuildIndex(0);
                sceneToLoad = Path.GetFileNameWithoutExtension(nextScenePath);
            }
            else
            {
                nextScenePath = SceneUtility.GetScenePathByBuildIndex(activeSceneBuildIndex + 1);
                sceneToLoad = Path.GetFileNameWithoutExtension(nextScenePath);
            }
        }
        AnalyticsHelper.LogSceneLoadEvent(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        sceneToLoad = "";
    }
}
