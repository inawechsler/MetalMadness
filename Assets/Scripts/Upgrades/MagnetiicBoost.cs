using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticBoost : MonoBehaviour, IUpgrade
{
    public bool isEventCounter { get; } = false;

    public int newSphereRadius = 2;

    public void ApplyUpgrade(TopDownController controller)
    {
        if (controller.car.sphereCollider == null) return;
        controller.car.sphereCollider.radius = newSphereRadius;

    }

    public bool CounteractState(IState state)
    {
        return false;
    }

}
