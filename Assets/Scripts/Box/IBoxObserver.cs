using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoxObserver
{
    void OnBoxEntered(EntityType type, CarUpgrades carUpgrades);    
    void OnBoxExit(EntityType type, CarUpgrades carUpgrades);    
}

public enum EntityType
{
    Ai, Player
}