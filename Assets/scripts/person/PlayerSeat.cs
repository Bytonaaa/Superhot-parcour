using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(FirstPersonController))]
public class PlayerSeat : MonoBehaviour
{

    [SerializeField] private float seatFactor = 0.5f;

    private bool IsSeating;
    private float originalScale;
    private float originalBoxScale;
    private float originalCenter;
    private CharacterController characterController;
    private BoxCollider box;
    private FirstPersonController fpc;
	// Use this for initialization
	void Start ()
	{
	    IsSeating = false;
	    characterController = GetComponent<CharacterController>();
        originalScale = characterController.height;
	    box = GetComponent<BoxCollider>();
	    if (box != null)
	    {
	        originalBoxScale = box.size.y;
	    }
	    fpc = GetComponent<FirstPersonController>();
	    if (fpc == null || characterController == null)
	    {
	        throw new Exception("No First Person Controller");
	    }
	    fpc.SeatFactor = seatFactor;    

	}
	

    void FixedUpdate()
    {
        if (GetSeatButton())
        {
            if (!IsSeating)
            {
                characterController.height = originalScale * 0.5f;
                if (box != null)
                {
                    box.center += Vector3.up * originalBoxScale * 0.25f;
                    box.size = new Vector3(box.size.x, originalBoxScale*.5f, box.size.z);
                }
                characterController.center = new Vector3(characterController.center.x, -characterController.height * 0.5f, characterController.center.z);
                if (characterController.isGrounded)
                {
                    transform.position += Vector3.down * originalScale * 0.5f;

                }
                IsSeating = true;
                fpc.IsSeating = IsSeating;
            }
        }
        else if (IsSeating)
        {
            IsSeating = false;
            characterController.height = originalScale;
            if (box != null)
            {
                box.center += Vector3.down * originalBoxScale * 0.25f;
                box.size = new Vector3(box.size.x, originalBoxScale, box.size.z);
            }
            characterController.center = new Vector3(characterController.center.x, -characterController.height * 0.5f, characterController.center.z);
            
            if (characterController.isGrounded)
            {
                transform.position += Vector3.up*originalScale*0.5f;
            }
            fpc.IsSeating = IsSeating;
        }
    }

    private bool GetSeatButton()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
}
