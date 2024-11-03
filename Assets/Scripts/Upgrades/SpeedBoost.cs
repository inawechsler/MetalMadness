using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour, IUpgrade
{
    public bool isEventCounter { get; } = false;

    public void ApplyUpgrade(TopDownController controller)
    {
        Debug.Log(controller.currentMaxSpeedCap);
        controller.SetMaxSpeedCap(controller.currentMaxSpeedCap * 20);
    }

    public bool CounteractState(IState state)
    {
        return false;
    }

}
