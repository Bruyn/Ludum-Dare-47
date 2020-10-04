using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private string key;
    
    public void Break(string incomingKey)
    {
        if (incomingKey != key)
            return;

        gameObject.SetActive(false);
        //we can add fancy particles later
    }
}
