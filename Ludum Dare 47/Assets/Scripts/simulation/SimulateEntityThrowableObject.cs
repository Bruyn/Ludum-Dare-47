using System.Collections.Generic;
using Interactables;
using UnityEngine;


public class ThrowableSimulateState
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
}

public class SimulateEntityThrowableObject : SimulatedEntityBase
{
    private List<ThrowableSimulateState> states = new List<ThrowableSimulateState>();
    private int lastStateIdx = -1;
    public ThrowableObject ThrowableObject;
    
    private bool isPause;

    public override void TriggerSimulate(PlaybackMode mode)
    {
        switch (mode)
        {
            case PlaybackMode.Pause:
                ThrowableObject.rb.isKinematic = true;
                isPause = true;
                break;

            case PlaybackMode.PlayAndRecord:
                if (isPause)
                {
                    if (!ThrowableObject.taken)
                    {
                        ThrowableObject.rb.isKinematic = false;                        
                    }
                    
                    ThrowableObject.rb.velocity = states[lastStateIdx].velocity;
                    isPause = false;
                }
                
                if (lastStateIdx < states.Count - 1)
                {
                    states.RemoveRange(lastStateIdx, states.Count - lastStateIdx);
                }

                ThrowableSimulateState state = new ThrowableSimulateState();
                state.position = transform.position;
                state.rotation = transform.rotation;
                state.velocity = ThrowableObject.rb.velocity;
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
    }

    private void TryExecuteCommand()
    {
        if (lastStateIdx == states.Count - 1)
            return;

        ThrowableSimulateState state = states[lastStateIdx];
        transform.position = state.position;
        transform.rotation = state.rotation;
        ThrowableObject.rb.velocity = state.velocity;

        lastStateIdx++;
    }

    private void TryRestoreCommand()
    {
        if (lastStateIdx == 0)
            return;

        ThrowableSimulateState state = states[lastStateIdx];

        transform.position = state.position;
        transform.rotation = state.rotation;
        ThrowableObject.rb.velocity = state.velocity;
        lastStateIdx--;
    }
}