using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgrade
{
    bool isEventCounter { get; }
    void ApplyUpgrade(TopDownController controller);
    bool CounteractState(IState state);
}
