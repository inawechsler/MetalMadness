using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SlowState : MonoBehaviour, IState
{
    private float slowedAcceleration = 11f;
    public void EnterState(TopDownController controller)
    {

        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.SetLastSpeedBefChange(controller.currentMaxSpeedCap);

            controller.SetMaxSpeedCap(slowedAcceleration);
        }
    }

    public void ExitState(TopDownController controller)
    {

        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = false;
            controller.SetDriftFactor(controller.currentDriftFactor);
            controller.SetMaxSpeedCap(controller.lastSpeedBefChange);
        }

    }

    public void UpdateState(TopDownController controller)
    {
        if(controller.isOnState)
        {

        }
    }
}
