using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Authority : MonoBehaviour
{
    public bool Enabled
    {
        get { return isEnabled; }
        set
        {
            isEnabled = value;
            camera.enabled = value;
            audioListener.enabled = value;
        }
    }

    private bool isEnabled;
    private Camera camera;
    private AudioListener audioListener;

    void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        audioListener = GetComponentInChildren<AudioListener>();
    }
}