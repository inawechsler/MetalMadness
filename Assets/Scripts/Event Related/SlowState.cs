using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SlowState : MonoBehaviour, IState
{
    private float slowedAcceleration = 11f;

    public Color color { get; set; } = new Color(91, 70, 65, 20);

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
