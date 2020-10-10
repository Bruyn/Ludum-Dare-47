using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackAreaScript : MonoBehaviour
{
    public GameObject playBackArea;
    
    private SimulationController simulationController;
    
    void Start()
    {
        simulationController = SimulationController.Instance;
        if (simulationController.IsSimulationActive())
            playBackArea.SetActive(true);
    }

    void Update()
    {
        playBackArea.SetActive(simulationController.IsSimulationActive());
    }
}
