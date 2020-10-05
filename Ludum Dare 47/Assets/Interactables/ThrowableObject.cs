using UnityEngine;

namespace Interactables
{
    public class ThrowableObject : InteractiveObject, IThrowable
    {
        public Rigidbody rb;

        public bool taken;
        
        public void Take()
        {
            rb.isKinematic = true;
            taken = true;
        }

        public void Throw(Vector3 direction)
        {
            rb.isKinematic = false;
            rb.velocity = direction * 20;
            taken = false;
        }

        public override bool IsCanInteract()
        {
            return !taken;
        }
    }
}