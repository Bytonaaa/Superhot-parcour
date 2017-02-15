using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToLevelEnd : MonoBehaviour
{

    [SerializeField] private float _levelTime;

    public float LevelTime
    {
        get { return _levelTime; }
        private set { _levelTime = value; }
    }

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    LevelTime -= Time.deltaTime;
	    if (LevelTime <= 0f)
	    {
	        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDie>().Die();
	    }
	}
}
