using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class BoxCanvasManager : MonoBehaviour, IBoxObserver
{
    private GameObject boxCanvas;
    [SerializeField] Button purchaseButton;

    [SerializeField] Button antiSlipperyState;
   
    private List<IUpgrade> upgradeList = new List<IUpgrade>();

    private Dictionary<string, IUpgrade> upgradeDictionary = new Dictionary<string, IUpgrade>();

    IUpgrade currentUpgrade;
    public UnityEvent onClickedPurchased { get; private set; } = new UnityEvent();

    CarUpgrades carUpgrades;

    private void Awake()
    {
        purchaseButton.onClick.AddListener(OnBoxExit);

        carUpgrades = GameObject.FindWithTag("Player").GetComponent<CarUpgrades>();

        antiSlipperyState.onClick.AddListener( delegate { ManageButton("WheelsSpikes");} );

        boxCanvas = GameObject.FindWithTag("ShopCanvas");

        boxCanvas.SetActive(false);
    }

    public void OnBoxEntered()
    {
        boxCanvas.SetActive(true);

        upgradeList = FindObjectsOfType<MonoBehaviour>().OfType<IUpgrade>().ToList();

        foreach (var upgrade in upgradeList)
        {
            GetType().Name.ToString();
            upgradeDictionary.Add(upgrade.GetType().Name, upgrade);
        }
    }

    public void OnBoxExit()
    {
        boxCanvas.SetActive(false);
        onClickedPurchased?.Invoke();
    }
    void ManageButton(string buttonName)
    {
        if (upgradeDictionary.TryGetValue(buttonName, out IUpgrade upgrade))
        {
            currentUpgrade = upgrade;
        }
        else
        {
            Debug.LogWarning($"Upgrade '{buttonName}' no encontrado.");
        }

        carUpgrades.AddUpgrade(currentUpgrade);

    }

}
