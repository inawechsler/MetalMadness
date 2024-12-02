//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class IdealPath : MonoBehaviour, IUpgrade
//{
//    public TDAGraph Dijkstra;
    
//    public List<CheckPoints> checkPoints;
//    public bool isEventCounter { get; } = false;

//    public Tilemap tilemap;
//    public List<Tilemap> tilesWState = new List<Tilemap>();

//    public void Start()
//    {
//        Dijkstra = GameObject.FindWithTag("Managers").GetComponent<TDAGraph>();

//        tilemap = GameObject.FindWithTag("Track").GetComponent<Tilemap>();

//        tilesWState = GameObject.FindGameObjectsWithTag("TileState")
//          .Select(obj => obj.GetComponent<Tilemap>())
//          .Where(tilemap => tilemap != null)
//          .ToList();

//        //Dijkstra.InitGraph(tilemap, tilesWState);

//        checkPoints = FindObjectsOfType<CheckPoints>().ToList();
      
//    }

//    public void ApplyUpgrade(TopDownController controller)
//    {
        
//    }

//    public bool CounteractState(IState state)
//    {
//        return false;
//    }
//}