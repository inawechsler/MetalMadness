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


    AICheckPoints currentWP;
    AICheckPoints[] allAIWP;

    CapsuleCollider2D capsuleCollider;

    ABBWaypoints waypointTree;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TopDownController>();
        allAIWP = FindObjectsOfType<AICheckPoints>();
        currentWP = FindObjectOfType<AICheckPoints>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        waypointTree = FindObjectOfType<ABBWaypoints>();

        foreach (var wp in allAIWP)
        {
            waypointTree.AgregarElem(wp);
        }

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

        float angleToTarget = Vector2.SignedAngle(transform.up, vecTarget); //Angulo entre la orientaci�n del auto en relaci�n al target (WP)
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

        return Mathf.Clamp(1.0f - Mathf.Abs(inputX) * 0.5f, 0.5f, 1.0f); //Multiplico el positivo de X (giro), al restar el resultado es valor entre 0.5(giro Max) y 1(giro Max), clampeo para que cuando la IA gire brusco baje velocidad pero nunca menos que la mitad de su aceleraci�n
    }

    float ManageWPSpeed(AICheckPoints nextWP)
    {

        return nextWP.maxSpeed = Random.Range(20, 25);
    }


    void FollowWP()
    {

        if (currentWP == null)
        {
            // Si no hay waypoint asignado, buscar el m�s cercano.
            currentWP = waypointTree.FindClosestWP(transform.position);
            if (currentWP == null)
            {
                Debug.LogError("No se encontr� un waypoint cercano.");
                return;
            }
        }

        // A�adir un desplazamiento aleatorio para dar un movimiento m�s natural
        float radius = 3.0f;
        Vector2 randomOffset = Random.insideUnitCircle * radius;
        targetPosition = currentWP.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        // Calcular la distancia al waypoint
        float distanceToWP = (targetPosition - transform.position).magnitude;
        float adjustedMinDist = currentWP.minDistToReachWP + Random.Range(0.5f, 2.0f);

        // Si la IA est� cerca del waypoint, buscar el siguiente
        if (distanceToWP <= adjustedMinDist)
        {
            // Solo cambiar de waypoint cuando se alcanza el waypoint actual
            currentWP = waypointTree.FindClosestWP(transform.position);
            if (currentWP == null)
            {
                Debug.LogError("No se pudo encontrar un nuevo waypoint.");
            }
        }


        //if (currentWP == null)
        //{
        //    currentWP = FindClosestWP();
        //}

        //if (currentWP != null)
        //{
        //    // A�ade desplazamiento aleatorio dentro de un radio alrededor del waypoint
        //    float radius = 3.0f; // Radio de tolerancia para no seguir un camino tan lineal
        //    Vector2 randomOffset = Random.insideUnitCircle * radius;
        //    targetPosition = currentWP.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

        //    float distanceToWP = (targetPosition - transform.position).magnitude;

        //    // A�ade un margen de error a la distancia m�nima para alcanzar el waypoint
        //    float adjustedMinDist = currentWP.minDistToReachWP + Random.Range(0.5f, 2.0f);

        //    if (distanceToWP <= adjustedMinDist)
        //    {
        //        // Manejo de la velocidad en el siguiente waypoint
        //        maxSpeed = ManageWPSpeed(FindClosestWP());

        //        // Si el siguiente waypoint tiene m�s de un camino, elige aleatoriamente
        //        currentWP = currentWP.nextWP[Random.Range(0, currentWP.nextWP.Length)];
        //    }
        //}

        //AICheckPoints FindClosestWP()
        //{
        //    //Devuelve el waypont mas cercano a la IA
        //    return allAIWP.OrderBy(index => Vector3.Distance(transform.position, index.transform.position)).FirstOrDefault();
        //}
    }
}