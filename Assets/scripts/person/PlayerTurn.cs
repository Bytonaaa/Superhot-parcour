using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerTurn : MonoBehaviour
{

    [SerializeField] private float TimeToTurn = 0.1f;
    [SerializeField] private float TimeToUnControl = 0.3f;
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
            if (flag && GetTurnButton())
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
        return Input.GetKeyDown(KeyCode.V);
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
}
