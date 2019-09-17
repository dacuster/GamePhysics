using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    /*
     *  Lab 1 Step 1
     *  Define particle variables.
     */
    [SerializeField]
    private Vector2 position = Vector2.zero;
    [SerializeField]
    private Vector2 velocity = Vector2.right;
    [SerializeField]
    private Vector2 acceleration = Vector2.zero;
    [SerializeField]
    private float rotation = 0.0f;
    [SerializeField]
    private float angularVelocity = 0.0f;
    [SerializeField]
    private float angularAcceleration = 0.0f;

    private enum PositionType { Euler, Kinematic };
    private enum RotationType { Euler, Kinematic };

    [SerializeField]
    private PositionType positionType = PositionType.Euler;
    [SerializeField]
    private RotationType rotationType = RotationType.Euler;

    /*
     *  Lab 2 Step 1
     */
    [SerializeField]
    private float startingMass;
    [SerializeField]
    private float mass;
    private float massInverse;

    /*
     *  Lab 2 Step 2
     */
    private Vector2 force;
    private const float GRAVITY = -9.8f;
    private bool gravityActive = true;

    /*
     *  Lab 2
     *  Spring variables.
     */
    [SerializeField]
    private Transform springAnchor;
    [SerializeField]
    private float springRestLength = 0.0f;
    [SerializeField]
    private float springStiffness = 0.0f;

    /*
     *  Lab 2
     *  Surface normal variables.
     */
    [SerializeField]
    private Vector2 surfaceNormal;


    private void Awake()
    {
        //SetInitailVelocity();
    }
    // Start is called before the first frame update
    void Start()
    {
        Mass = startingMass;

        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check user selection from menu items.
        //GetInspectorItems();

        UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        UpdateAcceleration();

        //AngularVelocityScalar(-Mathf.Sin(50f) * 2f);

        // Lab 1 Apply to transform.
        transform.position = position;

        transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);

        /*
         *  Lab 1 Step 4
         */

        // Test
        // Note: Set the initial velocity to integral value
        //acceleration.x = -Mathf.Sin(Time.fixedTime);
        //angularAcceleration = -Mathf.Sin(Time.fixedTime) * 360f;

        // f_gravity = f = mg
        //Vector2 f_gravity = mass * new Vector2(0.0f, -9.8f);
        //AddForce(f_gravity);
        //AddForce(ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up));
        //AddForce(ForceGenerator.GenerateForce_Spring(springAnchor.position, transform.position, springRestLength, springStiffness));
        AddForce(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, Vector2.up));
        //AddForce(ForceGenerator.GenerateForce_Sliding(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, Vector2.up), Vector2.right));

        AddForce(ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, Vector2.up), surfaceNormal));

        return;
    }

    /*
     * Integrate user friendly menu.
     * 
     */
     // Get selectable items from the inspector menu.
     private void GetInspectorItems()
    {
        /*
         *  Lab 1 Step 3
         */

        // Integrate.
        if (positionType == PositionType.Euler)
        {
            UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        }
        else if (positionType == PositionType.Kinematic)
        {
            UpdatePositionKinematic(Time.fixedDeltaTime);
        }

        if (rotationType == RotationType.Euler)
        {
            UpdateRotationEulerExplicit(Time.fixedDeltaTime);
        }
        else if (rotationType == RotationType.Kinematic)
        {
            UpdateRotationKinematic(Time.fixedDeltaTime);
        }

        return;
    }

    /*
     *  Lab 1 Step 2
     *  Integration algorithms.
     */
    private void UpdatePositionEulerExplicit(float _deltaTime)
    {
        /*  
         *  x(t + dt) = x(t) + v(t)dt
         *  Euler's method:
         *  F(t + dt) = F(t) + f(t)dt
         *                   + (dF / dt)dt
         */
        position += velocity * _deltaTime;

        /*
         *  v(t + dt) = v(t) + a(t)dt
         */
        velocity += acceleration * _deltaTime;

        return;
    }

    private void UpdatePositionKinematic(float _deltaTime)
    {
        /*
         *  x(t + dt) = x(t) + v(t)dt + 1/2 a(t) dt^2
         */

        position += velocity * _deltaTime + 0.5f * acceleration * _deltaTime * _deltaTime;

        /*
         *  v(t + dt) = v(t) + a(t)dt
         */
        velocity += acceleration * _deltaTime;

        return;
    }

    private void UpdateRotationEulerExplicit(float _deltaTime)
    {
        /*  
         *  x(t + dt) = x(t) + v(t)dt
         *  Euler's method:
         *  F(t + dt) = F(t) + f(t)dt
         *                   + (dF / dt)dt
         */
        rotation += angularVelocity * _deltaTime;

        /*
         *  v(t + dt) = v(t) + a(t)dt
         */
        angularVelocity += angularAcceleration * _deltaTime;

        return;
    }

    private void UpdateRotationKinematic(float _deltaTime)
    {
        /*
         *  x(t + dt) = x(t) + v(t)dt + 1/2 a(t) dt^2
         */

        rotation += angularVelocity * _deltaTime + 0.5f * angularAcceleration * _deltaTime * _deltaTime;

        /*
         *  v(t + dt) = v(t) + a(t)dt
         */
        angularVelocity += angularAcceleration * _deltaTime;

        return;
    }

    private void SetInitailVelocity()
    {
        // Set the initial velocity to integral value of acceleration.
        velocity.x = Mathf.Cos(Time.fixedTime);
        angularVelocity = Mathf.Cos(Time.fixedTime) * 360f;

        return;
    }

    private void OnValidate()
    {
        if (positionType == PositionType.Euler)
        {
            Debug.Log("Changed to Kinematic");
        }
        else if (positionType == PositionType.Kinematic)
        {
            //Debug.Log("Changed to Euler");
        }
    }

    // Mass accessors.
    public float Mass
    {
        get
        {
            return mass;
        }

        set
        {
            mass = value > 0.0f ? value : 0.0f;
            massInverse = mass > 0.0f ? 1.0f / mass : 0.0f;
        }

    }

    public void AddForce(Vector2 _newForce)
    {
        // D'Alembert's law.
        force += _newForce;

        return;
    }

    private void UpdateAcceleration()
    {
        // Newton's second law.
        acceleration = massInverse * force;

        force = Vector2.zero;

        return;
    }

    private void ToggleGravity()
    {
        gravityActive = !gravityActive;

        return;
    }
}
