using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float maxSteeringAngle = 30f;

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
        // Calculate current speed
        float forwardSpeed = Vector3.Dot(transform.forward, rb.linearVelocity);
            
        foreach (var wheel in wheels)
        {
            // Accelerate car if applying throttle
            if (throttleInput > 0)
            {
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = throttleInput * motorTorque;    
                }
            }
            // Allow braking since throttle isnt being applied
            else
            {
                wheel.WheelCollider.motorTorque = 0;
                wheel.WheelCollider.brakeTorque = Mathf.Abs(throttleInput) * brakeTorque;
            }

            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = steeringInput * maxSteeringAngle;
            }
        }
    }
}
