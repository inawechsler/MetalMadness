using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMemento
{
    public IUpgrade upgrade;
    public Button button;
    public ShopMemento(IUpgrade upgrade, Button button)
    {
        this.upgrade = upgrade;
        this.button = button;
    }   
}
