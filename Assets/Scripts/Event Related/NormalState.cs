using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : MonoBehaviour, IState
{
    [SerializeField] private float normalDrift = 0.85f;

    public void EnterState(TopDownController controller)
    {
        
    }

    public void ExitState(TopDownController controller)
    {
        
    }

    public void UpdateState(TopDownController controller)
    {
        // Aquí se asegura de que el factor de drift vuelva al valor normal
        controller.SetDriftFactor(normalDrift);
    }
}
