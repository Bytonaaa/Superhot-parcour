using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class WallButton : MonoBehaviour
{

    [SerializeField] private MoveableBlock target;
	
    // Use this for initialization
	void Start ()
	{
	    GetComponent<Collider>().isTrigger = true;
	}

    void OnTriggerStay(Collider collider)
    {
        if (target && Input.GetKeyDown(KeyCode.E) && collider.CompareTag("Player"))
        {
            target.StartMove();
        }
    }
}
