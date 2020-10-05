using System;
using System.Collections.Generic;
using Interactables;
using UnityEngine;
using Random = System.Random;

public class SimulateEntityDestructableWall : SimulatedEntityBase
{
    private List<ThrowableSimulateState> states = new List<ThrowableSimulateState>();
    private int lastStateIdx = -1;
    

    public GameObject[] cells;

    private bool isPause;

    private bool isDestroyed;

    private Rigidbody rb;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        
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
                isPause = true;
                break;

            case PlaybackMode.PlayAndRecord:
                if (isPause)
                {
                    

                    
                    isPause = false;
                }

                if (lastStateIdx < states.Count - 1)
                {
                    states.RemoveRange(lastStateIdx, states.Count - lastStateIdx);
                }

                ThrowableSimulateState state = new ThrowableSimulateState();
                state.position = transform.position;
                state.rotation = transform.rotation;
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
            rb.isKinematic = true;
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
        isDestroyed = state.isDestroyed;
        lastStateIdx--;
    }
}