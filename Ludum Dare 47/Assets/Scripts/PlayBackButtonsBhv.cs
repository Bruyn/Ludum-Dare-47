using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackButtonsBhv : MonoBehaviour
{
    private Animator _animator;
    private SimulationController _simulationController;
    void Start()
    {
        _animator = GetComponent<Animator>();
        var simObj = GameObject.FindWithTag("simulationController");
        _simulationController = simObj.GetComponent<SimulationController>();
    }

    void Update()
    {
        _animator.SetInteger("playBackState", (int)_simulationController.GetCurrentMode());
        _animator.SetBool("isFast", _simulationController.IsFastPlayBack());
    }
}
