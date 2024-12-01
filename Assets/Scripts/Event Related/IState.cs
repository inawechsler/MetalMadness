using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState 
{
    Color color {  get; set; }
    void UpdateState(TopDownController controller);

    void EnterState(TopDownController controller);
    void ExitState(TopDownController controller);
    

}
