using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeLineScript : MonoBehaviour
{
    public float timeLimitSec = 60f;
    public Text timeTextBox;
    public Slider progressSlider;
    
    private PlayerSwitchMng playersManager;

    void Start()
    {
        var mngObject = GameObject.FindWithTag("playerSwitchManager");
        if (mngObject != null)
        {
            playersManager = mngObject.GetComponent<PlayerSwitchMng>();
        }
    }

    void Update()
    {
        if (playersManager == null)
            return;

        var currentAuth = playersManager.GetCurrentAuthority();
        if (currentAuth == null)
            return;

        var inputController = currentAuth.GetComponent<InputController>();
        var commandIdx = inputController.GetCurrentCommandIdx();
        var currTimeIn = Time.fixedDeltaTime * commandIdx;
        
        progressSlider.value = currTimeIn / timeLimitSec;
        TimeSpan timeSpan = TimeSpan.FromSeconds(currTimeIn);
        
        timeTextBox.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }
}