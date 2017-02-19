using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveableBlock : MonoBehaviour
{

    [SerializeField] private LineCurve line;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool fromStart = true;
    [SerializeField] private bool waitFirstPoint = false;

    private const float speed = 1f;
    private float myPos = 0f;
    private int myIndex = 0;
    private int nextIndex = 1;
    private bool turnBack = false;
    private bool moved;
    private bool endWaitPoint;
    
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

        if (waitFirstPoint)
        {
            StartCoroutine(WaitTime(myIndex));
        }
        else
        {
            endWaitPoint = true;
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

    IEnumerator WaitTime(int pos)
    {
        endWaitPoint = false;
        yield return new WaitForSeconds(line.getWaitTime(pos));
        endWaitPoint = true;
    }

    public void Move(float dt)
    {
        if (line.isEnd(nextIndex))
        {
            StopMove();
            return;
        }

        if (!endWaitPoint)
        {
            return;
        }

        
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

                    StartCoroutine(WaitTime(myIndex));
                    myRigidbody.MovePosition(line.getConcretePosiion(myIndex));

                }
                else
                {
                    StopMove();
                    myRigidbody.MovePosition(line.getConcretePosiion(myIndex));
                    return;
                }
            }
            else
            {
                StartCoroutine(WaitTime(myIndex));
                myRigidbody.MovePosition(line.getConcretePosiion(myIndex));
            }
        }

        myPos += dt;
        myRigidbody.MovePosition(line.getPosition(myIndex, nextIndex, myPos / line.getTime(myIndex, nextIndex)));
    }

    public void StopMove()
    {
        moved = false;
    }
	
	
}
