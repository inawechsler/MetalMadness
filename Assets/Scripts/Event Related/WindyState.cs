using System.Collections;
using UnityEngine;

public class WindyState : MonoBehaviour, IState
{
    [SerializeField] private float lateralSlideMultiplier = 1.1f; // Multiplicador base del deslizamiento lateral.
    [SerializeField] private float windBurstForce = 400f; // Fuerza adicional de las r�fagas de viento.
    [SerializeField] private float burstDuration = 4f; // Duraci�n de cada r�faga de viento.
    [SerializeField] private float burstIntervalMin = 2.0f; // Intervalo m�nimo entre r�fagas.
    [SerializeField] private float burstIntervalMax = 5.0f; // Intervalo m�ximo entre r�fagas.

    private bool isWindBurstActive = false;
    private float currentBurstDirection = 0f;

    public bool isClimateAffected { get; set; } = true;

    public void ClimateStateSet(ParticleSystem stateParticle)
    {

        if(stateParticle == null) { Debug.Log("SASAS"); }

        gameObject.SetActive(true);

        if (stateParticle == null) { Debug.Log("SASAS"); }
        stateParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        stateParticle.Play();
    }

    public void EnterState(TopDownController controller)
    {
        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = true;

            controller.SetDriftFactorTemporarily(1f);
            StartCoroutine(WindBurstRoutine(controller));


        }
    }

    public void ExitState(TopDownController controller)
    {
        if (!controller.carUpgrades.HasUpgradeToCounteract(this))
        {
            controller.isOnState = false;
            controller.RestoreDriftFactor();
            StopCoroutine(WindBurstRoutine(controller));

            // Detener el sonido del viento al salir del estado.
            AudioManager.instance.StopSound("wind");
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
            
            yield return new WaitForSeconds(Random.Range(burstIntervalMin, burstIntervalMax));

            
            isWindBurstActive = true;
            currentBurstDirection = Random.Range(-1, 1); 

           
            AudioManager.instance.PlaySound("wind");

            
            yield return new WaitForSeconds(burstDuration);

            AudioManager.instance.StopSound("wind");



            isWindBurstActive = false;
            currentBurstDirection = 0;
        }
    }
}