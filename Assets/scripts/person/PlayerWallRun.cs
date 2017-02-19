using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController))]
public class PlayerWallRun : MonoBehaviour
{

    [SerializeField] private float wallRunTime;
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private LayerMask layerMask;

    private float wallTime;
    private bool isWallRuning;
    private int colliderHash;
    private bool fromGround;
    private Vector3 direction;
   

    private bool jump;
    private CharacterController characterController;
    private FirstPersonController fpc;
    private Transform defaultCamera;
    
	// Use this for initialization
	void Start ()
	{
	    characterController = GetComponent<CharacterController>();
	    fpc = GetComponent<FirstPersonController>();
	    if (fpc == null)
	    {
	        throw new Exception("No First Person Controller");
        }

	    defaultCamera = Camera.main.transform;
	}

    void Update()
    {
        jump = CrossPlatformInputManager.GetButton("Jump");
    }
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (!fpc.WallRun && !fromGround && characterController.isGrounded)
        {
            fromGround = true;
        }

        if (fpc.WallRun)
        {
            RaycastHit hitInfo;
            if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.down * characterController.height, characterController.radius, direction, out hitInfo,
                characterController.radius + characterController.skinWidth, layerMask))
            {
                fpc.WallRun = false;
            }
            else
            {

                if (hitInfo.collider.GetHashCode() != colliderHash || !CanWallRun(hitInfo.normal))
                {
                    fpc.WallRun = false;
                }
                else
                {
                    
                    wallTime -= Time.fixedDeltaTime;
                    if (wallTime <= 0f)
                    {
                        fpc.WallRun = false;
                    }
                    else
                    {
                        fpc.WallRunDirection = projectionMoveDirection(GetDirection(), hitInfo.normal);
                    }
                }
            }
        }

    }



    private Vector3 projectionMoveDirection(Vector3 forwardDirection, Vector3 normal)
    {
        return Vector3.ProjectOnPlane(forwardDirection, normal).normalized * wallRunSpeed;
    }

    private bool CanWallRun(Vector3 normal)
    {
        return !characterController.isGrounded
               && Vector3.Dot(fpc.MoveDirection, -normal) >= 0
               && Vector3.Dot(GetDirection(), -normal) >= 0;
    }

    private Vector3 GetDirection()
    {
        return defaultCamera.forward;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((LayerMask.GetMask(LayerMask.LayerToName(hit.gameObject.layer)) & layerMask) == 0)
        {
            return;
        }

        if (!fpc.WallRun && jump && CanWallRun(hit.normal) && (fromGround || hit.collider.GetHashCode() != colliderHash))
        {
            Vector3 moveDirection = projectionMoveDirection(GetDirection(), hit.normal);
            direction = -hit.normal;
            fpc.WallRun = true;
            fpc.WallRunDirection = moveDirection;
            wallTime = wallRunTime;
            colliderHash = hit.collider.GetHashCode();
            fromGround = false;
        }
    }
}
