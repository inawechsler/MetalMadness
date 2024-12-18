using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class TDAGraph : MonoBehaviour
{
    private Dictionary<Vector3Int, Dictionary<Vector3Int, int>> graph;
    private List<Vector3Int> nodesOnCollider;
    private Tilemap tilemap;
    private Car car;
    private Vector3Int currentStart;
    private Vector3Int currentEnd;
    private List<Vector3Int> cachedPath;
    private bool needsRecalculation = true;

    private void Start()
    {
        car = GameObject.FindWithTag("Player").GetComponent<Car>();
    }

    public void InitGraph(Tilemap tilemap, List<Tilemap> stateTilemaps)
    {
        // Inicializamos las estructuras
        graph = new Dictionary<Vector3Int, Dictionary<Vector3Int, int>>();
        nodesOnCollider = new List<Vector3Int>();
        this.tilemap = tilemap;

        // Definimos los offsets de los vecinos (arriba, abajo, etc)
        var neighbourOffsets = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.up + Vector3Int.right,
            Vector3Int.up + Vector3Int.left,
            Vector3Int.down + Vector3Int.right,
            Vector3Int.down + Vector3Int.left
        };

        // Recorremos el tilemap
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            AddVertex(pos);

            // Chequeamos cada vecino
            foreach (var offset in neighbourOffsets)
            {
                Vector3Int neighbour = pos + offset;
                if (!tilemap.HasTile(neighbour)) continue;

                int weight = 1;
                foreach (var state in stateTilemaps)
                {
                    weight = CheckNodeOnCollision(neighbour, state);
                }
                AddEdge(pos, neighbour, weight);
            }
        }

        // Calculamos el primer camino
        var startPoint = spawnPoint(SceneManager.GetActiveScene().name, "Start");
        var endPoint = spawnPoint(SceneManager.GetActiveScene().name, "End");
        var path = Dijkstra(startPoint, endPoint);
        DrawPath(path, startPoint, endPoint);
    }

    private Vector3Int spawnPoint(string sceneName, string pointToReturn)
    {
        if (pointToReturn.ToLower() == "start")
        {
            return sceneName switch
            {
                "Race" => new Vector3Int(-19, 27, 0),
                "Race3" => new Vector3Int(-109, 130, 0),
                _ => Vector3Int.zero
            };
        }

        return sceneName switch
        {
            "Race" => new Vector3Int(-12, 29, 0),
            "Race3" => new Vector3Int(-129, 116, 0),
            _ => Vector3Int.zero
        };
    }

    private int CheckNodeOnCollision(Vector3Int nodePosition, Tilemap stateTiles)
    {
        if (!stateTiles.HasTile(nodePosition)) return 1;

        bool isColliding = FindNodeOnCollider(nodePosition, stateTiles);

        // Ya no modificamos nodesOnCollider aquí
        Debug.DrawLine(tilemap.CellToWorld(nodePosition),
                      tilemap.CellToWorld(nodePosition) + Vector3.up * 0.5f,
                      isColliding ? Color.black : Color.white,
                      Mathf.Infinity);

        return isColliding ? 100 : 1;
    }

    private bool FindNodeOnCollider(Vector3Int position, Tilemap stateTiles)
    {
        Vector3 worldPosition = stateTiles.GetCellCenterWorld(position);
        var hit = Physics2D.Raycast(worldPosition, Vector2.zero, 1f, 1 << 6);

        if (hit.collider == null) return false;

        var stateCollider = hit.collider.GetComponent<StateCollider>();
        if (stateCollider?.state == null) return false;

        return !car.upgrades.HasUpgradeToCounteract(stateCollider.state);
    }

    public void UpdateGraphWeights(Tilemap stateTiles)
    {
        // Creamos una nueva lista para los nodos en colisión
        var newNodesOnCollider = new List<Vector3Int>();

        // Primero actualizamos los pesos y construimos la nueva lista de nodos en colisión
        foreach (var node in graph.Keys)
        {
            if (stateTiles.HasTile(node) && FindNodeOnCollider(node, stateTiles))
            {
                newNodesOnCollider.Add(node);

                // Actualizamos los pesos para este nodo y sus vecinos
                foreach (var neighbour in graph[node].Keys.ToList())
                {
                    int newWeight = CheckNodeOnCollision(neighbour, stateTiles);
                    graph[node][neighbour] = newWeight;
                    graph[neighbour][node] = newWeight; // Aseguramos simetría
                }
            }
        }

        // Actualizamos la lista de nodos en colisión
        nodesOnCollider = newNodesOnCollider;

        needsRecalculation = true;

        var startPoint = spawnPoint(SceneManager.GetActiveScene().name, "Start");
        var endPoint = spawnPoint(SceneManager.GetActiveScene().name, "End");
        var path = Dijkstra(startPoint, endPoint);
        DrawPath(path, startPoint, endPoint);
    }

    // ... (resto de métodos sin cambios)

    private void AddVertex(Vector3Int tileToAdd)
    {
        if (!graph.ContainsKey(tileToAdd))
        {
            graph[tileToAdd] = new Dictionary<Vector3Int, int>();
        }
    }

    private void AddEdge(Vector3Int tile1, Vector3Int tile2, int distance)
    {
        if (!graph.ContainsKey(tile1) || !graph.ContainsKey(tile2)) return;

        graph[tile1][tile2] = distance;
        graph[tile2][tile1] = distance;
    }

    public List<Vector3Int> GetNeighbours(Vector3Int tile)
    {
        return graph.ContainsKey(tile) ? new List<Vector3Int>(graph[tile].Keys) : new List<Vector3Int>();
    }

    public int EdgeWeight(Vector3Int tile1, Vector3Int tile2)
    {
        if (graph.ContainsKey(tile1) && graph[tile1].ContainsKey(tile2))
            return graph[tile1][tile2];
        return int.MaxValue;
    }

    public List<Vector3Int> Dijkstra(Vector3Int start, Vector3Int target)
    {
        // Usamos el cache si es posible
        if (!needsRecalculation && start == currentStart && target == currentEnd && cachedPath != null)
        {
            return cachedPath;
        }

        // Validamos que los puntos existan
        if (!graph.ContainsKey(start) || !graph.ContainsKey(target))
        {
            Debug.LogWarning("Punto inicial o final no encontrado en el grafo");
            return new List<Vector3Int>();
        }

        var weights = new Dictionary<Vector3Int, int>();
        var previous = new Dictionary<Vector3Int, Vector3Int>();
        var priorityQueue = new PriorityQueueTDA<Vector3Int>();

        // Inicializamos el punto de inicio
        weights[start] = 0;
        priorityQueue.Enqueue(start, 0);

        while (priorityQueue.Count() > 0)
        {
            var currentTile = priorityQueue.Dequeue();

            if (currentTile == target)
                break;

            int currentWeight = weights[currentTile];

            foreach (var kvp in graph[currentTile])
            {
                var neighbour = kvp.Key;
                var edgeWeight = kvp.Value;

                int newWeight = currentWeight + edgeWeight;

                if (!weights.ContainsKey(neighbour) || newWeight < weights[neighbour])
                {
                    weights[neighbour] = newWeight;
                    previous[neighbour] = currentTile;
                    priorityQueue.Enqueue(neighbour, newWeight);
                }
            }
        }

        // Reconstruimos el camino
        var path = new List<Vector3Int>();

        if (!previous.ContainsKey(target))
            return path;

        var current = target;
        while (current != start)
        {
            path.Add(current);
            current = previous[current];
        }
        path.Add(start);
        path.Reverse();

        // Actualizamos el cache
        currentStart = start;
        currentEnd = target;
        cachedPath = path;
        needsRecalculation = false;

        return path;
    }

    private void DrawPath(List<Vector3Int> path, Vector3Int start, Vector3Int target)
    {
        var lineRenderer = GetComponent<LineRenderer>();

        if (path.Count > 0)
        {
            lineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                lineRenderer.SetPosition(i, tilemap.CellToWorld(path[i]));
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
            Debug.LogWarning("No se encontró un camino entre los puntos");
        }
    }
}