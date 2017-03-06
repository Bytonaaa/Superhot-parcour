using System;
using System.Collections;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{

    [SerializeField] private float TimeToTurn = 0.1f;
    [SerializeField] private float TimeToUnControl = 0.3f;
    [SerializeField] private KeyCode TurnButton = KeyCode.V;
    private Transform defaultCamera;

    private bool flag = true;
    private FirstPersonController fpc;

    private float minTime = 0.4f;
    private float _time;
    // Use this for initialization
    void Start()
    {
        fpc = GetComponent<FirstPersonController>();

        if (fpc == null)
        {
            throw new Exception("No First Person Controller");
        }

        defaultCamera = Camera.main.transform;
        flag = true;
        _time = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_time < 0f)
        {
            if (flag && GetTurnButton() && fpc.isControlled)
            {
                flag = false;
                Vector3 axis = defaultCamera.up;
                StartCoroutine(Turning(defaultCamera.forward, axis));
            }
        }
        else
        {
            _time -= Time.deltaTime;
        }
    }

    private bool GetTurnButton()
    {
        return Input.GetKeyDown(TurnButton);
    }

    public void setDieTurn()
    {
        fpc.enabled = false;
        StartCoroutine(TurningUp(0.05f));
        Destroy(GetComponent<Rigidbody>());
        GetComponent<CharacterController>().enabled = false;
        StartCoroutine(MoveDown());
    }

    public void setWinTurn()
    {

        fpc.enabled = false;
        GetComponent<PlayerDie>().win = true;
        StartCoroutine(TurningUp());
    }

    private IEnumerator Turning(Vector3 forwardFrom, Vector3 axis)
    {
        fpc.setJump();
        
        fpc.UnControll(TimeToUnControl);
        fpc.unControllDirection = -fpc.transform.forward;
        fpc.useUnControllSpeed = false;


        float time = 0f;
        Vector3 temp;
        Quaternion lastTransformRotation = transform.rotation;
        while (time <= TimeToTurn)
        {
            temp = Quaternion.AngleAxis(180*(time/TimeToTurn), axis)*forwardFrom;
            temp.y = forwardFrom.y;
            defaultCamera.LookAt(defaultCamera.position + temp);
            
            fpc.MouseLook.Init(transform, defaultCamera.transform);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        temp = Quaternion.AngleAxis(180, axis) * forwardFrom;
        temp.y = forwardFrom.y;
        transform.rotation = lastTransformRotation * Quaternion.AngleAxis(180, Vector3.up);
        defaultCamera.LookAt(defaultCamera.position + temp);
        fpc.MouseLook.Init(transform, defaultCamera.transform);
        flag = true;
        _time = minTime;
    }

    private IEnumerator TurningUp(float speedCnst = 1f)
    {
        
        Quaternion up = Quaternion.Euler(-90, 0, 0);
        float speed = 0.35f, time = 0f;
        var from = transform.rotation;
        var to = Vector3.up;

        while (true)
        {
            //var temp = Vector3.Lerp(from, to, time*speed); 
            transform.rotation = Quaternion.Lerp(from, up, time * speed * speedCnst);
            //up = Quaternion.Euler(-90, time, 0);
            yield return new WaitForEndOfFrame();
            time += Time.unscaledDeltaTime;
        }
    }


    private IEnumerator MoveDown()
    {
        float time = 0f;
        Vector3 dir = Vector3.down;
        float speed = 0.1f;

        while (time <= 5f)
        {
            yield return new WaitForEndOfFrame();
            transform.Translate(dir * speed * Time.unscaledDeltaTime);
            time += Time.unscaledDeltaTime;
        }
    }
}
