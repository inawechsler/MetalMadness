using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour, IUpgrade
{
    public bool isEventCounter { get; } = false;

    public int upgradesApplied { get; private set; }

    public void ApplyUpgrade(TopDownController controller)
    {
        upgradesApplied++;

        controller.SetMaxSpeedCap(controller.currentMaxSpeedCap * 1.10f);

    }

    public bool CounteractState(IState state)
    {
        return false;
    }

}
