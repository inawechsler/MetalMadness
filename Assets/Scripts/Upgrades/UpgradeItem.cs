using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour 
{
    public Button Button;
    public IUpgrade upgrade { get; private set; }
    public int priceOnShop;
    public int maxCapacity;
    public string description;

    private void Awake()
    {
        upgrade = GetComponent<IUpgrade>();
        Button = GetComponent<Button>();

   
    }

}
