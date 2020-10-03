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
        bool jump = Input.GetButtonDown("Jump");
        controller.Move(horizontal, vertical, jump);
    }
}