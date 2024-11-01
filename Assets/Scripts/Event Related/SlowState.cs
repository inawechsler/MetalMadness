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
            controller.isOnState = true;
        }
    }

    public void ExitState(TopDownController controller)
    {
        controller.isOnState = false;
        //controller.SetDriftFactor(controller.currentDriftFactor);
    }

    public void UpdateState(TopDownController controller)
    {
        if(controller.isOnState)
        {
            controller.SetMaxSpeedCap(slowedAcceleration);
        }
    }
}
