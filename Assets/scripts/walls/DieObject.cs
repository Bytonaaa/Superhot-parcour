using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            collider.GetComponent<PlayerDie>().Die();
        }
    }
}
