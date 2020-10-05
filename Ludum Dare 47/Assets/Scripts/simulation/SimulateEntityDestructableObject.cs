using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;
using Random = System.Random;

public class SimulateEntityDestructableObject : SimulatedEntityBase
{
    private List<ThrowableSimulateState> states = new List<ThrowableSimulateState>();
    private int lastStateIdx = -1;
    private ThrowableObject _throwableObject;


    public GameObject[] cells;

    private bool isPause;

    private bool isDestroyed;

    private Rigidbody rb;
    
    private void Awake()
    {
        _throwableObject = GetComponent<ThrowableObject>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        isDestroyed = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        isDestroyed = true;
        
         // if (other.impulse.magnitude > 10)
         // {
         //
         //    
         // }
    }

    public override void TriggerSimulate(PlaybackMode mode)
    {
        switch (mode)
        {
            case PlaybackMode.Pause:
                _throwableObject.rb.isKinematic = true;
                isPause = true;
                break;

            case PlaybackMode.PlayAndRecord:
                if (isPause)
                {
                    if (!_throwableObject.taken)
                    {
                        _throwableObject.rb.isKinematic = false;
                    }

                    _throwableObject.rb.velocity = states[lastStateIdx].velocity;
                    isPause = false;
                }

                if (lastStateIdx < states.Count - 1)
                {
                    states.RemoveRange(lastStateIdx, states.Count - lastStateIdx);
                }

                ThrowableSimulateState state = new ThrowableSimulateState();
                state.position = transform.position;
                state.rotation = transform.rotation;
                state.velocity = _throwableObject.rb.velocity;
                state.isDestroyed = isDestroyed;
                
                states.Add(state);

                lastStateIdx = states.Count - 1;

                break;
            case PlaybackMode.FastForward:
                TryExecuteCommand();
                break;
            case PlaybackMode.Rewind:
                TryRestoreCommand();
                break;
        }


        if (currentIsDestroy != isDestroyed)
        {
            foreach (var cell in cells)
            {
                cell.SetActive(isDestroyed);

                //float min = 0;
                //float max = 1;
                //var myVector = new Vector3(Random.Range(min, max), Random.Range(min, max), UnityEngine.Random.Range(min, max))
                
                cell.GetComponent<Rigidbody>().velocity = rb.velocity;
                //cell.GetComponent<Rigidbody>().velocity = myVector;
            }
        
            rb.detectCollisions = !isDestroyed;
            rb.isKinematic = isDestroyed;
            GetComponent<MeshRenderer>().enabled = !isDestroyed;
            
            
            
            currentIsDestroy = isDestroyed;
        }
    }

    private bool currentIsDestroy;
    
    private void TryExecuteCommand()
    {
        if (lastStateIdx == states.Count - 1)
            return;

        ThrowableSimulateState state = states[lastStateIdx];
        transform.position = state.position;
        transform.rotation = state.rotation;
        _throwableObject.rb.velocity = state.velocity;
        isDestroyed = state.isDestroyed;

        lastStateIdx++;
    }

    private void TryRestoreCommand()
    {
        if (lastStateIdx == 0)
            return;

        ThrowableSimulateState state = states[lastStateIdx];

        transform.position = state.position;
        transform.rotation = state.rotation;
        _throwableObject.rb.velocity = state.velocity;
        isDestroyed = state.isDestroyed;
        lastStateIdx--;
    }
}