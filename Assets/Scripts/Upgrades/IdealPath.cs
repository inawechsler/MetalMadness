using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IdealPath : MonoBehaviour, IUpgrade
{
    public TDAGraph Dijkstra;
    
    public List<CheckPoints> checkPoints;
    public bool isEventCounter { get; } = false;

    public Tilemap tilemap, tileWState;

    public void Start()
    {
        Dijkstra = GameObject.FindWithTag("Managers").GetComponent<TDAGraph>();

        tilemap = GameObject.FindWithTag("Track").GetComponent<Tilemap>();

        tileWState = GameObject.FindWithTag("TileState").GetComponent<Tilemap>();

        Dijkstra.InitGraph(tilemap, tileWState);

        checkPoints = FindObjectsOfType<CheckPoints>().ToList();

        var chec = checkPoints.First(s => s.checkPointNumber == 1);

        Vector3Int startPoint = new Vector3Int(-19, 27, 0);


        //FindClosestNode(Vector3Int.RoundToInt(chec.transform.position), //Encuentro el punto del checkpoint de inicio y abajo con el target
        //          Dijkstra.GetNodes());

        Vector3Int targetPoint = new Vector3Int(-12, 29, 0);


          //FindClosestNode(Vector3Int.RoundToInt(GetFinishCheckPoint().transform.position),
          //Dijkstra.GetNodes()); // Assuming this method returns the list of nodes

        DrawPath(startPoint, targetPoint);


        Debug.Log($"Inicio: {startPoint}, Destino: {targetPoint}");
        Debug.Log($"{chec.gameObject.name}");




      
    }

    void DrawPath(Vector3Int start, Vector3Int target)
    {
        // Calcular el camino usando Dijkstra
        List<Vector3Int> path = Dijkstra.Dijkstra(start, target);

        // Dibujar el camino con LineRenderer
        if (path.Count > 0)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = path.Count;

            for (int i = 0; i < path.Count; i++)
            {
                lineRenderer.SetPosition(i, tilemap.CellToWorld(path[i]));
            }
        }
        else
        {
            Debug.LogError("No se encontró un camino entre los puntos seleccionados.");
        }

        //foreach (Vector3Int node in Dijkstra.GetNodes())
        //{
        //    if (node == FindClosestNode(target, Dijkstra.GetNodes())
        //        ||
        //        node == FindClosestNode(start, Dijkstra.GetNodes())) continue;


        //    Debug.DrawLine(tilemap.CellToWorld(node), tilemap.CellToWorld(node) + Vector3.up * 0.5f, Color.white, int.MaxValue);
        //}
    }

    public void ApplyUpgrade(TopDownController controller)
    {
        
    }

    public bool CounteractState(IState state)
    {
        return false;
    }

    public CheckPoints GetFinishCheckPoint()
    {
        var checkPointNearPos = checkPoints.First(s => s.isFinishLine);
        return checkPointNearPos;
    }
    private Vector3Int FindClosestNode(Vector3Int position, List<Vector3Int> nodes)
    {
        Vector3Int closestNode = Vector3Int.zero;
        float smallestDistance = float.MaxValue;

        foreach (Vector3Int node in nodes)
        {
            float distance = Vector3.Distance(position, node);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }
}