using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class SlowState : MonoBehaviour, IState
{
    private float slowedAcceleration = 11f;
    public void EnterState(TopDownController controller)
    {

        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.SetLastSpeedBefChange(controller.currentMaxSpeedCap);
            
            if (controller.gameObject.CompareTag("Player")) Debug.Log("Enter: " + controller.lastSpeedBefChange);


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
