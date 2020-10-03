using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private PlayerContoller controller;
    
    void Start()
    {
        controller = GetComponent(typeof(PlayerContoller));
    }
    
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        controller.Move(horizontal, vertical);

        if (Input.GetButtonDown("Jump"))
        {
            controller.Jump();
        }
    }
}