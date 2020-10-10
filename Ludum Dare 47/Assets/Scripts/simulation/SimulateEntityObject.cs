using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;


public class SimulateEntityObject : SimulatedEntityBase
{
    private List<ThrowableSimulateState> states = new List<ThrowableSimulateState>();
    private int lastStateIdx = -1;

    private bool isPause;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        defaultLocalPostion = transform.localPosition;
    }


    private Vector3 defaultLocalPostion;
    
    private void OnEnable()
    {
        isPause = false;
        states.Clear();
        lastStateIdx = -1;
        transform.localPosition = defaultLocalPostion;
        _rigidbody.isKinematic = false;
    }
    
    public override void TriggerSimulate(PlaybackMode mode)
    {
        switch (mode)
        {
            case PlaybackMode.Pause:
                _rigidbody.isKinematic = true;
                isPause = true;
                break;

            case PlaybackMode.PlayAndRecord:
                if (isPause && states.Count > 0)
                {
                    _rigidbody.isKinematic = false;
                    _rigidbody.velocity = states[lastStateIdx].velocity;
                    isPause = false;
                }

                if (lastStateIdx < states.Count - 1)
                {
                    states.RemoveRange(lastStateIdx, states.Count - lastStateIdx);
                }

                AddState();

                break;
            case PlaybackMode.FastForward:
                if (states.Count == 0 || lastStateIdx >= states.Count - 1)
                {
                    if (isPause)
                    {
                        _rigidbody.isKinematic = false;
                        if (states.Count == 0)
                        {
                            _rigidbody.velocity = Vector3.zero;
                        }
                        else
                        {
                            _rigidbody.velocity = states[lastStateIdx].velocity;
                        }

                        isPause = false;
                    }

                    AddState();
                }
                else
                {
                    TryExecuteCommand();
                }

                break;
            case PlaybackMode.Rewind:
                _rigidbody.isKinematic = true;
                TryRestoreCommand();
                break;
        }
    }

    private void AddState()
    {
        ThrowableSimulateState state = new ThrowableSimulateState();
        state.position = transform.position;
        state.rotation = transform.rotation;
        state.velocity = _rigidbody.velocity;
        states.Add(state);

        lastStateIdx = states.Count - 1;
    }

    private void TryExecuteCommand()
    {
        if (lastStateIdx == states.Count - 1)
            return;

        ThrowableSimulateState state = states[lastStateIdx];
        transform.position = state.position;
        transform.rotation = state.rotation;
        _rigidbody.velocity = state.velocity;

        lastStateIdx++;
    }

    private void TryRestoreCommand()
    {
        if (lastStateIdx <= 0)
            return;

        ThrowableSimulateState state = states[lastStateIdx];

        transform.position = state.position;
        transform.rotation = state.rotation;
        _rigidbody.velocity = state.velocity;
        lastStateIdx--;
    }
}