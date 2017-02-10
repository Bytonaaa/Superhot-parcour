using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{


    [SerializeField] private float minYPos = -10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (transform.position.y < minYPos)
	    {
	        Die();
	    }
	}


    public void Die()
    {
        GameController.RestartGame();
    }
}
