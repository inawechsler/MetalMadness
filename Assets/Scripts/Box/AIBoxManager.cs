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
        upgradeList = FindObjectsOfType<MonoBehaviour>().OfType<IUpgrade>().Where(upgrade => upgrade.GetType().Name != "IdealPath").ToList();
        boardUIHandler = FindAnyObjectByType<LeadeBoardUIHandler>();
    }
    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Player) return; //Si el player entra, retorna

        upgradeToApply = ManageUpgradeToApply(carUpgrades); //Consigue el upgrade a aplicar

        carUpgrades.AddUpgrade(upgradeToApply);
        //boardUIHandler.UpdateImage(upgradeToApply, carUpgrades );
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
        
        int randomUpgradeIndex = Random.Range(0, upgradeList.Count); // Número random del 0 a cantidad de States
        IUpgrade upgrade = upgradeList[randomUpgradeIndex];

        return upgrade;
    }


}
