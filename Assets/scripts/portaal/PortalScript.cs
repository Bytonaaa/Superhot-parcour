using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalScript : MonoBehaviour {

    [SerializeField] private string nextLevel;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AnalyticsHelper.LogSceneLoadEvent(nextLevel);
            GameController.LoadLevel(nextLevel);
        }
    }
}
