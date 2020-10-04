using UnityEngine;

namespace Interactables
{
    public class ThrowableObject : InteractiveObject, IThrowable
    {
        public Rigidbody rb;
        
        public void Take()
        {
            rb.isKinematic = true;
        }

        public void Throw(Vector3 direction)
        {
            rb.isKinematic = false;
            rb.velocity = direction * 20;
        }

        public override bool IsCanInteract()
        {
            return !rb.isKinematic;
        }
    }
}