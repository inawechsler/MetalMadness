using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoxShopEnter : MonoBehaviour
{
    List<IBoxObserver> boxObservers = new List<IBoxObserver>();
    private BoxCanvasManager boxCanvasManager;
    private CarUpgrades playerCarUpgrades;
    private CarUpgrades aiCarUpgrades;
    //si verdadero, va a entrar, falso, va a salir
    private bool pitState;
    private bool isNotifying = false;
    bool hasEntered;

    private void Awake()
    {
        boxCanvasManager = GameObject.FindWithTag("EventSystem").GetComponent<BoxCanvasManager>();
        boxObservers = FindObjectsOfType<MonoBehaviour>().OfType<IBoxObserver>().ToList();

        boxCanvasManager.onClickedPurchased.AddListener(() => NotifyExitBox(EntityType.Player, playerCarUpgrades));

        foreach (var observer in boxObservers)
        {
            Debug.Log("Found observer: " + observer.GetType().Name);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CarUpgrades carUpgrades = collision.gameObject.GetComponent<CarUpgrades>();

        if(carUpgrades != null)
        {
            pitState = true;
            if (collision.gameObject.CompareTag("Player") && !hasEntered)
            {
                playerCarUpgrades = carUpgrades;
                NotifyObserver(pitState, EntityType.Player, playerCarUpgrades);//Si es Player, en el Enter le manda PitState true, es decir está entrando y el enum que le pasa es Player
            }

            if (collision.gameObject.CompareTag("AI"))
            {
                aiCarUpgrades = carUpgrades;
                NotifyObserver(pitState, EntityType.Ai, aiCarUpgrades);//Si es AI, en el Enter le manda PitState true, es decir está entrando y el enum que le pasa es AI
            }
        }

    }
    IEnumerator manageBoolEntered()
    {
        hasEntered = true;

        yield return new WaitForSeconds(3f);

        hasEntered = false;
    }
    private void NotifyExitBox(EntityType type, CarUpgrades carUpgrades)//Evento suscripto al click de Purchase de BoxCanvasManager, al tocarlo recibe pitState en falso, por lo que maneja la salida de Pits
    {
        StartCoroutine(manageBoolEntered());
        pitState = false;
        NotifyObserver(pitState, type, carUpgrades);
    }

    public void UnregisterObserver()
    {
        boxObservers.RemoveAll(o => o != null);
    }

    public void NotifyObserver(bool pitState, EntityType type, CarUpgrades carUpgrades)
    {

        if (isNotifying) return;
        isNotifying = true;


        foreach (var observer in boxObservers)
        {
            if (pitState)
                observer.OnBoxEntered(type, carUpgrades);
            else
                observer.OnBoxExit(type, carUpgrades);
        }

        isNotifying = false;
    }
}

