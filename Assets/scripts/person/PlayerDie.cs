using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviour
{
    private const float minYPos = -100f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (transform.position.y < minYPos)
	    {
            AnalyticsHelper.LogSceneRestartEvent(SceneManager.GetActiveScene().name, AnalyticsHelper.PlayerDeath.fallthrough);
            Die();
	    }
	}


    public void Die()
    {
        GameController.RestartGame();
    }
}
