using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(NeonEffect))]
public class WallButton : MonoBehaviour
{

    [SerializeField] private MoveableBlock target;
    [SerializeField] private Color pressedLineColor;
    [SerializeField] private Color pressedFillColor;
    [SerializeField] private float pressedLineSize;

    [SerializeField] private bool CanBePressedAgain;

    private bool isPressed;
    private NeonEffect effect;
    private bool canCheck;

    // Use this for initialization
	void Start ()
	{
	    GetComponent<Collider>().isTrigger = true;
	    effect = GetComponent<NeonEffect>();
	    isPressed = false;
	}

    void Update()
    {
        canCheck = true;
    }

    void OnTriggerStay(Collider playerCollider)
    {

        if (canCheck && target && Input.GetKeyDown(KeyCode.E) && playerCollider.isTrigger  && playerCollider.CompareTag("Player"))
        {
            if (CanBePressedAgain && isPressed) { 
                target.StopMove();
                isPressed = false;
                effect.Reset();
                canCheck = false;
            } else if (!isPressed)
            {
                canCheck = false;
                target.StartMove();
                isPressed = true;
                effect.setNeonLine(pressedLineColor, pressedFillColor, pressedLineSize);
            }
        }
    }
}
