using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{

    void UpdateState(TopDownController controller);

    void EnterState(TopDownController controller);
    void ExitState(TopDownController controller);
    

}
