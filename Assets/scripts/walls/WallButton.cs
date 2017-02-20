using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(NeonEffect))]
public class WallButton : MonoBehaviour
{

    [SerializeField] private GameObject[] targets;
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

        if (canCheck && (Input.GetKeyDown(KeyCode.T) || Input.GetMouseButtonDown(0)) && playerCollider.isTrigger  && playerCollider.CompareTag("Player"))
        {

            if (CanBePressedAgain && isPressed)
            {
                isPressed = false;
                effect.Reset();
                canCheck = false;
            }
            else if (!isPressed)
            {
                canCheck = false;
                isPressed = true;
                effect.setNeonLine(pressedLineColor, pressedFillColor, pressedLineSize);
            }
            else
            {
                return;
            }

            foreach (var VARIABLE in targets.SelectMany(t => t.GetComponents<MonoBehaviour>()).OfType<IClickable>())
            {
                VARIABLE.Click();
            }      

        }
    }
}
