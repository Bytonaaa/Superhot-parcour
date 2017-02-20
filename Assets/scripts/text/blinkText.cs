using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blinkText : MonoBehaviour
{


    [SerializeField] private Text textComp;
    [Range(0.1f, 5f)] [SerializeField] private float blinkTime = 2f;
    [SerializeField] private float waitStartTime = 2f;


    private float myTime;
    private bool moveToOneAlpha;
	// Use this for initialization
	void Start () {
	    if (textComp == null && (textComp = GetComponent<Text>()) == null)
	    {
	        throw new Exception("No text on object");
	    }

	    moveToOneAlpha = true;
        textComp.enabled = false;
        textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b, 0f);
    }


    void Update()
    {
        if (waitStartTime > 0f)
        {
            waitStartTime -= Time.deltaTime;
            if (waitStartTime <= 0f)
            {
                textComp.enabled = true;
            }
        }
        else
        {
            if (moveToOneAlpha)
            {
                if (myTime >= blinkTime)
                {
                    moveToOneAlpha = false;
                    textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b, 1f);
                    myTime = 0f;
                }
                else
                {
                    textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b,
                        Mathf.Clamp01(myTime/blinkTime));
                }
            }
            else
            {
                if (myTime >= blinkTime)
                {
                    moveToOneAlpha = true;
                    textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b, 0f);
                    myTime = 0f;
                }
                else
                {
                    textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b,
                        1 - Mathf.Clamp01(myTime / blinkTime));
                }
            }


            myTime += Time.deltaTime;
        }
    }
	
}
