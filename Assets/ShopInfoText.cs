using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopInfoText : MonoBehaviour
{
    TextMeshProUGUI infoText;
    // Start is called before the first frame update
    void Start()
    {
        infoText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void SetTextInfo(UpgradeItem upgradeItem)
    {
        if(upgradeItem == null)
        {
            infoText.text = "R to restore last purchase";
        }
        infoText.text = upgradeItem.description;

    }
}
