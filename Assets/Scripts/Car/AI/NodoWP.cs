using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoWDP : MonoBehaviour
{
    public List<AICheckPoints> info;   // Lista de waypoints en el nodo
    public NodoWDP hijoIzq;             // Subárbol izquierdo
    public NodoWDP hijoDer;             // Subárbol derecho
    public float distancia;             // Distancia del waypoint al coche, usada para ordenar en el árbol

    // Constructor para inicializar el nodo con una lista de waypoints
    public NodoWDP(List<AICheckPoints> waypoints, float dist)
    {
        this.info = waypoints;
        this.distancia = dist;
        this.hijoIzq = null;
        this.hijoDer = null;
    }


}
