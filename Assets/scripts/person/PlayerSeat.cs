using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(FirstPersonController))]
public class PlayerSeat : MonoBehaviour
{

    [SerializeField] private float seatFactor = 0.5f;
    [SerializeField] private KeyCode seatKey = KeyCode.LeftControl;
    [SerializeField] private float slideStartSpeedFactor = 1.5f;
    [SerializeField] private float slideSlow = 1f;
    [SerializeField] private float minMoveTimeToSlide = 0.1f;


    private float moveTime;
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
	    originalBoxScale = box == null ? 1f : box.size.y;
	    
	    fpc = GetComponent<FirstPersonController>();
	    if (fpc == null || characterController == null)
	    {
	        throw new Exception("No First Person Controller");
	    }
	    fpc.SeatFactor = seatFactor;
	    slided = false;

	}

    private bool playerMove()
    {
        return new Vector2(characterController.velocity.x, characterController.velocity.z).sqrMagnitude >= 0.1f;
    }

    private bool slided;

    IEnumerator slide()
    {
        slided = true;

        float mySpeed = fpc.WalkSpeed * slideStartSpeedFactor;
        fpc.isControlled = false;
        fpc.useUnControllSpeed = true;
        fpc.unControllDirection = transform.forward;
        while (mySpeed > seatFactor * fpc.WalkSpeed)
        {
            fpc.unControllSpeed = mySpeed;     
            yield return new WaitForFixedUpdate();
            mySpeed -= slideSlow * Time.fixedDeltaTime;
        }

        slided = false;
        fpc.isControlled = true;
    }

    void FixedUpdate()
    {
        if (slided)
        {
            return;
        }


        if (!IsSeating && characterController.isGrounded && playerMove())
        {
            if (moveTime < minMoveTimeToSlide)
            {
                moveTime += Time.fixedDeltaTime;
            }
        }
        else
        {
            moveTime = 0f;
        }


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

                if (moveTime >= minMoveTimeToSlide)
                {
                    StartCoroutine(slide());
                }
            }

            
        } else if (IsSeating && CanStandUp())
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
        return Input.GetKey(seatKey);
    }


    private bool CanStandUp()
    {
        if (!IsSeating)
        {
            return true;
        }


        return (
            Physics.OverlapCapsule(transform.position + Vector3.up*originalScale*0.5f,
                transform.position, characterController.radius, ~LayerMask.GetMask("Player"),
                QueryTriggerInteraction.Ignore).Length == 0);   
    }
}
