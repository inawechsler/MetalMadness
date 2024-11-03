using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Rendering;

public class TopDownController : MonoBehaviour
{
    //Start is called before the first frame update
    public float accelerationFactor = 35.0f;
    public float turnFactor = 3.5f;

    public float driftFactor { get; private set; } = .85f;
    public float currentDriftFactor { get; private set; }

    float accelerationInput = 0;
    float steeringInput = 0;


    float velocity;

    [SerializeField] public float currentMaxSpeedCap { get; private set; } = 20;

    public float lastSpeedBefChange { get; private set; }

    float rotationAngle = 0;

    [SerializeField] float maxSpeed = 20f;

    float velocitVsUp;

    public Rigidbody2D rb2D;

    public bool isOnState;

    Car car;

    public CarUpgrades carUpgrades {  get; private set; }

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        car = GetComponent<Car>();
        carUpgrades = GetComponent<CarUpgrades>();
    }

    private void Start()
    {
        rotationAngle = transform.rotation.eulerAngles.z;
    }

    //Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();


    }

    void ApplyEngineForce()
    {

        ConstraintsEngineForce();

        velocity = rb2D.velocity.magnitude;

        rb2D.drag = accelerationInput == 0 ? Mathf.Lerp(rb2D.drag, 3f, Time.fixedDeltaTime * 3) : 0;


        Vector2 engineForceVec = transform.up * accelerationInput * accelerationFactor;



        rb2D.AddForce(engineForceVec, ForceMode2D.Force);
    }

    public float GetVelocityMag()
    {
        return velocity;
    }

    void ManageCarSpeed()
    {
        Debug.Log(currentMaxSpeedCap);
        if (velocity > maxSpeed)
        {
            rb2D.velocity -= rb2D.velocity.normalized * 0.5f;
        }

    }

    void ConstraintsEngineForce()
    {


        ManageCarSpeed();

        //Calculo que tan para adelante estoy yendo en la direc de mi velocidad
        velocitVsUp = Vector2.Dot(transform.up, rb2D.velocity);

        maxSpeed = car.onTrack ? currentMaxSpeedCap : 10;

        //Limito para no ir más rapido que la max en la direccion "forward"
        if (car.onTrack)
        {
            if (velocity > maxSpeed)
            {
                rb2D.velocity = rb2D.velocity.normalized * maxSpeed;
            }

        }


        //Limito para ir más lento en reversa
        if (velocitVsUp < -maxSpeed * .5f && accelerationInput < 0)
            return;

    }

    public float SetAccelerationInput(float accel)
    {
        return accelerationInput = accel;
    }

    public float GetAccelerationInput()
    {
        return accelerationInput;
    }

    void ApplySteering()
    {
        //Con el 8 labura bien, dobla solo si a cierta velocidad
        float minSpeedBefTurningFactor = (rb2D.velocity.magnitude / 8);
        minSpeedBefTurningFactor = Mathf.Clamp01(minSpeedBefTurningFactor);


        rotationAngle -= steeringInput * turnFactor * minSpeedBefTurningFactor;

        rb2D.MoveRotation(rotationAngle);
    }

    public float SetMaxSpeedCap(float maxSpeedCap)
    {

        currentMaxSpeedCap = maxSpeed;
        Debug.Log(maxSpeedCap);
        currentMaxSpeedCap = maxSpeedCap;

        return currentMaxSpeedCap;
    }


    public void SetLastSpeedBefChange(float maxSpeedCap)
    {
        if (lastSpeedBefChange > maxSpeedCap)
        {
            return;
        }
        lastSpeedBefChange = maxSpeedCap;
    }
    public float SetDriftFactor(float newDrift)
    {
        if(driftFactor <= .85f)
        {
            currentDriftFactor = driftFactor;
        }

       return driftFactor = newDrift;
    }
    public float GetSpeed()
    {
        return velocity;
    }

    void KillOrthogonalVelocity()
    {

        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb2D.velocity, transform.up);

        Vector2 rightVelocity = transform.right * Vector2.Dot(rb2D.velocity, transform.right);

        rb2D.velocity = forwardVelocity + rightVelocity * driftFactor;


    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rb2D.velocity);
    }
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public bool isTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();

        isBraking = false;

        if (accelerationInput < 0 && velocitVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if(Mathf.Abs(GetLateralVelocity()) > 4f)
        {
            return true;
        }

        return false;
    }

}
