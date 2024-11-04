using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IBoxObserver
{
    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if(type == EntityType.Ai) return;
        Time.timeScale = 0f;
    }

    public void OnBoxExit(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return;
        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
