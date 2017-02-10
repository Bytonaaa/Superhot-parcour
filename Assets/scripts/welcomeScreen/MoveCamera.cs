using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private Transform center;
    [SerializeField] private float radius;
    private Vector3 R;
    private Vector3 Rtemp;
    private float pos;
    private Vector3 mainPos;
    private Vector3 mainCenterPosition;
	// Use this for initialization
	void Start ()
	{
        transform.LookAt(center.position);
        
        transform.position = center.position - transform.forward*radius;
        
	    R = transform.forward;
	    Rtemp = transform.right;
	    mainPos = transform.position;
	    mainCenterPosition = center.position;
	}
	
	// Update is called once per frame
	void Update ()
	{

        pos += speed*Time.deltaTime;
	    if (pos > 1f)
	    {
	        pos = pos - Mathf.Floor(pos);
	    }

	    var phi = Mathf.PI * pos - Mathf.PI / 2;
	    var r = 2*radius*Mathf.Sin(phi);

        transform.position = mainPos + (center.position - mainCenterPosition) + R * r * Mathf.Sin(phi) + Rtemp * r * Mathf.Cos(phi);
        transform.LookAt(center);
	}
}
