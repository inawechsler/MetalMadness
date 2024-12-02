using System.Collections;
using UnityEngine;

public class WindyState : MonoBehaviour, IState
{
    [SerializeField] private float lateralSlideMultiplier = 1.1f; // Multiplicador base del deslizamiento lateral.
    [SerializeField] private float windBurstForce = 150f; // Fuerza adicional de las r�fagas de viento.
    [SerializeField] private float burstDuration = 2f; // Duraci�n de cada r�faga de viento.
    [SerializeField] private float burstIntervalMin = 2.0f; // Intervalo m�nimo entre r�fagas.
    [SerializeField] private float burstIntervalMax = 5.0f; // Intervalo m�ximo entre r�fagas.

    private bool isWindBurstActive = false;
    private float currentBurstDirection = 0f;


    public bool isClimateAffected { get; set; } = true;

    public void ClimateStateSet(ParticleSystem stateParticle)
    {
        if(stateParticle == null) { Debug.Log("SASAS"); }
        stateParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        stateParticle.Play();
    }

    public void EnterState(TopDownController controller)
    {
        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = true;

           StartCoroutine(WindBurstRoutine(controller));
        }
    }

    public void ExitState(TopDownController controller)
    {
        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = false;

            StopCoroutine(WindBurstRoutine(controller));
            
        }
    }

    public void UpdateState(TopDownController controller)
    {
        if (controller.isOnState)
        {
            if (isWindBurstActive)
            {

                controller.ApplyLateralSlide(currentBurstDirection * windBurstForce);
            }
        }
    }

    private IEnumerator WindBurstRoutine(TopDownController controller)
    {
        while (controller.isOnState)
        {
            // Esperar un intervalo aleatorio antes de la pr�xima r�faga.
            yield return new WaitForSeconds(/*Random.Range(burstIntervalMin, burstIntervalMax)*/burstDuration);
            Debug.Log("Rafagaq");
            // Activar una r�faga de viento con una direcci�n aleatoria.
            isWindBurstActive = true;
            int random = Random.Range(-360, 360);
            currentBurstDirection = random; // 1 para derecha, -1 para izquierda.

            // Mantener la r�faga durante la duraci�n especificada.
            yield return new WaitForSeconds(3f);

            // Desactivar la r�faga de viento.
            isWindBurstActive = false;
            currentBurstDirection = 0;
            Debug.Log("afuera");
        }

    }
}