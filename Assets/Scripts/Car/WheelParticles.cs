using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSkidMarks : MonoBehaviour
{
    TopDownController controller;
    TrailRenderer trRenderer;
    ParticleSystem particleSystemSmoke;

    ParticleSystem.EmissionModule emissionModule; //M�dulo del sistema de part�culas que permite ajustar la tasa de emisi�n del humo.

    float particleEmissionRate = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<TopDownController>();

        trRenderer = GetComponent<TrailRenderer>();

        particleSystemSmoke = GetComponent<ParticleSystem>();

        if(particleSystemSmoke != null)
        {

            emissionModule = particleSystemSmoke.emission;
            emissionModule.rateOverDistance = 0;

        }


        if (trRenderer != null)
        {
            trRenderer.emitting = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5); //Lerp entre 0 y valor que queda bien para efecto de desvanecimiento

        if (particleSystemSmoke != null)
        {
            emissionModule.rateOverTime = particleEmissionRate;
        }

        if (controller.isTireScreeching(out float lateralVelocity, out bool isBraking)) //Out para evitar pasar referencia, uso puntero, toma los valores de este script
        {
            particleEmissionRate = isBraking ? 30 : Mathf.Abs(lateralVelocity) * 2; // Si estoy frenando es 30, si no, la emisi�n es proporcional a la velocidad lateral

            trRenderer.emitting = true;

        }
        else
        {
            trRenderer.emitting = false;

        }
    }
}
