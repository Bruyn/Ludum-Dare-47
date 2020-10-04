using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCutter : MonoBehaviour
{
    [SerializeField] private string key;

    private void OnCollisionEnter(Collision other)
    {
        var breakable =  other.gameObject.GetComponent<Breakable>();
        if (breakable == null)
            return;
        
        breakable.Break(key);
    }
}
