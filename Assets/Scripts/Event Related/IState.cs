using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    bool isClimateAffected {  get; set; }
    void UpdateState(TopDownController controller);

    void EnterState(TopDownController controller);
    void ExitState(TopDownController controller);

    void ClimateStateSet(ParticleSystem particleSystem);
    

}
