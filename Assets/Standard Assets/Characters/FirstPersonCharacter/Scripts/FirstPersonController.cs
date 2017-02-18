using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;



[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private MouseLook m_MouseLook;

    public MouseLook MouseLook
    {
        get { return m_MouseLook; }
    }

    [SerializeField] private bool m_UseFovKick;
    [SerializeField] private FOVKick m_FovKick = new FOVKick();
    [SerializeField] private bool m_UseHeadBob;
    [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField] private float m_StepInterval;

    [SerializeField] private AudioClip[] m_FootstepSounds;
        // an array of footstep sounds that will be randomly selected from.

    [SerializeField] private AudioClip m_JumpSound; // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound; // the sound played when character touches back on ground.


    [SerializeField] private float timeWallRunWithNoButton = 0.35f;
    private float _timeWallRun;
    private bool jumpButtonReleased;

    private Camera m_Camera;
    private bool m_Jump;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;

    public Vector3 MoveDirection
    {
        get { return m_MoveDir; }
    }

    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;


    private Transform playerScaleFactor;

    private bool _wallRun;

    public bool WallRun
    {
        get { return _wallRun; }
        set
        {
            _wallRun = value;
            if (value)
            {
                DoubleJump = true;
                _timeWallRun = timeWallRunWithNoButton;
                jumpButtonReleased = false;
            }
        }
    }

    public bool IsSeating { get; set; }
    public float SeatFactor { get; set; }

    public Vector3 WallRunDirection { get; set; }
    private bool DoubleJump;

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();

        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle/2f;
        m_Jumping = false;
        DoubleJump = true;
        m_MouseLook.Init(transform, m_Camera.transform);

        isControlled = true;

        playerScaleFactor = transform.parent;
        if (playerScaleFactor == null)
        {
            playerScaleFactor = Instantiate(new GameObject("PlayerScaleFactor")).transform;
            transform.SetParent(playerScaleFactor);
        }
    }

    public void setJump()
    {
        if (WallRun || (!m_Jump && (DoubleJump || m_CharacterController.isGrounded)))
        {
            m_Jump = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        RotateView();
        // the jump state needs to read here to make sure it is not missed
        if (WallRun || (!m_Jump && (DoubleJump || m_CharacterController.isGrounded)))
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        jumpButtonReleased = jumpButtonReleased ? jumpButtonReleased : CrossPlatformInputManager.GetButtonUp("Jump");


        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            StartCoroutine(m_JumpBob.DoBobCycle());
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;

        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }


    private void PlayLandingSound()
    {
        //m_AudioSource.clip = m_LandSound;
        //m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }


    public bool isControlled { get; private set; }
    public Vector3 unControllDirection { get; set; }
    private float _unControllSpeed;

    public float unControllSpeed
    {
        get { return useUnControllSpeed ? _unControllSpeed : m_WalkSpeed; }
        set { _unControllSpeed = value; }
    }

    public bool useUnControllSpeed { get; set; }

    IEnumerator UnControllCoroutine(float time)
    {
        isControlled = false;
        yield return new WaitForSecondsRealtime(time);
        isControlled = true;
    }

    public void UnControll(float time)
    {
        if (isControlled)
        {
            StartCoroutine(UnControllCoroutine(time));
        }
    }


    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = isControlled ? transform.forward*m_Input.y + transform.right*m_Input.x : unControllDirection * unControllSpeed ;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position + m_CharacterController.center - Vector3.up * 0.5f * (m_CharacterController.height + m_CharacterController.skinWidth), m_CharacterController.radius * 0.5f, Vector3.down, out hitInfo,
            m_CharacterController.height, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        Debug.DrawLine(transform.position, transform.position + m_CharacterController.center - Vector3.up * 0.5f * (m_CharacterController.height + m_CharacterController.skinWidth), Color.red);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x*speed;
        m_MoveDir.z = desiredMove.z*speed;

        var jump = false;

        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;
            if (m_Jump)
            {
                Jump();
                jump = true;
            }
            DoubleJump = true;
        }
        else if (WallRun)
        {

            if (!jumpButtonReleased)
            {
                m_MoveDir = WallRunDirection*speed;
            }
            else if (_timeWallRun <= 0f)
            {
                WallRun = false;
            }
            else
            {
                _timeWallRun -= Time.fixedDeltaTime;
                if (m_Jump)
                {
                    Jump();
                    WallRun = false;
                    jump = true;
                }
            }
        }
        else if (DoubleJump && m_Jump)
        {
            Jump();
            DoubleJump = false;
            jump = true;

        }
        else
        {
            m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
        }

        var last = m_CharacterController.isGrounded;

        m_CharacterController.Move(Vector3.forward*0.01f);
        m_CharacterController.Move(Vector3.back * 0.01f);
        m_CharacterController.Move(Vector3.left * 0.01f);
        m_CharacterController.Move(Vector3.right * 0.01f);


        m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

        if (m_CollisionFlags == CollisionFlags.Above)
        {
            WallRun = false;
        }

        if (last && !m_CharacterController.isGrounded && !jump && !WallRun)
        {
            m_MoveDir.y = 0f;
        }

        var colliders = WallRun ? Physics.OverlapCapsule(
            transform.position + m_CharacterController.center + m_CharacterController.height*.5f*Vector3.up,
            transform.position + m_CharacterController.center - m_CharacterController.height*.5f*Vector3.up,
            m_CharacterController.radius + m_CharacterController.skinWidth + 0.1f) : 
            m_CharacterController.isGrounded ?
            Physics.OverlapSphere(transform.position + m_CharacterController.center - (m_CharacterController.height * .5f + m_CharacterController.skinWidth) * Vector3.up ,
                m_CharacterController.radius) : new Collider[0];





        var flag = false;
        foreach (var VARIABLE in colliders)
        {
            if (VARIABLE.gameObject.GetComponent("MoveableBlock"))
            {
                if (playerScaleFactor.parent == null ||
                    playerScaleFactor.parent.GetHashCode() != VARIABLE.transform.GetHashCode())
                {
                    playerScaleFactor.SetParent(VARIABLE.transform, true);
                    
                }
                flag = true;
            }
        }


        if (!flag && playerScaleFactor.parent != null)
        {
            playerScaleFactor.SetParent(null, true);
        }

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
    }

    private void Jump()
    {
        m_MoveDir.y = m_JumpSpeed;
        PlayJumpSound();
        m_Jump = false;
        m_Jumping = true;

    }

    private void PlayJumpSound()
    {
        /*m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();*/
    }


    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                           Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    private void PlayFootStepAudio()
    {
        return;
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        //int n = Random.Range(1, m_FootstepSounds.Length);
        //m_AudioSource.clip = m_FootstepSounds[n];
        //m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        //m_FootstepSounds[n] = m_FootstepSounds[0];
        //m_FootstepSounds[0] = m_AudioSource.clip;
    }


    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob)
        {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                    (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }


    private void GetInput(out float speed)
    {
        // Read input
        float vertical = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ? 1f :
                           (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? -1f : 0f;
            //CrossPlatformInputManager.GetAxis("Horizontal");
        float horizontal = (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ? 1f :
                           (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ? -1f : 0f;
        //CrossPlatformInputManager.GetAxis("Vertical");
        bool waswalking = m_IsWalking;

        // set the desired speed to be walking or running
        speed = IsSeating ? m_WalkSpeed * SeatFactor : m_WalkSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }


    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        return;
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
    }

}

