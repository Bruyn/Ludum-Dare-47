using System.Collections.Generic;
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

    public Rigidbody rb;
    private bool isPause;

    public override void TriggerSimulate(PLaybackMode mode)
    {
        switch (mode)
        {
            case PLaybackMode.Pause:
                rb.isKinematic = true;
                isPause = true;
                break;

            case PLaybackMode.PlayAndRecord:
                if (isPause)
                {
                    rb.isKinematic = false;
                    rb.velocity = states[lastStateIdx].velocity;
                    isPause = false;
                }
                
                if (lastStateIdx < states.Count - 1)
                {
                    states.RemoveRange(lastStateIdx, states.Count - lastStateIdx);
                }

                ThrowableSimulateState state = new ThrowableSimulateState();
                state.position = transform.position;
                state.rotation = transform.rotation;
                state.velocity = rb.velocity;
                states.Add(state);

                lastStateIdx = states.Count - 1;

                break;
            case PLaybackMode.FastForward:
                TryExecuteCommand();
                break;
            case PLaybackMode.Rewind:
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
        rb.velocity = state.velocity;

        lastStateIdx++;
    }

    private void TryRestoreCommand()
    {
        if (lastStateIdx == 0)
            return;

        ThrowableSimulateState state = states[lastStateIdx];

        transform.position = state.position;
        transform.rotation = state.rotation;
        rb.velocity = state.velocity;
        lastStateIdx--;
    }
}