using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionTime : MonoBehaviour
{
    [SerializeField] private float minSpeed = 0f;
    [SerializeField] private float timeScale = 0.1f;


    private CharacterController controller;
    private float mainTimeScale;
    void Start ()
	{
	    controller = GetComponent<CharacterController>();
	    if (controller == null)
	    {
	        throw new Exception("No characterControoller");
	    }
        mainTimeScale = Time.timeScale;
        slowed = false;
	}
	
	// Update is called once per frame

    private bool slowed = false;

	void Update () {
	    if (controller.isGrounded && controller.velocity.sqrMagnitude <= minSpeed)
	    {
	        if (!slowed)
	        {
	            Time.timeScale = mainTimeScale*timeScale;
	            Time.fixedDeltaTime = 0.02f*mainTimeScale*timeScale;
	            slowed = true;
	        }
	    }
	    else
	    {
	        if (slowed)
	        {
                
	            Time.timeScale = mainTimeScale;
	            Time.fixedDeltaTime = 0.02f*mainTimeScale;
	            slowed = false;
	        }
	    }
    }


    void OnDestroy()
    {
        if (slowed)
        {

            Time.timeScale = mainTimeScale;
            Time.fixedDeltaTime = 0.02f * mainTimeScale;
        }
    }
}
