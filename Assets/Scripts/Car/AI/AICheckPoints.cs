using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICheckPoints : MonoBehaviour
{
    // Lista de waypoints a los que este waypoint puede ir
    public AICheckPoints[] nextWP;

    // Distancia m�nima para considerar que el waypoint ha sido alcanzado
    public float minDistToReachWP = 5;

    // Velocidad m�xima permitida en este waypoint
    public float maxSpeed = 0;

    // Propiedad para obtener la posici�n del waypoint
    public Vector3 Position => transform.position;
}
