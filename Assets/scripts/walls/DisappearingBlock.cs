using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DisappearingBlock : MonoBehaviour
{


    [SerializeField] private bool collisionWithPlayer;
    [SerializeField] private float VisionTime = 5f;
    [SerializeField] private bool Regenerate;
    [SerializeField] private float UnVisionTime = 1f;


    private bool isWaited;
	// Use this for initialization
	void Start ()
	{
	    if (!collisionWithPlayer)
	    {
	        StartCoroutine(enableAfter(VisionTime, false));
	    }
	}

    void OnTriggerEnter(Collider collider)
    { 
        if (!isWaited && collisionWithPlayer && collider.CompareTag("Player"))
        {
            StartCoroutine(enableAfter(VisionTime, false));
        }
    }

    private void SetActive(bool flag)
    {
        foreach (var VARIABLE in GetComponents<Behaviour>())
        {
            VARIABLE.enabled = flag;
        }

        foreach (var VARIABLE in GetComponents<Collider>())
        {
            VARIABLE.enabled = flag;
        }

        foreach (var VARIABLE in GetComponents<Renderer>())
        {
            VARIABLE.enabled = flag;
        }

        this.enabled = flag;
    }

    IEnumerator enableAfter(float time, bool enable)
    {
        isWaited = true;
        yield return new WaitForSeconds(time);
        SetActive(enable);
        isWaited = false;

        if (!Regenerate) yield break;

        if (!collisionWithPlayer)
        {
            StartCoroutine(enableAfter(enable ? VisionTime : UnVisionTime, !enable));
        }
        else if (!enable)
        {
            StartCoroutine(enableAfter(UnVisionTime, true));
        }

        
    }

    
}
