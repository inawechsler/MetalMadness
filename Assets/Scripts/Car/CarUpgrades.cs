using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarUpgrades : MonoBehaviour
{
    private List<IUpgrade> activeUpgradeList = new List<IUpgrade>();
    private TopDownController controller;
    public EntityType type;
    bool hasBought;

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
        // Verificar si la mejora está bloqueada temporalmente.
        if (hasBought)
        {
            Debug.LogWarning("You can't buy another upgrade yet. Please wait.");
            return;
        }

        // Verificar si la mejora es Event Counter.
        if (upgrade.isEventCounter)
        {
            // Si ya existe una mejora Event Counter activa, rechazar la compra.
            if (activeUpgradeList.Any(u => u.GetType() == upgrade.GetType()))
            {
                Debug.LogWarning("This Event Counter upgrade is already active.");
                return;
            }

            // Si existe otra Event Counter activa, removerla antes de equipar la nueva.
            IUpgrade upgradeToRemove = GetActiveCounterUpgrade();
            if (upgradeToRemove != null)
            {
                RemoveUpgrade(upgradeToRemove);
            }
        }


        // Agregar la mejora a la lista y aplicarla.
        activeUpgradeList.Add(upgrade);
        upgrade.ApplyUpgrade(controller);

        Debug.Log($"Upgrade added: {upgrade.GetType().Name}");

        // Iniciar el temporizador para bloquear nuevas compras.
        StartCoroutine(manageBoolEntered());
    }
    public void RemoveUpgrade(IUpgrade upgrade)
    {
        if (upgrade != null)
        {
            activeUpgradeList.Remove(upgrade);
        }
    }

    public bool CanUseUpgrade(UpgradeItem item)
    {
        // Verificar si ya se ha alcanzado el límite de acumulación para la mejora.
        int upgradeCount = activeUpgradeList.Where(u => !u.isEventCounter).Count(u => u.GetType() == item.upgrade.GetType());
        if (upgradeCount > item.maxCapacity)
        {
            Debug.LogWarning($"You can't have more than 3 of this upgrade: {item.upgrade.GetType().Name}");
            return false;
        } else
        {
            return true;
        }
    }

    IEnumerator manageBoolEntered()
    {
        hasBought = true;

        yield return new WaitForSeconds(3f);

        hasBought = false;
    }

    public bool HasUpgradeToCounteract(IState state)
    {

        return activeUpgradeList.Any(upgrade => upgrade.CounteractState(state));
    }
}
