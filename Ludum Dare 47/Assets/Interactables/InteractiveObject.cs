using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public event Action OnBecameUninteractable;
    
    public bool isAvailableByDefault = true;
    public bool isOneTimeInteraction;
    public string interactionText;

    public string[] responseObjectTags;
    
    private bool isAvailable;

    public bool IsCanInteract()
    {
        return isAvailable;
    }
    
    public void TryExecuteAction()
    {
        ExecuteInternal();

        if (isOneTimeInteraction)
        {
            MakeUninteractable();
        }
    }

    private void ExecuteInternal()
    {
        foreach (var objectTag in responseObjectTags)
        {
            var obj = GameObject.FindWithTag(objectTag);
            if (obj != null)
            {
                obj.GetComponent<InteractiveResponse>().DoResponseAction();
            }
        }
    }

    private void MakeUninteractable()
    {
        isAvailable = false;
        OnBecameUninteractable();
    }

    private void Awake()
    {
        isAvailable = isAvailableByDefault;
    }
}
