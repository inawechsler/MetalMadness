using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBWaypoints : MonoBehaviour
{
    public NodoWDP raiz;

    // Método para agregar un waypoint al árbol
    public void AgregarElem(AICheckPoints wp)
    {
        List<AICheckPoints> waypoints = new List<AICheckPoints> { wp };  // Lista de un solo waypoint
        float dist = Vector3.Distance(wp.Position, transform.position);  // Distancia al coche
        raiz = AgregarElemRecursivo(raiz, waypoints, dist);
    }

    // Método recursivo para agregar un nodo al árbol
    private NodoWDP AgregarElemRecursivo(NodoWDP nodo, List<AICheckPoints> waypoints, float dist)
    {
        if (nodo == null)
        {
            Debug.Log("Nodo creado con waypoints.");
            return new NodoWDP(waypoints, dist);
        }

        // Comparar las distancias para decidir si agregar a la izquierda o a la derecha
        if (dist < nodo.distancia)
        {
            nodo.hijoIzq = AgregarElemRecursivo(nodo.hijoIzq, waypoints, dist);
        }
        else
        {
            nodo.hijoDer = AgregarElemRecursivo(nodo.hijoDer, waypoints, dist);
        }

        return nodo;
    }

    // Buscar el waypoint más cercano en el árbol
    public AICheckPoints FindClosestWP(Vector3 position)
    {
        // Verifica si el árbol tiene nodos antes de intentar buscar
        if (raiz == null)
        {
            Debug.LogWarning("El árbol de waypoints está vacío");
            return null;
        }
        Debug.Log("Buscando waypoint más cercano desde la posición: " + position);
        return FindClosestWPRecursivo(raiz, position);
    }

    // Método recursivo para encontrar el waypoint más cercano
    private AICheckPoints FindClosestWPRecursivo(NodoWDP nodo, Vector3 position)
    {
        if (nodo == null)
            return null;

        // Calculamos la distancia al nodo actual
        float distActual = Vector3.Distance(position, nodo.info[0].transform.position);  // Usamos el primer waypoint en la lista para calcular la distancia
        AICheckPoints closest = nodo.info[0];
        float closestDist = distActual;

        // Decidir en qué subárbol buscar según la distancia
        if (position.x < nodo.info[0].transform.position.x)  // Si la posición está a la izquierda del nodo, ir al subárbol izquierdo
        {
            if (nodo.hijoIzq != null)
            {
                AICheckPoints leftClosest = FindClosestWPRecursivo(nodo.hijoIzq, position);
                float leftDist = Vector3.Distance(position, leftClosest.transform.position);
                if (leftDist < closestDist)
                {
                    closest = leftClosest;
                    closestDist = leftDist;
                }
            }
        }
        else // Si está a la derecha, ir al subárbol derecho
        {
            if (nodo.hijoDer != null)
            {
                AICheckPoints rightClosest = FindClosestWPRecursivo(nodo.hijoDer, position);
                float rightDist = Vector3.Distance(position, rightClosest.transform.position);
                if (rightDist < closestDist)
                {
                    closest = rightClosest;
                }
            }
        }

        return closest;
    }

    //// Método para agregar un waypoint al árbol
    //public void AgregarElem(AICheckPoints wp)
    //{
    //    float dist = Vector3.Distance(wp.Position, transform.position);  // Distancia al coche
    //    raiz = AgregarElemRecursivo(raiz, wp, dist);
    //}

    //// Método recursivo para agregar un nodo al árbol
    //private NodoWDP AgregarElemRecursivo(NodoWDP nodo, AICheckPoints wp, float dist)
    //{
    //    if (nodo == null)
    //    {
    //        Debug.Log("Nodo creado con waypoint: " + wp.name);
    //        return new NodoWDP(wp, dist);
    //    }

    //    // Comparar las distancias para decidir si agregar a la izquierda o a la derecha
    //    if (dist < nodo.distancia)
    //    {
    //        nodo.hijoIzq = AgregarElemRecursivo(nodo.hijoIzq, wp, dist);
    //    }
    //    else
    //    {
    //        nodo.hijoDer = AgregarElemRecursivo(nodo.hijoDer, wp, dist);
    //    }

    //    return nodo;
    //}

    //// Buscar el waypoint más cercano en el árbol
    //public AICheckPoints FindClosestWP(Vector3 position)
    //{
    //    // Verifica si el árbol tiene nodos antes de intentar buscar
    //    if (raiz == null)
    //    {
    //        Debug.LogWarning("El árbol de waypoints está vacío");
    //        return null;
    //    }
    //    Debug.Log("Buscando waypoint más cercano desde la posición: " + position);
    //    return FindClosestWPRecursivo(raiz, position);
    //}

    //// Método recursivo para encontrar el waypoint más cercano
    //private AICheckPoints FindClosestWPRecursivo(NodoWDP nodo, Vector3 position)
    //{
    //    if (nodo == null)
    //        return null;

    //    // Calculamos la distancia al nodo actual
    //    float distActual = Vector3.Distance(position, nodo.info.transform.position);
    //    AICheckPoints closest = nodo.info;
    //    float closestDist = distActual;

    //    // Decidir en qué subárbol buscar según la distancia
    //    if (position.x < nodo.info.transform.position.x)  // Si la posición está a la izquierda del nodo, ir al subárbol izquierdo
    //    {
    //        if (nodo.hijoIzq != null)
    //        {
    //            AICheckPoints leftClosest = FindClosestWPRecursivo(nodo.hijoIzq, position);
    //            float leftDist = Vector3.Distance(position, leftClosest.transform.position);
    //            if (leftDist < closestDist)
    //            {
    //                closest = leftClosest;
    //                closestDist = leftDist;
    //            }
    //        }
    //    }
    //    else // Si está a la derecha, ir al subárbol derecho
    //    {
    //        if (nodo.hijoDer != null)
    //        {
    //            AICheckPoints rightClosest = FindClosestWPRecursivo(nodo.hijoDer, position);
    //            float rightDist = Vector3.Distance(position, rightClosest.transform.position);
    //            if (rightDist < closestDist)
    //            {
    //                closest = rightClosest;
    //            }
    //        }
    //    }

    //    return closest;
    //}
}
