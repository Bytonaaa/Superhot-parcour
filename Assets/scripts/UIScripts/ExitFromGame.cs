using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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


	    ExitButton.targetGraphic.enabled = isExitOpen = false;


	    SceneManager.sceneLoaded += disable;
	}


    private void disable(Scene scene, LoadSceneMode mode)
    {
        ExitButton.targetGraphic.enabled = isExitOpen = false;
    }


    private IEnumerator waitEsc()
    {
        float time = 0f;
        ExitButton.targetGraphic.enabled = isExitOpen = true;
        while (time <= 2f) 
        {
            
            yield return new WaitForEndOfFrame();
            if (pressed)
            {
                OnClick();
            }
            time += Time.unscaledDeltaTime;
            
        }

        ExitButton.targetGraphic.enabled = isExitOpen = false;
    }

    private bool pressed = false;
	// Update is called once per frame
	void Update ()
	{

	    pressed = false;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pressed = true;
           
        }

        if (!isExitOpen && pressed)
        {
            pressed = false;
            StartCoroutine(waitEsc());
        }
	}


    private void OnClick()
    {
        Debug.Log("Exit from game");
        Application.Quit();
    }


}
