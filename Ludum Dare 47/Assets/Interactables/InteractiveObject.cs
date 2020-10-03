using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public event Action OnBecameUninteractable;

    public string interactionText;
    [SerializeField] bool isAvailableByDefault = true;
    [SerializeField] bool isOneTimeInteraction;

    public string[] responseObjectTags;

    private bool lastActionIsDo;
    private bool isAvailable;

    private void Awake()
    {
        isAvailable = isAvailableByDefault;
        lastActionIsDo = !isAvailable;
    }
    
    public bool IsCanInteract()
    {
        return isAvailable;
    }
    
    public void TryDoInteract()
    {
        SwitchInteraction();

        if (isOneTimeInteraction)
        {
            MakeUninteractable();
        }
    }

    public void TryUndoInteract()
    {
        SwitchInteraction();

        if (isOneTimeInteraction)
            isAvailable = !isAvailable;
    }

    private void SwitchInteraction()
    {
        if (lastActionIsDo)
        {
            UndoInteractInternal();
        }
        else
        {
            DoInteractInternal();
        }

        lastActionIsDo = !lastActionIsDo;
    }

    private void DoInteractInternal()
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

    private void UndoInteractInternal()
    {
        foreach (var objectTag in responseObjectTags)
        {
            var obj = GameObject.FindWithTag(objectTag);
            if (obj != null)
            {
                obj.GetComponent<InteractiveResponse>().UndoResponseAction();
            }
        }
    }
    

    private void MakeUninteractable()
    {
        isAvailable = false;
        OnBecameUninteractable();
    }
}
