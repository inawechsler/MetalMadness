using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour, IUpgrade
{
    public bool isEventCounter { get; } = false;

    public void ApplyUpgrade(TopDownController controller)
    {

        controller.SetMaxSpeedCap(controller.currentMaxSpeedCap * 1.25f);
    }

    public bool CounteractState(IState state)
    {
        return false;
    }

}
