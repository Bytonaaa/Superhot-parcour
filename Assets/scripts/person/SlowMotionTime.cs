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
	}
	
	// Update is called once per frame
	void Update () {
	    if (controller.isGrounded && controller.velocity.sqrMagnitude <= minSpeed)
	    {
	        Time.timeScale = mainTimeScale * timeScale;
	        //Time.fixedDeltaTime = 0.02f*Time.timeScale;
	    }
	    else
	    {
	        Time.timeScale = mainTimeScale;
	        //Time.fixedDeltaTime = 0.02f*Time.timeScale;
	    }
	}
}
