using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPJ, followWP};

    public AIMode mode;
    public bool isAvoidingCars = true;

    public float maxSpeed;

    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    Vector2 avoidanceVectorLerp = Vector2.zero;


    TopDownController controller;


    AICheckPoints currentWP = null;
    AICheckPoints[] allAIWP;

    CapsuleCollider2D capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TopDownController>();
        allAIWP = FindObjectsOfType<AICheckPoints>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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

        float angleToTarget = Vector2.SignedAngle(transform.up, vecTarget);
        angleToTarget *= -1;


        float steeringAmount = angleToTarget / 45f;

        steeringAmount = Mathf.Clamp(steeringAmount, -1f, 1f);

        return steeringAmount;

    }

    float ManageAISpeed(float inputX)
    {

        if (controller.GetVelocityMag() > maxSpeed)
        {
            return 0;
        }

        // Reduce menos la velocidad al girar
        return Mathf.Clamp(1.0f - Mathf.Abs(inputX) * 0.5f, 0.5f, 1.0f);
    }

    float ManageWPSpeed(AICheckPoints nextWP)
    {
       
        return nextWP.maxSpeed = Random.Range(20, 25);
    }

    void FollowWP()
    {
        if (currentWP == null)
        {
            currentWP = FindClosestWP();
        }

        if (currentWP != null)
        {
            // A�adir un desplazamiento aleatorio dentro de un radio alrededor del waypoint
            float radius = 3.0f; // Radio de tolerancia para no seguir un camino tan lineal
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            targetPosition = currentWP.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            float distanceToWP = (targetPosition - transform.position).magnitude;

            // A�adir un margen de error a la distancia m�nima para alcanzar el waypoint
            float adjustedMinDist = currentWP.minDistToReachWP + Random.Range(0.5f, 2.0f);

            if (distanceToWP <= adjustedMinDist)
            {
                // Manejo de la velocidad en el siguiente waypoint
                maxSpeed = ManageWPSpeed(FindClosestWP());

                // Si el siguiente waypoint tiene m�s de un camino, elige aleatoriamente
                currentWP = currentWP.nextWP[Random.Range(0, currentWP.nextWP.Length)];
            }
        }
    }
    AICheckPoints FindClosestWP()
    {
        //Devuelve el waypont mas cercano a la IA
        return allAIWP.OrderBy(index => Vector3.Distance(transform.position, index.transform.position)).FirstOrDefault();
    }
}
