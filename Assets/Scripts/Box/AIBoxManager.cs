using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class AIBoxManager : MonoBehaviour, IBoxObserver
{
    List<IUpgrade> upgradeList = new List<IUpgrade>();
    IUpgrade upgradeToApply;
    LeadeBoardUIHandler boardUIHandler;

    private void Awake()
    {
        upgradeList = FindObjectsOfType<MonoBehaviour>().OfType<IUpgrade>().ToList();
        boardUIHandler = FindAnyObjectByType<LeadeBoardUIHandler>();
    }
    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Player) return;

        upgradeToApply = ManageUpgradeToApply(carUpgrades);

        carUpgrades.AddUpgrade(upgradeToApply);
        boardUIHandler.UpdateImage(upgradeToApply, carUpgrades );
    }

    public void OnBoxExit(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Player) return;
    }

    IUpgrade ManageUpgradeToApply(CarUpgrades carUpgrades)
    {
        // Verifica que la lista no esté vacía
        if (upgradeList == null || upgradeList.Count == 0)
        {
            Debug.LogWarning("No hay upgrades disponibles en upgradeList.");
            return null; // Retorna null si no hay upgrades disponibles
        }

        int randomUpgradeIndex = Random.Range(0, upgradeList.Count); // Cambié el rango para incluir el último índice
        IUpgrade upgrade = upgradeList[randomUpgradeIndex];

        return upgrade;
    }


}
