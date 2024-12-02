using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Canvas : MonoBehaviour, IBoxObserver
{
    
    [SerializeField] GameObject lightCanvas, canvas;
    [SerializeField] Image[] raceLights;
    [SerializeField] TextMeshProUGUI engineText;



    public void OnBoxEntered(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return;

        canvas.gameObject.SetActive(false);
    }

    public void OnBoxExit(EntityType type, CarUpgrades carUpgrades)
    {
        if (type == EntityType.Ai) return;

        canvas.gameObject.SetActive(true);
    }

    void Start()
    {
        if(engineText == null)
        {
            //engineText = canvas.G
        }

        LevelManager.Instance.LightSet(raceLights, lightCanvas);
    }
    //    Update is called once per frame
    void Update()
    {
        engineText.text = LevelManager.Instance.enginePiecesCollected.ToString();
    }
}
