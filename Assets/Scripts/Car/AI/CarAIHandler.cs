using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPJ, followWP };

    public AIMode mode;
    public bool isAvoidingCars = true;

    public float maxSpeed;

    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    Vector2 avoidanceVectorLerp = Vector2.zero;




    TopDownController controller;


   // AICheckPoints currentWP;
    AICheckPoints[] allAIWP;

    CapsuleCollider2D capsuleCollider;

    public  ABBWaypoints waypointTree;
    private NodoWDP currentNode;
    private AICheckPoints currentWP;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TopDownController>();
        allAIWP = FindObjectsOfType<AICheckPoints>();
       
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        currentNode = waypointTree.raiz;

        waypointTree = FindObjectOfType<ABBWaypoints>();

     

        foreach (var wp in allAIWP)
        {
            waypointTree.AgregarElem(wp);
        }

        currentNode = waypointTree.raiz;
        if (currentNode != null && currentNode.info.Count > 0)
        {
            currentWP = currentNode.info[0];  // Comienza con el primer waypoint del nodo
        }

        // Empieza el enumerador para recorrer el ABB
        StartCoroutine(WaypointEnumerator());

        Debug.Log("Waypoints agregados: " + allAIWP.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        Debug.Log("CurrentWP :" + currentWP);
     
        Vector2 inputVector = Vector2.zero;

        switch (mode)
        {
            case AIMode.followPJ:
                FollowPlayer();
                break;
            case AIMode.followWP:
                FollowWP();
                break;
            default:
                break;
        }

        inputVector.x = TurnTowardsTarget();
        inputVector.y = ManageAISpeed(inputVector.x);

        controller.SetInputVector(inputVector);
    }

    void FollowPlayer()
    {
        if (targetTransform == null)
        {
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            targetPosition = targetTransform.position;
        }
    }

    float TurnTowardsTarget()
    {
        Vector2 vecTarget = targetPosition - transform.position;
        vecTarget.Normalize();

        float angleToTarget = Vector2.SignedAngle(transform.up, vecTarget); //Angulo entre la orientación del auto en relación al target (WP)
        angleToTarget *= -1;


        float steeringAmount = angleToTarget / 45f; //Calculo giro necesario entre 45(45 es giro completo a la derecha o izquierda

        steeringAmount = Mathf.Clamp(steeringAmount, -1f, 1f);//Rango de -1 a 1, 1 Derecha, -1 izquierda

        return steeringAmount;

    }

    float ManageAISpeed(float inputX)
    {

        if (controller.GetVelocityMag() > maxSpeed) //Si mayor deja de acelerar
        {
            return 0;
        }

        return Mathf.Clamp(1.0f - Mathf.Abs(inputX) * 0.5f, 0.5f, 1.0f); //Multiplico el positivo de X (giro), al restar el resultado es valor entre 0.5(giro Max) y 1(giro Max), clampeo para que cuando la IA gire brusco baje velocidad pero nunca menos que la mitad de su aceleración
    }

    float ManageWPSpeed(AICheckPoints nextWP)
    {
        return nextWP.maxSpeed > 0 ? nextWP.maxSpeed : Random.Range(20, 25);
    }


    void FollowWP()
    {
        if (currentWP == null)
        {
            // Si no hay waypoint asignado, buscar el más cercano.
            currentWP = waypointTree.FindClosestWP(transform.position);
            if (currentWP == null)
            {
                Debug.LogError("No se encontró un waypoint cercano.");
                return;
            }
        }

        // Calcular la distancia al waypoint actual
        float distanceToWP = (transform.position - currentWP.transform.position).magnitude;
        float adjustedMinDist = currentWP.minDistToReachWP + Random.Range(0.5f, 2.0f);

        if (distanceToWP <= adjustedMinDist)
        {
            // Si la IA está cerca del waypoint actual, busca el siguiente waypoint.
            MoveToNextWaypoint();
        }

        // Añadir desplazamiento aleatorio alrededor del waypoint para mayor naturalidad
        float radius = 3.0f;
        Vector2 randomOffset = Random.insideUnitCircle * radius;
        targetPosition = currentWP.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);


        //if (currentWP == null)
        //{
        //    currentWP = FindClosestWP();
        //}

        //if (currentWP != null)
        //{
        //    // Añade desplazamiento aleatorio dentro de un radio alrededor del waypoint
        //    float radius = 3.0f; // Radio de tolerancia para no seguir un camino tan lineal
        //    Vector2 randomOffset = Random.insideUnitCircle * radius;
        //    targetPosition = currentWP.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        //    float distanceToWP = (targetPosition - transform.position).magnitude;

        //    // Añade un margen de error a la distancia mínima para alcanzar el waypoint
        //    float adjustedMinDist = currentWP.minDistToReachWP + Random.Range(0.5f, 2.0f);

        //    if (distanceToWP <= adjustedMinDist)
        //    {
        //        // Manejo de la velocidad en el siguiente waypoint
        //        maxSpeed = ManageWPSpeed(FindClosestWP());

        //        // Si el siguiente waypoint tiene más de un camino, elige aleatoriamente
        //        currentWP = currentWP.nextWP[Random.Range(0, currentWP.nextWP.Length)];
        //    }
        //}

        //AICheckPoints FindClosestWP()
        //{
        //    //Devuelve el waypont mas cercano a la IA
        //    return allAIWP.OrderBy(index => Vector3.Distance(transform.position, index.transform.position)).FirstOrDefault();
        //}
    }

    void MoveToNextWaypoint()
    {
        if (currentWP != null)
        {
            if (currentWP.nextWP.Length > 0)
            {
                currentWP = currentWP.nextWP[Random.Range(0, currentWP.nextWP.Length)];
            }
            else
            {
                // Si no hay waypoints siguientes en 'nextWP', busca el siguiente en el árbol
                currentWP = waypointTree.FindClosestWP(transform.position);
                if (currentWP == null)
                {
                    Debug.LogError("No se pudo encontrar un nuevo waypoint.");
                }
            }
        }
        else
        {
            Debug.LogError("El waypoint actual es nulo, no se puede mover al siguiente.");
        }
    }
    private IEnumerator WaypointEnumerator()
    {
        while (currentNode != null && currentNode.info.Count > 0)
        {
            // Comienza con el primer waypoint del nodo
            currentWP = currentNode.info[0];
            while (Vector3.Distance(transform.position, currentWP.transform.position) > currentWP.minDistToReachWP)
            {
                // Mueve la IA hacia el waypoint
                targetPosition = currentWP.transform.position;
                float turnInput = TurnTowardsTarget();
                float speedInput = ManageAISpeed(turnInput);
                controller.SetInputVector(new Vector2(turnInput, speedInput));

                yield return null; // Espera al siguiente frame
            }

            // Si se alcanzó el waypoint, avanzamos al siguiente
            TraverseToNextWaypoint();
            yield return null;
        }
    }

    // Método para recorrer el árbol en orden
    private void TraverseToNextWaypoint()
    {
        if (currentNode == null)
        {
            Debug.LogError("currentNode es null, no se puede avanzar.");
            return; // Si el nodo es null, no hacemos nada
        }

        int currentIndex = currentNode.info.IndexOf(currentWP);
        if (currentIndex < currentNode.info.Count - 1)
        {
            // Avanzamos al siguiente waypoint en la lista
            currentWP = currentNode.info[currentIndex + 1];
        }
        else
        {
            // Si no hay más waypoints en este nodo, encontrar el siguiente nodo
            currentNode = FindNextClosestNode(currentNode);
            if (currentNode != null && currentNode.info.Count > 0)
            {
                currentWP = currentNode.info[0];  // Comienza con el primer waypoint del siguiente nodo
            }
            else
            {
                Debug.LogError("No se encontraron más waypoints o nodos.");
                return; // Si no hay más waypoints ni nodos, termina la ejecución
            }
        }
    }

    // Método para encontrar el siguiente nodo más cercano en el árbol
    private NodoWDP FindNextClosestNode(NodoWDP node)
    {
        if (node == null)
        {
            Debug.LogError("El nodo es null.");
            return null;
        }
        // Buscar el hijo más cercano (izquierda o derecha) dependiendo de la distancia
        if (node.hijoIzq != null && node.hijoDer != null)
        {
            float leftDist = Vector3.Distance(transform.position, node.hijoIzq.info[1].transform.position);
            float rightDist = Vector3.Distance(transform.position, node.hijoDer.info[1].transform.position);

            if (leftDist < rightDist)
            {
                return node.hijoIzq;
            }
            else
            {
                return node.hijoDer;
            }
        }
        else if (node.hijoIzq != null)
        {
            return node.hijoIzq;
        }
        else if (node.hijoDer != null)
        {
            return node.hijoDer;
        }

        return null;  // Si no hay hijos, terminamos
    }
}