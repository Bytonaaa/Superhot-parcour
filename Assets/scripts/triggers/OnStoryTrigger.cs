using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStoryTrigger : MonoBehaviour
{

    [SerializeField] private string storyText;
    [Range(0.01f, 1f)] [SerializeField] private float delay;
    [SerializeField] private Collider myCollider;
    // Use this for initialization
    void Start () {
        if (myCollider == null && (myCollider = GetComponent<Collider>()) == null)
        {
            throw new Exception("No trigger exist");
        }

        myCollider.isTrigger = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            UIController.Controler.PlayStoryText(storyText.Split(' '), delay);
            Destroy(this);
        }
    }
}
