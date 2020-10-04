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

        if (_currentThrowable != null)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                ((MonoBehaviour) _currentThrowable).transform.parent = null;
                _currentThrowable.Throw(cameraObject.forward);
                _currentThrowable = null;
            }

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

        TrySetText(otherInteractive.interactionText);

        if (Input.GetButtonDown("Fire3"))
        {
            IThrowable throwable = otherInteractive as IThrowable;

            if (throwable != null)
            {
                throwable.Take();
                otherInteractive.transform.parent = throwableObjectAttachTransform;
                otherInteractive.transform.localPosition = Vector3.zero;
                _currentThrowable = throwable;
                return;
            }

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