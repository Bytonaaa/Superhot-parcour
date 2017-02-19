using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {

    [SerializeField]private string sceneToLoad;
	// Use this for initialization
	void Update () {
	    if (Input.anyKeyDown)
	    {
	        LoadScene();
	    }
	}
	
	public void LoadScene()
    {
        AnalyticsHelper.LogSceneLoadEvent(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
