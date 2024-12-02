using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverBoost : MonoBehaviour, IUpgrade
{
    public bool isEventCounter { get; } = false;

    public float newSpeedOutsideLimits { get; } = 14;

    public void ApplyUpgrade(TopDownController controller)
    {
        controller.SetMinSpeedCap(newSpeedOutsideLimits);

    }

    public bool CounteractState(IState state)
    {
        return false;
    }

}
