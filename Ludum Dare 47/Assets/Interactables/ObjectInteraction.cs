using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] LayerMask interactibleLayer;
    [SerializeField] Text uiInteractiveTextBox;
    [SerializeField] Transform cameraObject;
    [SerializeField] float maxRayDistance = 4f;

    private GameObject lastChecked;
    private InteractiveObject currentInteractive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            TryInteract();
        }
        
        RaycastHit hit;
        if (Physics.Raycast(cameraObject.position, cameraObject.forward, out hit, maxRayDistance, interactibleLayer))
        {
            if (lastChecked == hit.collider.gameObject || hit.collider == null)
                return;

            lastChecked = hit.collider.gameObject;
            FoundInteractable(lastChecked);
        }
        else
        {
            lastChecked = null;
            LostInteractable();
        }
    }

    private void FoundInteractable(GameObject obj)
    {
        if (!CanInteractByLayer(obj))
        {
            return;
        }
        
        var otherInteractive = obj.GetComponent<InteractiveObject>();
        if (otherInteractive == null || !otherInteractive.IsCanInteract())
        {
            return;
        }

        currentInteractive = otherInteractive;
        currentInteractive.OnBecameUninteractable += HandleBecameUnavailable;
        uiInteractiveTextBox.text = currentInteractive.interactionText;

        uiInteractiveTextBox.enabled = true;
    }

    private void LostInteractable()
    {
        currentInteractive = null;
        uiInteractiveTextBox.enabled = false;
    }

    private void HandleBecameUnavailable()
    {
        currentInteractive.OnBecameUninteractable -= HandleBecameUnavailable;
        uiInteractiveTextBox.enabled = false;
    }

    private bool CanInteractByLayer(GameObject obj)
    {
        return interactibleLayer == (interactibleLayer | (1 << obj.layer));
    }

    private void TryInteract()
    {
        if (currentInteractive != null)
        {
            currentInteractive.TryDoInteract();
        }
    }
    
}
