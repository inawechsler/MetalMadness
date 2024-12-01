using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarUpgrades : MonoBehaviour
{
    private List<IUpgrade> activeUpgradeList = new List<IUpgrade>();
    private TopDownController controller;
    public EntityType type;


    private void Start()
    {
        controller = GetComponent<TopDownController>();
    }

    IUpgrade GetActiveCounterUpgrade()
    {
        var activeUpgrade = activeUpgradeList.FirstOrDefault(upgrade => upgrade.isEventCounter);
        return activeUpgrade;
    }

    public void AddUpgrade(IUpgrade upgrade)
    {
        if(upgrade.isEventCounter)
        {
            IUpgrade upgradeToRemove = GetActiveCounterUpgrade();

            RemoveUpgrade(upgradeToRemove);
        }

        if (!activeUpgradeList.Contains(upgrade))
        {
            activeUpgradeList.Add(upgrade);
            upgrade.ApplyUpgrade(controller);
        }
    }

    public void RemoveUpgrade(IUpgrade upgrade)
    {
        if (upgrade != null)
        {
            activeUpgradeList.Remove(upgrade);
        }
    }

    public bool HasUpgradeToCounteract(IState state)
    {

        return activeUpgradeList.Any(upgrade => upgrade.CounteractState(state));
    }
}
