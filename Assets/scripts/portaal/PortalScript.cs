﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalScript : MonoBehaviour
{
    [SerializeField]
    private string nextLevel;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerTurn>().setWinTurn();
            StartCoroutine(loading());
        }
    }

    private IEnumerator loading()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneLoader.LoadScene(nextLevel);
    }
}
