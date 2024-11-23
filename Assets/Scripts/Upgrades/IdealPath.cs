using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IdealPath : MonoBehaviour, IUpgrade
{
    public TDAGraph Dijkstra;
    public bool isEventCounter { get; } = false;

    public Tilemap tilemap;

    public void Start()
    {
        Dijkstra = GameObject.FindWithTag("Managers").GetComponent<TDAGraph>();
        tilemap = GameObject.FindWithTag("Track").GetComponent<Tilemap>();
        Dijkstra.InitGraph(tilemap);
    }

    public void ApplyUpgrade(TopDownController controller)
    {
        Dijkstra.InitGraph(tilemap);
    }

    public bool CounteractState(IState state)
    {
        return false;
    }

}