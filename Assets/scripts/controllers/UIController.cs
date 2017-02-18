using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Controler { get; private set; }

    public static bool isController { get { return Controler != null; } }

    [SerializeField] private Text timeText;
    [SerializeField] private Text helpText;
    [SerializeField] private Text storyText;


    private TimeToLevelEnd time;

    private Text getTextFromObject(string name)
    {
        var temp = gameObject.GetComponentsInChildren<Text>();
        foreach (var VARIABLE in temp)
        {
            if (VARIABLE.name == name)
            {
                return VARIABLE;
            }
        }
        return null;
    }

	// Use this for initialization
	void Start () {
	    if (isController)
	    {
	        Destroy(this);
	    }
	    else
	    {
	        Controler = this;
	    }

	    if (timeText == null && (timeText = getTextFromObject("Time Text")) == null)
	    {
	        throw new Exception("No GameObjet \"Time Text\" or no component \"Text\"");

	    } else if (helpText == null && (helpText = getTextFromObject("Help Text")) == null)
	    {
            throw new Exception("No GameObjet \"Help Text\" or no component \"Text\"");
        } else if (storyText == null && (storyText = getTextFromObject("Story Text")) == null)
	    {
            throw new Exception("No GameObjet \"Story Text\" or no component \"Text\"");
        }


        var temp = FindObjectsOfType<TimeToLevelEnd>();
        if (temp.Length > 1)
        {
            throw new Exception("No more then 1 \"TimeToLevelEnd\" script");
        }

        if (temp.Length > 0)
        {
            time = temp[0];
        }

	    storyText.text = helpText.text = timeText.text = string.Empty;

	}

    void Update()
    {
        if (time != null)
        {
            timeText.text = string.Format("{0:F2}", time.LevelTime);
        }
    }

    IEnumerator StoryText(string[] text, float delay)
    {
        if (text == null || text.Length <= 0)
        {
            yield break;
        }

        int myState = 0;
        while (myState < text.Length)
        {
            storyText.text = text[myState++];
            yield return new WaitForSecondsRealtime(delay);
        }

        storyText.text = string.Empty;
    }

    public void PlayStoryText(string[] text, float delay)
    {
        StopCoroutine("StoryText");
        StartCoroutine(StoryText(text, delay));
    }

    public void DisplayHelpText(string text)
    {
        helpText.text = text ?? string.Empty;
    }

    public void DisableHelpText()
    {
        helpText.text = string.Empty;
    }

    void OnDestroy()
    {
        if (Controler == this)
        {
            Controler = null;
            StopAllCoroutines();
        }
    }

    



}
