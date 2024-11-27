using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICheckPoints : MonoBehaviour
{
    // Lista de waypoints a los que este waypoint puede ir
    public AICheckPoints[] nextWP;

    // Distancia mínima para considerar que el waypoint ha sido alcanzado
    public float minDistToReachWP = 5;

    // Velocidad máxima permitida en este waypoint
    public float maxSpeed = 0;

    // Propiedad para obtener la posición del waypoint
    public Vector3 Position => transform.position;
}
