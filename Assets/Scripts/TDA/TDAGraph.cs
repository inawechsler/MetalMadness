using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TDAGraph : MonoBehaviour
{
    private Dictionary<TileCollider, Dictionary<TileCollider, int>> graph;
    private Tilemap tilemap;


    public Dictionary<Vector3Int, TileCollider> tileColliders { get; private set; }


    [SerializeField] TileCollider tileColliderPrefab;


    public void InitGraph(Tilemap tilemap)
    {
        graph = new();
        tileColliders = new();

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            // Instancia un nuevo TileCollider en la posición correspondiente
            TileCollider tileColliderInstance = Instantiate(tileColliderPrefab, tilemap.CellToWorld(pos), Quaternion.identity);

            tileColliders.Add(pos, tileColliderInstance);

            Vector3Int[] neighbours = {
                pos + Vector3Int.up,
                pos + Vector3Int.down,
                pos + Vector3Int.left,
                pos + Vector3Int.right
            };

            foreach (var neighbour in neighbours)
            {
                if (tilemap.HasTile(neighbour))
                {
                    TileCollider neighbourCollider = Instantiate(tileColliderPrefab, tilemap.CellToWorld(neighbour), Quaternion.identity);
                    Debug.Log("JA2");
                    tileColliders.Add(neighbour, tileColliderInstance);
                    if (!graph.ContainsKey(tileColliders[neighbour]))
                    {
                        Debug.Log("skd");
                        AddEdge(tileColliderInstance, neighbourCollider, 1);
                    }
                }
            }

        }

    }

    public int GetTileWeight(Vector3Int neighbours)
    {
        return 0;
    }


    private void AddVertex(TileCollider tileToAdd)
    {
        if (!graph.ContainsKey(tileToAdd))
        {
            graph[tileToAdd] = new Dictionary<TileCollider, int>();
        }
    }

    private void AddEdge(TileCollider tile1, TileCollider tile2, int distance)
    {
        if (!graph.ContainsKey(tile1) || !graph.ContainsKey(tile2)) return;

        graph[tile1][tile2] = distance;
        graph[tile2][tile1] = distance;
    }

    public List<TileCollider> GetNeighbours(TileCollider tile)
    {
        if (graph.ContainsKey(tile))
        {
            return new List<TileCollider>(graph[tile].Keys);
        }
        return new List<TileCollider>();
    }

    public List<TileCollider> GetNodes()
    {
        return graph.Keys.ToList();
    }

    public int EdgeWeight(TileCollider tile1, TileCollider tile2)
    {
        if (graph.ContainsKey(tile1) && graph[tile1].ContainsKey(tile2))
        {
            return graph[tile1][tile2];
        }
        return int.MaxValue;
    }

    public List<TileCollider> Dijkstra(TileCollider start, TileCollider target)
    {
        var weights = new Dictionary<TileCollider, int>();
        var previous = new Dictionary<TileCollider, TileCollider?>(); // Almacena el nodo anterior
        var visited = new HashSet<TileCollider>();
        var priorityQueue = new PriorityQueueTDA<TileCollider>();

        // Inicializa todos los nodos con una distancia infinita
        foreach (var tile in graph.Keys)
        {
            weights[tile] = int.MaxValue;
            previous[tile] = null; // Inicia a null
        }

        // Establece el peso inicial para el nodo de inicio
        weights[start] = 0;
        priorityQueue.Enqueue(start, weights[start]);

        while (priorityQueue.Count() > 0)
        {
            var currentTile = priorityQueue.Dequeue();

            // Si el nodo ya ha sido visitado, continúa con el siguiente
            if (visited.Contains(currentTile)) continue;

            visited.Add(currentTile);

            // Si se ha alcanzado el nodo objetivo, termina
            if (currentTile == target) break;

            // Procesa los vecinos del nodo actual
            foreach (var neighbour in GetNeighbours(currentTile))
            {
                if (visited.Contains(neighbour)) continue;

                int weight = EdgeWeight(currentTile, neighbour);
                int newDist = weights[currentTile] + weight; // Suma la distancia actual al peso del vecino

                // Si encontramos una ruta más corta, actualizamos la distancia y el nodo anterior
                if (newDist < weights[neighbour])
                {
                    weights[neighbour] = newDist;
                    previous[neighbour] = currentTile;
                    priorityQueue.Enqueue(neighbour, newDist);
                }
            }
        }

        // Generar el camino desde el nodo objetivo hacia el nodo de inicio
        var path = new List<TileCollider>();

        TileCollider? current = target;
        while (current != null)
        {
            path.Add(current);
            current = previous[current];
        }

        path.Reverse(); // Revertir el camino para que vaya de inicio a destino
        return path;
    }

    private TileCollider GetTileCollider(Vector3Int position)
    {
        return tileColliders.ContainsKey(position) ? tileColliders[position] : null;
    }

}
