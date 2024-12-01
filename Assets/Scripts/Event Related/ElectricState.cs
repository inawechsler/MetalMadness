using UnityEngine;

public class ElectricState : MonoBehaviour, IState
{
    private float accelerationMultiplier = 15f; // Aceleración constante extra.
    private float steeringReductionFactor = 0.5f; // Reduce la capacidad de girar.
    private float increasedDriftFactor = 1.02f; // Incremento en el drift para mayor deslizamiento.
    private float lateralSlideMultiplier = 1.1f; // Multiplica la velocidad lateral.

    public Color color { get; set; } = new Color(91 / 255f, 70 / 255f, 65 / 255f, 20 / 255f);

    public void ClimateStateSet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void EnterState(TopDownController controller)
    {
        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = true;
            //controller.SetDriftFactor(increasedDriftFactor); // Incrementar el drift.
        }
    }

    public void ExitState(TopDownController controller)
    {
        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = false;
            //controller.SetDriftFactor(0.85f); // Restaurar drift normal.
        }
    }

    public void UpdateState(TopDownController controller)
    {
        if (controller.isOnState)
        {
            // Forzar aceleración constante hacia adelante.
            //controller.SetAccelerationInput(accelerationMultiplier);

            // Incrementar deslizamiento lateral.
            controller.ApplyLateralSlide(lateralSlideMultiplier);
        }
    }



}