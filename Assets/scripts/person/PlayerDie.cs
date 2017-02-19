using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviour
{
    private const float minYPos = -100f;

    private bool died;

    public bool Died
    {
        get { return died; }
    }

    // Use this for initialization
	void Start ()
	{
	    died = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (died)
	    {

	        if (Input.GetKeyDown(KeyCode.R))
	        {
                GameController.RestartGame();
            }
	        return;
	    }


	    if (transform.position.y < minYPos)
	    {
            AnalyticsHelper.LogSceneRestartEvent(SceneManager.GetActiveScene().name, AnalyticsHelper.PlayerDeath.fallthrough);
            Die();
	    }
	}




    public void Die()
    {
        if (!died)
        {
            died = true;
            UIController.Controler.onDie();
            if (BackgroundMusicControll.GetInstance != null)
            {
                BackgroundMusicControll.GetInstance.onDieVolume();
            }
            GetComponent<FirstPersonController>().enabled = false;
            
        }
    }

    public void OnDestroy()
    {
        if (BackgroundMusicControll.GetInstance != null)
        {
            BackgroundMusicControll.GetInstance.resetVolume();
        }
    }
}
