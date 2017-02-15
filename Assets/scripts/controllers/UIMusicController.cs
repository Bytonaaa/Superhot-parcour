using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Canvas))]
class UIMusicController : MonoBehaviour
{
    private static UIMusicController musicControll;

    public static UIMusicController GetInstance
    {
        get { return musicControll; }
    }

    [SerializeField] private float timeToMusicName = 8f;
    [SerializeField] private Text musicText;



    private Canvas myCanvas;

    void Start()
    {
        if (musicControll != null)
        {
            Destroy(gameObject);
        }
        else
        {
            musicControll = this;
            DontDestroyOnLoad(this);
        }

        musicText = gameObject.GetComponentInChildren<Text>();
        if (musicText == null)
        {
            throw new Exception("Missed music text");
        }

        musicText.text = string.Empty;

        myCanvas = GetComponent<Canvas>();
        if (myCanvas == null)
        {
            throw new Exception("No Canvas at object");
        }
        checkCamera();
        
    }

    private void checkCamera()
    {
        if (myCanvas.worldCamera == null)
        {
            myCanvas.worldCamera = Camera.main;
        }
    }

    void Update()
    {
        checkCamera();   
    }

    IEnumerator TextCoroutine(string author, string song)
    {
        musicText.text = string.Format("{0}\n{1}", song, author);
        yield return new WaitForSecondsRealtime(timeToMusicName);
        musicText.text = string.Empty;
    }

    public void SetMusicName(string author, string song)
    {
        StopCoroutine("TextCoroutine");
        StartCoroutine(TextCoroutine(author, song));
    }

}

