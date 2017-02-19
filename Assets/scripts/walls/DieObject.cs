using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DieObject : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var temp = GetComponent<Collider>();
	    temp.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            AnalyticsHelper.LogSceneRestartEvent(SceneManager.GetActiveScene().name, AnalyticsHelper.PlayerDeath.collision);
            collider.GetComponent<PlayerDie>().Die();
        }
    }
}
