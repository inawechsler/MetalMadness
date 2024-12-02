using UnityEngine;

public class SlippyState : MonoBehaviour, IState
{
    public float slipperyDrift = 1f;
    private bool isOnState = false;
    public bool isClimateAffected { get; set; } = false;

    public void ClimateStateSet(ParticleSystem stateParticle)
    {
        stateParticle.Stop();
    }
    public void EnterState(TopDownController controller)
    {

        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = true;
            controller.SetDriftFactorTemporarily(slipperyDrift);
        }
    }

    public void ExitState(TopDownController controller)
    {
        controller.isOnState = false;  
        controller.RestoreDriftFactor();
    }

    public void UpdateState(TopDownController controller)
    {
        if (controller.isOnState)
        {

            controller.rb2D.drag = 0;
            controller.SetAccelerationInput(Mathf.Clamp(controller.GetAccelerationInput(), 0, 1f));
        }
    }
}
