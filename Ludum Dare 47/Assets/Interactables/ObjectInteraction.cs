using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] LayerMask interactibleLayer;
    [SerializeField] Transform cameraObject;
    [SerializeField] float maxRayDistance = 4f;

    private Text uiInteractiveTextBox;
    public Authority _authority;
    
    private void Awake()
    {
        var textObject = GameObject.FindWithTag("interactableTextBox");
        if (textObject != null)
        {
            uiInteractiveTextBox = textObject.GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_authority.Enabled) return;
        
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

        if (Input.GetButtonDown("Fire3"))
        {
            otherInteractive.TryDoInteract();
        }
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