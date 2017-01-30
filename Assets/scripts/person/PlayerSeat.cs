using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerSeat : MonoBehaviour
{


    private bool IsSeating;
    private float originalScale;
    private float originalCenter;
    private CharacterController characterController;
	// Use this for initialization
	void Start ()
	{
	    IsSeating = false;
	    characterController = GetComponent<CharacterController>();
        originalScale = characterController.height;

	}
	

    void FixedUpdate()
    {
        if (GetSeatButton())
        {
            if (!IsSeating)
            {
                characterController.height = originalScale * 0.5f;
                characterController.center = new Vector3(characterController.center.x, -characterController.height * 0.5f, characterController.center.z);
                if (characterController.isGrounded)
                {
                    transform.position += Vector3.down * originalScale * 0.5f;
                }
                IsSeating = true;
            }
        }
        else if (IsSeating)
        {
            IsSeating = false;
            characterController.height = originalScale;
            characterController.center = new Vector3(characterController.center.x, -characterController.height * 0.5f, characterController.center.z);
            if (characterController.isGrounded)
            {
                transform.position += Vector3.up*originalScale*0.5f;
            }
        }
    }

    private bool GetSeatButton()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }
}
