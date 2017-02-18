using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveableBlock : MonoBehaviour
{

    [SerializeField] private LineCurve line;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool fromStart = true;

    private const float speed = 1f;
    private float myPos = 0f;
    private int myIndex = 0;
    private int nextIndex = 1;
    private bool turnBack = false;
    private bool moved;

    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        if (myRigidbody == null)
        {
            myRigidbody = gameObject.AddComponent<Rigidbody>();
        }
        myRigidbody.isKinematic = true;
        myRigidbody.useGravity = false;
        if (fromStart)
        {
            StartMove();
        }
        else
        {
            StopMove();
        }
    }

    void FixedUpdate()
    {
        if (moved)
        {
            Move(Time.fixedDeltaTime);
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

        myPos += dt;
        while (myPos >= line.getTime(myIndex, nextIndex))
        {
            myPos -= line.getTime(myIndex, nextIndex);
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
                    break;
                }
            }
        }

        myRigidbody.MovePosition(line.getPosition(myIndex, nextIndex, myPos / line.getTime(myIndex, nextIndex)));
        transform.position = line.getPosition(myIndex, nextIndex, myPos / line.getTime(myIndex, nextIndex));

    }

    public void StopMove()
    {
        moved = false;
    }
	
	
}
