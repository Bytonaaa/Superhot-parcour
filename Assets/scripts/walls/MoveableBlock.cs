﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBlock : MonoBehaviour
{

    [SerializeField] private LineCurve line;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool fromStart = true;

    private const float speed = 1f;
    private const float maxPos = 100f;
    private float myPos = 0f;
    private int myIndex = 0;
    private int nextIndex = 1;
    private bool turnBack = false;
    private bool moved;

	void Start () {
	    if (fromStart)
	    {
	        StartMove();
	    }
	}

    void Update()
    {
        if (moved)
        {
            Move(Time.deltaTime);
        }
    }


    public void StartMove()
    {
        moved = true;
    }

    public void Move(float dt)
    {
        if (line.isEnd(nextIndex))
        {
            StopMove();
            return;
        }

        myPos += dt*speed*line.getSpeed(myIndex, nextIndex);
        while (myPos >= maxPos)
        {
            myPos -= maxPos;
            myIndex = nextIndex;
            nextIndex += turnBack ? -1 : 1;
            if (line.isEnd(nextIndex))
            {
                if (loop)
                {
                    turnBack = !turnBack;
                    nextIndex += turnBack ? -2 : 2;
                }
                else
                {
                    StopMove();
                }
            }
        }
        transform.position = line.getPosition(myIndex, nextIndex, myPos/maxPos);



    }

    public void StopMove()
    {
        moved = false;
    }
	
	
}