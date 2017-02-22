using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitFromGame : MonoBehaviour
{


    [SerializeField] private Button ExitButton;

    private bool isExitOpen;

	// Use this for initialization
	void Start () {
	    if (ExitButton || (ExitButton = GetComponent<Button>()) == null)
	    {
	        var obj = GameObject.Find("Exit button");

	        if (obj != null)
	        {
	            ExitButton = obj.GetComponent<Button>();
	        }

	        if (!ExitButton)
	        {
	            throw new Exception("No exit button");
	        }
	    }

        ExitButton.onClick.AddListener(OnClick);
	    ExitButton.targetGraphic.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
	        if (isExitOpen)
	        {
	            ExitButton.targetGraphic.enabled = isExitOpen = false;
	        }
	        else
	        {
	            ExitButton.targetGraphic.enabled = isExitOpen = true;
	        }
	    }
	}

    private void OnClick()
    {

        Application.Quit();
    }


}
