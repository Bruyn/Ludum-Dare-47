using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectCollision : InteractiveObject
{
    private List<GameObject> objectInside = new List<GameObject>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("simulatedEntity"))
            return;
        
        if (IsCanInteract() && objectInside.Count == 0)
        {
            TryDoInteract();
        }
        
        objectInside.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("simulatedEntity"))
            return;

        objectInside.Remove(other.gameObject);
        if (objectInside.Count > 0)
            return;

        if (IsCanInteract())
        {
            TryDoInteract();
            objectInside.Remove(other.gameObject);
        }
    }
}
