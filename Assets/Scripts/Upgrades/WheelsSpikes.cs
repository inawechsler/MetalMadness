using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelsSpikes : MonoBehaviour, IUpgrade
{
    public bool isEventCounter { get; private set; } = true;
    public void ApplyUpgrade(TopDownController controller)
    {
        
    }
    public bool CounteractState(IState state)
    {
        return state is SlippyState;
    }
}
