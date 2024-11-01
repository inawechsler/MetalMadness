using UnityEngine;

public class SlippyState : MonoBehaviour, IState
{
    public float slipperyDrift = 1f;
    private bool isOnState = false;
    public void EnterState(TopDownController controller)
    {

        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = true;
        }
    }

    public void ExitState(TopDownController controller)
    {
        controller.isOnState = false;  // Desactivar el estado resbaladizo
        controller.SetDriftFactor(controller.currentDriftFactor);
        //StateManager.Instance.ChangeCurrentState(StateManager.Instance.normalState);  // Cambiar al estado normal
    }

    public void UpdateState(TopDownController controller)
    {
        if (controller.isOnState)
        {
            controller.SetDriftFactor(slipperyDrift);
            controller.rb2D.drag = 0;
            controller.SetAccelerationInput(Mathf.Clamp(controller.GetAccelerationInput(), 0, 1f));
        }
    }
}
