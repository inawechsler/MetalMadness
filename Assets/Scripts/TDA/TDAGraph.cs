using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TDAGraph : MonoBehaviour
{
    private Dictionary<Vector3Int, Dictionary<Vector3Int, int>> graph;
    private Tilemap tilemap;


    
    public void InitGraph(Tilemap tilemap)
    {
        this.tilemap = tilemap;

        graph = new Dictionary<Vector3Int, Dictionary<Vector3Int, int>>();

        foreach(Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            Debug.DrawLine(pos, pos, Color.red);
            if (!tilemap.HasTile(pos)) continue;
            
            AddVertex(pos);

            Vector3Int[] neighbours = {
                    pos + Vector3Int.up,
                    pos + Vector3Int.down,
                    pos + Vector3Int.left,
                pos + Vector3Int.right
                };

            foreach(var neighbour in neighbours)
            {
                if (tilemap.HasTile(neighbour))
                {
                    AddEdge(pos, neighbour, 1);
                }

            }

        }

    }


    private void AddVertex(Vector3Int tileToAdd)
    {
        if (!graph.ContainsKey(tileToAdd))
        {
            graph[tileToAdd] = new Dictionary<Vector3Int, int>();
        }
    }

    private void AddEdge(Vector3Int tile1,  Vector3Int tile2, int distancia)
    {
        if (!graph.ContainsKey(tile1) || !graph.ContainsKey(tile2)) return;

        graph[tile1][tile2] = distancia;
        graph[tile2][tile1] = distancia;
    }

    public List<Vector3Int> GetNeighbours(Vector3Int tile)
    {
        if (graph.ContainsKey(tile))
        {
            return new List<Vector3Int>(graph[tile].Keys);
        }
        return new List<Vector3Int>();
    }

    public List<Vector3Int> GetNodes()
    {
        return graph.Keys.ToList();
    }

    public int EdgeWeight(Vector3Int tile1, Vector3Int tile2)
    {
        if(graph.ContainsKey(tile1) && graph.ContainsKey(tile2))
        {
            return graph[tile1][tile2];
        }
        return int.MaxValue;
    }

    public List<Vector3Int> Dijkstra(Vector3Int start, Vector3Int target)
    {
        var weights = new Dictionary<Vector3Int, int>();
        var previous = new Dictionary<Vector3Int, Vector3Int?>();  // Almacena el nodo anterior
        var visited = new HashSet<Vector3Int>();
        var priorityQueue = new PriorityQueueTDA<Vector3Int>();

        // Inicializa todos los nodos con una distancia infinita
        foreach (var tile in graph.Keys)
        {
            weights[tile] = int.MaxValue;
            previous[tile] = null;  // Inicia a null
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
                int newDist = weights[currentTile] + weight;  // Suma la distancia actual al peso del vecino

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
        var path = new List<Vector3Int>();

        // Asegurarse de que el nodo de destino tiene un valor válido
        Vector3Int? current = target;
        while (current.HasValue)
        {
            path.Add(current.Value);
            current = previous[current.Value];
        }

        path.Reverse();  // Revertir el camino para que vaya de inicio a destino
        return path;
    }


}
