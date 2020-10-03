using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    public LayerMask interactibleLayer;
    public Text uiInteractiveTextBox;

    private InteractiveObject currentInteractive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            TryInteract();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherObject = other.gameObject;
        if (!CanInteractByLayer(otherObject))
        {
            return;
        }
        
        var otherInteractive = otherObject.GetComponent<InteractiveObject>();
        if (otherInteractive == null || !otherInteractive.IsCanInteract())
        {
            return;
        }

        currentInteractive = otherInteractive;
        currentInteractive.OnBecameUninteractable += HandleBecameUnavailable;
        uiInteractiveTextBox.text = currentInteractive.interactionText;

        uiInteractiveTextBox.enabled = true;
    }

    private void HandleBecameUnavailable()
    {
        currentInteractive.OnBecameUninteractable -= HandleBecameUnavailable;
        uiInteractiveTextBox.enabled = false;
    }
    private void OnTriggerExit(Collider other)
    {
        var otherObject = other.gameObject;
        if (!CanInteractByLayer(otherObject))
        {
            return;
        }

        currentInteractive = null;
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
            currentInteractive.TryExecuteAction();
        }
    }
    
}
