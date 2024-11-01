using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxShopEnter : MonoBehaviour
{
    List<IBoxObserver> boxObservers = new List<IBoxObserver>();
    private BoxCanvasManager boxCanvasManager;

    //si verdadero, va a entrar, va a salir
    private bool pitState;

    private void Awake()
    {
        boxCanvasManager = GameObject.FindWithTag("EventSystem").GetComponent<BoxCanvasManager>();
        boxObservers = FindObjectsOfType<MonoBehaviour>().OfType<IBoxObserver>().ToList();

        boxCanvasManager.onClickedPurchased.AddListener(NotifyExitBox);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pitState = true;
            NotifyObserver(pitState);

        }
    }

    private void NotifyExitBox()
    {
        pitState = false;
        NotifyObserver(pitState);
    }

    public void RegisterObserver()
    {
        foreach (IBoxObserver observer in boxObservers)
        {
            boxObservers.Add(observer);
        }
    }

    public void UnregisterObserver()
    {
        boxObservers.RemoveAll(o => o != null);
    }

    private bool isNotifying = false;

    public void NotifyObserver(bool hasEntered)
    {
        if (isNotifying) return;
        isNotifying = true;

        foreach (var observer in boxObservers)
        {
            if (hasEntered)
                observer.OnBoxEntered();
            else
                observer.OnBoxExit();
        }

        isNotifying = false;
    }
}

