using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    public LayerMask interactibleLayer;
    public Transform cameraObject;
    public float maxRayDistance = 4f;

    private IThrowable _currentThrowable;
    private Text uiInteractiveTextBox;
    public Authority _authority;

    public Transform throwableObjectAttachTransform;

    private SimulationController simController;
    
    private void Awake()
    {
        var textObject = GameObject.FindWithTag("interactableTextBox");
        if (textObject != null)
        {
            uiInteractiveTextBox = textObject.GetComponent<Text>();
        }

        var obj = GameObject.FindWithTag("simulationController"); 
        simController = obj.GetComponent<SimulationController>();
    }
    
    public void Interact()
    {
        if (_currentThrowable != null)
        {
            ((MonoBehaviour) _currentThrowable).transform.parent = null;
            _currentThrowable.Throw(cameraObject.forward);
            _currentThrowable = null;
            return;
        }

        RaycastHit hit;
        if (!Physics.Raycast(cameraObject.position, cameraObject.forward, out hit, maxRayDistance, interactibleLayer))
        {
            return;
        }
        
        GameObject obj = hit.collider.gameObject;
        InteractiveObject otherInteractive = obj.GetComponent<InteractiveObject>();
        
        if (otherInteractive == null || !otherInteractive.IsCanInteract())
        {
            return;
        }

        IThrowable throwable = otherInteractive as IThrowable;

        if (throwable != null)
        {
            var sim = obj.GetComponent<SimulatedEntityBase>();
            if (sim == null && simController.GetCurrentMode() != PlaybackMode.PlayAndRecord)
                return;
            
            throwable.Take();
            otherInteractive.transform.parent = throwableObjectAttachTransform;
            otherInteractive.transform.localPosition = Vector3.zero;
            _currentThrowable = throwable;
            return;
        }
        
        otherInteractive.TryDoInteract();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_authority.Enabled)
            return;
        
        TrySetText("");
        
        RaycastHit hit;
        if (!Physics.Raycast(cameraObject.position, cameraObject.forward, out hit, maxRayDistance, interactibleLayer))
        {
            return;
        }

        GameObject obj = hit.collider.gameObject;
        InteractiveObject otherInteractive = obj.GetComponent<InteractiveObject>();

        if (otherInteractive == null || !otherInteractive.IsCanInteract())
        {
            return;
        }

        TrySetText(otherInteractive.interactionText);
    }

    void TrySetText(string value)
    {
        if (uiInteractiveTextBox)
        {
            uiInteractiveTextBox.text = value;
            uiInteractiveTextBox.enabled = value != "";
        }
    }
}