using System;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float maxSteeringAngle = 30f;
    public float steeringSmoothSpeed = 5f;
    public float currentSteering = 0f;

    private Rigidbody rb;
    private WheelControl[] wheels;
    
    private CarInputActions carControls;

    void Awake()
    {
        carControls = new CarInputActions();
    }
    void OnEnable()
    {
        carControls.Enable();
    }
    void OnDisable()
    {
        carControls.Disable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        // Read Vector2 inputs from car input system
        Vector2 inputVector = carControls.Car.Movement.ReadValue<Vector2>();
        float throttleInput = inputVector.y;
        float steeringInput = inputVector.x;
        currentSteering = Mathf.Lerp(currentSteering, steeringInput, steeringSmoothSpeed * Time.deltaTime);
        // Calculate current speed
        float forwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity);

        bool isAccelerating = Mathf.Sign(throttleInput) == Mathf.Sign(forwardSpeed);
            
        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = currentSteering * maxSteeringAngle;
            }

            if (isAccelerating)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = throttleInput * motorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = Mathf.Abs(throttleInput) * brakeTorque;
            }
            
        }
    }
}
