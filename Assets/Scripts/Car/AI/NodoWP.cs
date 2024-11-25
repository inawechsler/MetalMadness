using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoWDP : MonoBehaviour
{
    public AICheckPoints info;   // El waypoint
    public NodoWDP hijoIzq;       // Subárbol izquierdo
    public NodoWDP hijoDer;       // Subárbol derecho
    public float distancia;      // Distancia del waypoint al coche, usada para ordenar en el árbol

    public NodoWDP(AICheckPoints wp, float dist)
    {
        this.info = wp;
        this.distancia = dist;
        this.hijoIzq = null;
        this.hijoDer = null;
    }
}
