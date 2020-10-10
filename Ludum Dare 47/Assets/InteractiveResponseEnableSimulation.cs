using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveResponseEnableSimulation : InteractiveResponse
{
    public override void DoResponseAction()
    {
        SimulationController.Instance.ActivateSimulation();
    }

    public override bool IsAvailable()
    {
        return true;
    }
}
