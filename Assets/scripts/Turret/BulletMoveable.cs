using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BulletMoveable : MonoBehaviour
{

    [SerializeField] public float speed = 1f;
    private const float maxTimeToLive = 20f;
    private Collider _collider;
    private bool onExit;

    private float myTime;
	// Use this for initialization
	void Start ()
	{
        _collider = GetComponent<Collider>();
        Reset();  
	}
	
	// Update is called once per frame
	void Update ()
	{
	    myTime += Time.deltaTime;
	    if (myTime > maxTimeToLive)
	    {
	        Delete();
	    } else if (!onExit && myTime > 0.1f)
	    {
	        onExit = true;
	        _collider.isTrigger = false;
	    }

        
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (onExit)
        {
            Delete();
        }
    }

    void OnTriggerEnter(Collider enemyCollider)
    {
        if (onExit && enemyCollider.CompareTag("Player"))
        {
            AnalyticsHelper.LogSceneRestartEvent(SceneManager.GetActiveScene().name, AnalyticsHelper.PlayerDeath.bulletCollision);
            enemyCollider.GetComponent<PlayerDie>().Die();
        }
    }

    public void Reset()
    {
        myTime = 0f;
        _collider.isTrigger = true;
        onExit = false;
    }

    public void Delete()
    {
        foreach (var VARIABLE in GetComponents<MonoBehaviour>())
        {
            VARIABLE.enabled = false;
        }

        foreach (var VARIABLE in GetComponents<Collider>())
        {
            VARIABLE.enabled = false;
        }

        foreach (var VARIABLE in GetComponents<Renderer>())
        {
            VARIABLE.enabled = false;
        }

        GetComponent<TrailRenderer>().enabled = true;
    }
}
