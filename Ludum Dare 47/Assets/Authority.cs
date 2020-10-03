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
            _camera.enabled = value;
            _audioListener.enabled = value;
        }
    }

    private bool isEnabled;
    private Camera _camera;
    private AudioListener _audioListener;
    
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _audioListener = GetComponentInChildren<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}