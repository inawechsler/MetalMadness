using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour 
{
    public Button Button;
    [SerializeField] public TextMeshProUGUI textMesh;
    public IUpgrade upgrade { get; private set; }
    public int priceOnShop;

    private void Awake()
    {
        upgrade = GetComponent<IUpgrade>();
        Button = GetComponent<Button>();

        PriceOnButton();
        
    }

    public void PriceOnButton()
    {
        textMesh.text = priceOnShop.ToString();
    }


}
