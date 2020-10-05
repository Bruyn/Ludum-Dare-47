using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField]
    private List<InteractiveResponse> objectsToInteract = new List<InteractiveResponse>();

    public string interactionText;
    public bool isAvailableByDefault = true;
    public bool isOneTimeInteraction;

    private bool lastActionIsDo;
    private bool isAvailable;
    
    private List<int> doIdxs = new List<int>();
    private List<int> undoIdxs = new List<int>();
    private int currentId;

    private void Start()
    {
        isAvailable = isAvailableByDefault;
        lastActionIsDo = !isAvailable;
        
        if (objectsToInteract.Count == 0)
            Debug.LogWarning(gameObject.name + " has 0 interactable objects! Set them in the editor.");
    }

    private void Update()
    {
        /*
        var a = PlayerSwitchMng.Instance;
        var currentAuth = PlayerSwitchMng.Instance.GetCurrentAuthority(); 
        if (currentAuth == null)
            return;
        currentId = currentAuth.GetComponent<InputController>().GetCurrentCommandIdx();
        if (doIdxs.Contains(currentId))
            SimulateDo();
        if (undoIdxs.Contains(currentId))
            SimulateUndo();
        */    
    }

    public virtual bool IsCanInteract()
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
        doIdxs.Add(currentId);
        foreach (var objectToInteract in objectsToInteract)
            objectToInteract.DoResponseAction();

        Debug.Log("Do interaction with " + gameObject.name);
    }

    private void UndoInteractInternal()
    {
        undoIdxs.Add(currentId);
        foreach (var objectToInteract in objectsToInteract)
            objectToInteract.UndoResponseAction();

        Debug.Log("Undo interaction with " + gameObject.name);
    }
    
    private void SimulateDo()
    {
        foreach (var objectToInteract in objectsToInteract)
            objectToInteract.DoResponseAction();

        Debug.Log("Simulated do with " + gameObject.name);
    }

    private void SimulateUndo()
    {
        foreach (var objectToInteract in objectsToInteract)
            objectToInteract.UndoResponseAction();

        Debug.Log("Simulated undo with " + gameObject.name);
    }

    private void MakeUninteractable()
    {
        isAvailable = false;
    }
}
