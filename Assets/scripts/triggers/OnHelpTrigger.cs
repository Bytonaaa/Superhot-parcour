using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHelpTrigger : MonoBehaviour
{

    [SerializeField] private string HelpText;
    [SerializeField] private new Collider collider;
    [SerializeField] private bool DestroyOnRead;

	// Use this for initialization
	void Start () {
	    if (collider == null && (collider = GetComponent<Collider>()) == null)
	    {
	        throw new Exception("No trigger exist");
	    }

	    collider.isTrigger = true;
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            UIController.Controler.DisplayHelpText(HelpText);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            UIController.Controler.DisableHelpText();
            if (DestroyOnRead)
            {
                Destroy(this);
            }
        }
    }


}
