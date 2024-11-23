using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IdealPath : MonoBehaviour, IUpgrade
{
    public TDAGraph Dijkstra;
    
    public List<CheckPoints> checkPoints;
    public bool isEventCounter { get; } = false;

    public Tilemap tilemap;

    public void Start()
    {
        Dijkstra = GameObject.FindWithTag("Managers").GetComponent<TDAGraph>();
        tilemap = GameObject.FindWithTag("Track").GetComponent<Tilemap>();
        Dijkstra.InitGraph(tilemap);
        checkPoints = FindObjectsOfType<CheckPoints>().ToList();
        // Obtener dos posiciones aleatorias válidas
        List<Vector3Int> posicionesValidas = ObtenerPosicionesValidas(tilemap);
        if (posicionesValidas.Count < 2)
        {
            Debug.LogError("No hay suficientes tiles válidos en el Tilemap para calcular un camino.");
            return;
        }

        // Seleccionar puntos aleatorios
        Vector3Int inicio = posicionesValidas[Random.Range(0, posicionesValidas.Count)];
        Vector3Int destino = FindClosestNode(
    Vector3Int.RoundToInt(GetFinishCheckPoint().transform.position),
    Dijkstra.GetNodes()); // Assuming this method returns the list of nodes

        Debug.Log($"Inicio: {inicio}, Destino: {destino}");

        // Calcular el camino usando Dijkstra
        List<Vector3Int> ruta = Dijkstra.Dijkstra(inicio, destino);

        // Dibujar el camino con LineRenderer
        if (ruta.Count > 0)
        {
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = ruta.Count;

            for (int i = 0; i < ruta.Count; i++)
            {
                lineRenderer.SetPosition(i, tilemap.CellToWorld(ruta[i]));
            }
        }
        else
        {
            Debug.LogError("No se encontró un camino entre los puntos seleccionados.");
        }
    }

    // Método para obtener todas las posiciones válidas del Tilemap
    private List<Vector3Int> ObtenerPosicionesValidas(Tilemap tilemap)
    {
        List<Vector3Int> posicionesValidas = new List<Vector3Int>();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                posicionesValidas.Add(pos);
            }
        }

        return posicionesValidas;
    }

    public void ApplyUpgrade(TopDownController controller)
    {
        Vector3Int inicio = new Vector3Int(0, 0, 0);
        Vector3Int destino = FindClosestNode(
    Vector3Int.RoundToInt(GetFinishCheckPoint().transform.position),
    Dijkstra.GetNodes() // Assuming this method returns the list of nodes
);

        List<Vector3Int> ruta = Dijkstra.Dijkstra(inicio, destino);

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = ruta.Count;

        for (int i = 0; i < ruta.Count; i++)
        {
            lineRenderer.SetPosition(i, tilemap.CellToWorld(ruta[i]));
        }
    }

    public bool CounteractState(IState state)
    {
        return false;
    }

    public CheckPoints GetFinishCheckPoint()
    {
        var checkPointNearPos = checkPoints.First(s => s.isFinishLine);

        Debug.Log(checkPointNearPos.gameObject.name);
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