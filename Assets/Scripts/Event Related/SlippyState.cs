using UnityEngine;

public class SlippyState : MonoBehaviour, IState
{
    public float slipperyDrift = 1f;
    private bool isOnState = false;

    public Color color { get; set; } = new Color(113, 181, 236, 20);

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
        controller.SetDriftFactor(controller.currentDriftFactor);
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
