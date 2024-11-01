using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSkidMarks : MonoBehaviour
{
    TopDownController controller;
    TrailRenderer trRenderer;
    ParticleSystem particleSystemSmoke;

    ParticleSystem.EmissionModule emissionModule;

    float particleEmissionRate = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<TopDownController>();

        trRenderer = GetComponent<TrailRenderer>();
        if (trRenderer == null)
        {

        }

        particleSystemSmoke = GetComponent<ParticleSystem>();
        if (particleSystemSmoke == null)
        {

        }
        else
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
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);

        if (particleSystemSmoke != null)
        {
            emissionModule.rateOverTime = particleEmissionRate;
        }

        if (controller.isTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            particleEmissionRate = isBraking ? 30 : Mathf.Abs(lateralVelocity) * 2;

            if (trRenderer != null)
            {
                trRenderer.emitting = true;
            }
        }
        else
        {
            if (trRenderer != null)
            {
                trRenderer.emitting = false;
            }
        }
    }
}
