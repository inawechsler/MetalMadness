using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TDAGraph : MonoBehaviour
{
    private Dictionary<Vector3Int, Dictionary<Vector3Int, int>> graph;
    private Tilemap tilemap;


    void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angleStep = 360f / segments;
        Vector3 previousPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep;
            Vector3 newPoint = center + new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * angle), radius * Mathf.Sin(Mathf.Deg2Rad * angle), 0);

            Debug.DrawLine(previousPoint, newPoint, Color.red);
            previousPoint = newPoint;
        }
    }
    public void InitGraph(Tilemap tilemap)
    {
        this.tilemap = tilemap;
        graph = new Dictionary<Vector3Int, Dictionary<Vector3Int, int>>();

        float tileSize = tilemap.cellSize.x; // Asume que los tiles son cuadrados

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            // Dibuja un círculo en la posición del tile
            Vector3 worldPosition = tilemap.CellToWorld(pos);
            DrawCircle(worldPosition, tileSize / 2, 20);  // Ajusta el número de segmentos para suavizar el círculo

            AddVertex(pos);

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
        var previous = new Dictionary<Vector3Int, Vector3Int?>();
        var visited = new HashSet<Vector3Int>();
        var priorityQueue = new PriorityQueueTDA<Vector3Int>();

        foreach(var tile in graph.Keys)
        {
            weights[tile] = int.MaxValue;
            previous[tile] = null;
        }

        weights[start] = 0;
        priorityQueue.Enqueue(start, weights[start]);

        while (priorityQueue.Count() > 0)
        {
            var currentTile = priorityQueue.Dequeue();

            if (visited.Contains(currentTile)) continue;
            visited.Add(currentTile);

            if (currentTile == target) break;


            foreach (var neighbour in GetNeighbours(currentTile))
            {
                if (visited.Contains(neighbour)) continue;
                
                int weight = EdgeWeight(currentTile, neighbour);
                int newDist = weights[neighbour] + weight;


                if (newDist < weights[neighbour])
                {
                    weights[neighbour] = newDist;
                    previous[neighbour] = currentTile;
                    priorityQueue.Enqueue(neighbour, newDist);
                }
            }
            
        }

        var path = new List<Vector3Int>();
        
        for(var at = start; at != null; at = previous[at].Value)
        {
            path.Add(at);
        }
        path.Reverse();
        return path;

    }

}
