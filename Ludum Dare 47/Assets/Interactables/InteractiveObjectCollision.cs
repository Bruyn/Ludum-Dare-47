using System;
using UnityEngine;

public class InteractiveObjectCollision : InteractiveObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("simulatedEntity"))
            return;
        if (IsCanInteract())
            TryDoInteract();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("simulatedEntity"))
            return;
        Debug.Log("CollisionExit");
        if (IsCanInteract())
            TryDoInteract();
    }
}
