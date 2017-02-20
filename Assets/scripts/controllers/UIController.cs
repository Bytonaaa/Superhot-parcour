using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIController : MonoBehaviour
{
    public static UIController Controler { get; private set; }

    public static bool isController { get { return Controler != null; } }

    [SerializeField] private Text timeText;
    [SerializeField] private Text helpText;
    [SerializeField] private Text storyText;
    [SerializeField] private Text NEOHOTText;
    [SerializeField] private AudioClip neo;
    [SerializeField] private AudioClip hot;
    [SerializeField] private Text restartText;
    [SerializeField] private float alphaRestartTimeSpeed = 2f;

    private bool restart;
    private AudioSource source;
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
	void Start ()
	{
	    restart = false;
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
            enabled = false;
            throw new Exception("No GameObjet \"Time Text\" or no component \"Text\"");
	    }

        if (helpText == null && (helpText = getTextFromObject("Help Text")) == null)
	    {
            enabled = false;
            throw new Exception("No GameObjet \"Help Text\" or no component \"Text\"");
        }

        if (storyText == null && (storyText = getTextFromObject("Story Text")) == null)
	    {
            enabled = false;
            throw new Exception("No GameObjet \"Story Text\" or no component \"Text\"");
        }

        if (NEOHOTText == null && (NEOHOTText = getTextFromObject("NEOHOT Text")) == null)
        {
            enabled = false;
            throw new Exception("No GameObjet \"NEOHOT Text\" or no component \"Text\"");
        }

        if (restartText == null && (restartText = getTextFromObject("Restart Text")) == null)
        {
            enabled = false;
            throw new Exception("No GameObjet \"Restart Text\" or no component \"Text\"");
        }

	    restartText.enabled = NEOHOTText.enabled = false;
	    source = GetComponent<AudioSource>();
	    if (source == null)
	    {
	        source = gameObject.AddComponent<AudioSource>();
	    }


        var temp = FindObjectsOfType<TimeToLevelEnd>();
        if (temp.Length > 1)
        {
            enabled = false;
            throw new Exception("more then 1 \"TimeToLevelEnd\" script");
        }

        if (temp.Length > 0)
        {
            time = temp[0];
        }

	    storyText.text = helpText.text = timeText.text = string.Empty;

        GetComponent<Canvas>().worldCamera = Camera.main;

	}

    void Update()
    {
        if (!restart && time != null)
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

    IEnumerator restartTextMoved()
    {
        float a = 0f;
       
        bool up = true;
        while (true)
        {
            if ((a > 1f && up) || (!up && a < 0f))
            {
                a = Mathf.Clamp01(a);
                up = !up;
            }

            restartText.color  = new Color(restartText.color.r, restartText.color.g, restartText.color.b, a);

            yield return new WaitForEndOfFrame();
            a += (up ? 1 : -1) * alphaRestartTimeSpeed*Time.unscaledDeltaTime;
        }
    }

    IEnumerator hotMoved()
    {
        while (true)
        {
            NEOHOTText.text = "neo";
            source.PlayOneShot(neo);
            yield return new WaitForSecondsRealtime(neo.length);

            NEOHOTText.text = "hot";
            source.PlayOneShot(hot);
            yield return new WaitForSecondsRealtime(hot.length);
        }

    }

    public void onDie()
    {
        if (!restart && enabled)
        {
            restart
                = true;

            StartCoroutine(restartTextMoved());
            StartCoroutine(hotMoved());
            helpText.enabled
                =
                timeText.enabled
                    =
                    storyText.enabled
                        = false;
            NEOHOTText.enabled = restartText.enabled = true;

        }
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
