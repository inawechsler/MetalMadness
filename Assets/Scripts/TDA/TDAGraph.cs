using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class TDAGraph : MonoBehaviour
{
    private Dictionary<Vector3Int, Dictionary<Vector3Int, int>> graph;
    public TileCollider tColl;
    private Tilemap tilemap;
    private Car car;
    //Vector3Int funciona como nodo ya que representa cada celda en el tilemap

    private void Start()
    {
        car = GameObject.FindWithTag("Player").GetComponent<Car>();
    }

    public void InitGraph(Tilemap tilemap, List<Tilemap> stateTilemaps)
    {
        graph = new Dictionary<Vector3Int, Dictionary<Vector3Int, int>>();
        this.tilemap = tilemap;
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            AddVertex(pos);

            Vector3Int[] neighbours = {
            pos + Vector3Int.up,
            pos + Vector3Int.down,
            pos + Vector3Int.left,
            pos + Vector3Int.right
        };

            foreach (var neighbour in neighbours)
            {
                if (!tilemap.HasTile(neighbour)) continue;

                int weight = 1;
                foreach (var state in stateTilemaps)
                {
                    weight = CheckNodeOnCollision(neighbour, state); // Calcular peso dinámico en base a si esta en un evento activo o no
                }


                AddEdge(pos, neighbour, weight);
            }
        }

        Dijkstra(new Vector3Int(-19, 27, 0), new Vector3Int(-12, 29, 0));
    }

   

    private int CheckNodeOnCollision(Vector3Int nodePosition, Tilemap stateTiles)
    {

        int weight = 1;
        if (stateTiles.HasTile(nodePosition))
        {
            Debug.Log(FindNodeOnCollider(nodePosition, stateTiles) + " " + stateTiles.gameObject.name);
            if (FindNodeOnCollider(nodePosition, stateTiles)) //Revisa si el nodo esta en un collider y si el collider está acti
            {
                    weight = 20; // Peso más alto si tiene un estado activo

            }
            Debug.DrawLine(tilemap.CellToWorld(nodePosition), tilemap.CellToWorld(nodePosition) + Vector3.up * 0.5f, (weight == 1 ? Color.white : Color.black), 10);

        }


        return weight; // Peso normal si no tiene estado activo
    }

    //private bool FindNodeOnCollider(Vector3Int position, Tilemap stateTiles)
    //{
    //    Vector3 worldPosition = stateTiles.GetCellCenterWorld(position); // Centro exacto de la celda

    //    var hit = Physics2D.Raycast(worldPosition, Vector2.zero); // Raycast en la posición exacta

    //    if (hit.collider.gameObject.GetComponent<StateCollider>() != null)
    //    {
    //        var stateCol = hit.collider.gameObject.GetComponent<StateCollider>();

    //        if (stateTiles == null) Debug.LogWarning(stateTiles.gameObject.name + "nil");


    //        if (stateCol == null) Debug.LogWarning("statecol nil");

    //        var stateToCompare = stateCol.state;

    //        if (stateToCompare == null) Debug.LogWarning("statecompare nil");

    //        // Verifica si el auto tiene un upgrade que contrarreste el estado
    //        bool hasUpgrade = car.upgrades.HasUpgradeToCounteract(stateToCompare);

    //        if (car.upgrades == null) Debug.LogWarning("car.upgrades nil");

    //        return !hasUpgrade; // Devuelve true si no tiene el upgrade

    //    }

    //    if (hit.collider == null) Debug.LogWarning("hit.collider nil");

    //    return false; // Si no hay colisión o no se cumple ninguna condición

    //}

    private bool FindNodeOnCollider(Vector3Int position, Tilemap stateTiles)
    {
        Vector3 worldPosition = stateTiles.GetCellCenterWorld(position); // Centro exacto de la celda
 

        var hit = Physics2D.Raycast(worldPosition, Vector2.zero, 1f, 1 << 6); // Raycast en la posición exacta

        if (hit.collider == null)
        {
            Debug.LogWarning($"El Raycast no detectó ningún objeto en {worldPosition}. Verifica las capas o colisionadores.");
            return false;
        }
        if (position == new Vector3Int(-36, 6, 0)) Debug.Log("js" + hit.collider.gameObject.name);


        var isTileOnCollider = hit.collider.GetComponent<StateCollider>();
        if(isTileOnCollider != null )
        {
            Debug.Log($"Objeto detectado por Raycast: {stateTiles.WorldToCell(worldPosition)} : {hit.collider.gameObject.name}");
        }

        if (isTileOnCollider == null)
        {
            Debug.LogWarning($"El objeto detectado ({hit.collider.gameObject.name}) no tiene el componente StateCollider.");
            return false;
        }

        Debug.Log($"Estado encontrado: {isTileOnCollider.state}");

        if (isTileOnCollider.state != null)
        {
            IState stateToCompare = isTileOnCollider.state;

            // Verifica si el auto tiene un upgrade que contrarreste el estado
            bool hasUpgrade = car.upgrades.HasUpgradeToCounteract(stateToCompare);

            return !hasUpgrade; // Devuelve true si no tiene el upgrade
        }

        return false; // Si no hay colisión o no se cumple ninguna condición
    }



    public void UpdateGraphWeights(Tilemap stateTiles) //Hago Lista de Keys y Valores del grafo, y le asigno el nuevo valor que le llega a las aristas que unen a estos respectivamente
    {

        Dijkstra(new Vector3Int(-19, 27, 0), new Vector3Int(-12, 29, 0));
        foreach (var node in graph.Keys.ToList())
        {
            foreach (var neighbour in graph[node].Keys.ToList())
            {

                int newWeight = CheckNodeOnCollision(neighbour, stateTiles);
                graph[node][neighbour] = newWeight;
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

        DrawPath(path, new Vector3Int(-19, 27, 0), new Vector3Int(-12, 29, 0));

        return path;
    }


    void DrawPath(List<Vector3Int> path, Vector3Int start, Vector3Int target)
    {
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
    }

}
