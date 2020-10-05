using System;
using UnityEngine;

public class InteractiveObjectCollision : InteractiveObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        Debug.Log("CollisionEnter");
        if (IsCanInteract())
            TryDoInteract();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        Debug.Log("CollisionExit");
        if (IsCanInteract())
            TryDoInteract();
    }
}
