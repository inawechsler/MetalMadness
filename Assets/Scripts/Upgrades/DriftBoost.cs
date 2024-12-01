using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftBoost : MonoBehaviour, IUpgrade
{
    public bool isEventCounter { get; } = false;
    

    public void ApplyUpgrade(TopDownController controller)
    { 

        controller.SetDriftFactorTemporarily(controller.driftFactor * (-1.10f));

    }

    public bool CounteractState(IState state)
    {
        return false;
    }

}
