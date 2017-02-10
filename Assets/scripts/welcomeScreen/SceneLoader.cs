using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour {

    [SerializeField]
    private string sceneToLoad;
	// Use this for initialization
	void Start () {
		
	}
	
	public void LoadScene()
    {
        if (sceneToLoad == null)
        {
            throw new System.Exception("No scene");
        }
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
