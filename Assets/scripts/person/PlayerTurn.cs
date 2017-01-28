using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerTurn : MonoBehaviour
{

    [SerializeField] private float TimeToTurn = 0.1f;
    [SerializeField] private Transform camera;

    private bool flag = true;
    private FirstPersonController fpc;
    // Use this for initialization
    void Start()
    {
        fpc = GetComponent<FirstPersonController>();

        if (fpc == null)
        {
            throw new Exception("No First Person Controller");
        }

        camera = Camera.main.transform;
        flag = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (flag && GetTurnButton())
        {
            flag = false;
            Vector3 axis = camera.up;
            StartCoroutine(Turning(camera.forward, 180f, axis));
        }
    }

    private bool GetTurnButton()
    {
        return Input.GetKeyUp(KeyCode.LeftControl);
    }

    private IEnumerator Turning(Vector3 forwardFrom, float angle, Vector3 axis)
    {
        float time = 0f;
        Vector3 temp;
        while (time <= TimeToTurn)
        {
            temp = Quaternion.AngleAxis(180*(time/TimeToTurn), axis)*forwardFrom;
            temp.y = forwardFrom.y;
            camera.LookAt(camera.position + temp);
            
            fpc.MouseLook.Init(transform, camera.transform);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        temp = Quaternion.AngleAxis(180, axis) * forwardFrom;
        temp.y = forwardFrom.y;
        transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
        camera.LookAt(camera.position + temp);
        fpc.MouseLook.Init(transform, camera.transform);
        flag = true;
    }
}
