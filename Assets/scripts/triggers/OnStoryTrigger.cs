using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStoryTrigger : MonoBehaviour, IClickable
{

    [SerializeField] private string storyText;
    [Range(0.01f, 1f)] [SerializeField] private float delay;
    [SerializeField] private Collider myCollider;
    [SerializeField] private bool openByButton;
    // Use this for initialization
    void Start () {
        if (myCollider == null && (myCollider = GetComponent<Collider>()) == null)
        {
            throw new Exception("No trigger exist");
        }

        myCollider.isTrigger = true;

        if (openByButton)
        {
            enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            UIController.Controler.PlayStoryText(storyText.Split(' '), delay);
            Destroy(this);
        }
    }

    public void Click()
    {
        if (openByButton)
        {
            enabled = true;
        }
    }




}
