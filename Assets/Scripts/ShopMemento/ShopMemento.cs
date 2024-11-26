using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMemento
{
    public string upgrade;
    public Button button;
    public ShopMemento(string upgrade, Button button)
    {
        this.upgrade = upgrade;
        this.button = button;
    }   
}
