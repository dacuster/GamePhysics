using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    /*
     *  Lab 1 Step 1
     *  Define particle variables.
     */
    [Header("Particle")]
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

    [Header("Velocity Options")]
    [SerializeField]
    private PositionType positionType = PositionType.Euler;
    [SerializeField]
    private RotationType rotationType = RotationType.Euler;

    /*
     *  Lab 2 Step 1
     */
    [Header("Mass")]
    [SerializeField]
    private float startingMass = 0.0f;
    [SerializeField]
    private float mass = 0.0f;
    private float massInverse = 0.0f;

    /*
     *  Lab 2 Step 2
     */
    private Vector2 force;

    /*
     *  Lab 2
     *  Gravity variables.
     */
    private const float GRAVITY = -9.8f;
    [Header("Gravity")]
    [SerializeField]
    private Vector2 worldUp = Vector2.up;

    /*
     *  Lab 2
     *  Spring variables.
     */
    [Header("Spring")]
    [SerializeField]
    private Transform springAnchor = null;
    [SerializeField]
    private float springRestLength = 0.0f;
    [SerializeField]
    private float springStiffness = 0.0f;
    [SerializeField]
    private float springDamping = 0.0f;
    [SerializeField]
    private float springConstant = 0.0f;

    /*
     *  Lab 2
     *  Surface normal variables.
     */
    [Header("Surface Normal")]
    [SerializeField]
    private Vector2 surfaceNormal = Vector2.zero;

    /*
     *  Lab 2
     *  Drag variables.
     */
    [Header("Drag")]
    [SerializeField]
    private Vector2 fluidVelocity = Vector2.zero;
    [SerializeField]
    private float fluidDensity = 0.0f;
    [SerializeField]
    private float objectCrossSection = 0.0f;
    [SerializeField]
    private float dragCoefficient = 0.0f;

    /*
     *  Lab 2
     *  Friction variables.
     */
    [Header("Friction")]
    [SerializeField]
    private float kinematicFrictionCoefficient = 0.0f;
    [SerializeField]
    private Vector2 kinematicNormalForce = Vector2.zero;
    [SerializeField]
    private float staticFrictionCoefficient = 0.0f;
    [SerializeField]
    private Vector2 staticFrictionNormal = Vector2.zero;
    [SerializeField]
    private Vector2 staticFrictionOpposingForce = Vector2.zero;


    /*
     *  Lab 2
     *  Sliding variables.
     */
    [Header("Sliding")]
    [SerializeField]
    private Vector2 slidingNormalForce = Vector2.zero;


    /*
     *  Lab 2
     *  Inspector selections.
     */
    [Header("Forces")]
    [SerializeField]
    private bool gravityActive = false;
    [SerializeField]
    private bool normalActive = false;
    [SerializeField]
    private bool slidingActive = false;
    [SerializeField]
    private bool staticFrictionActive = false;
    [SerializeField]
    private bool kinematicFrictionActive = false;
    [SerializeField]
    private bool dragActive = false;
    [SerializeField]
    private bool springActive = false;
    [SerializeField]
    private bool dampingSpringActive = false;


    /*
     *  Lab 3 Step 1
     *  Moment of Inertia
     */
    [SerializeField]
    private float inertia = 0.0f;
    private float inertiaInverse = 0.0f;
    /*
     *  Lab 3 Step 2
     *  Torque
     */
    [Header("Torque")]
    [SerializeField]
    private float torque = 0.0f;
    [SerializeField]
    private Vector2 localCenterOfMass = Vector2.zero;
    private Vector2 worldCenterOfMass = Vector2.zero;
    [SerializeField]
    private float innerRingRadius = 0.0f;
    [SerializeField]
    private Vector2 torqueForce = Vector2.zero;
    [SerializeField]
    private Vector2 torqueArm = Vector2.zero;
    [SerializeField]
    private bool torqueActive = false;

    [SerializeField]
    private Inertia2D.Type inertiaType = Inertia2D.Type.Disk;



    private void Awake()
    {
        //SetInitailVelocity();
    }
    // Start is called before the first frame update
    void Start()
    {
        Mass = startingMass;
        // Set inertia algorithm based on selected object.
        CheckObjectType();

        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check user selection from menu items.
        GetInspectorItems();

        UpdateAcceleration();

        // Lab 1 Apply to transform.
        transform.position = position;

        UpdateWorldCenterOfMass();

        transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);

        // Lab 3 Step 3
        // Update angluar acceleration after applying rotation.
        UpdateAngularAcceleration();

        return;
    }

    /*
     *  Lab 1
     *  Integrate user friendly menu.
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

        if (gravityActive)
        {
            // Gravity force.
            AddForce(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, Vector2.up));
        }

        if (springActive)
        {
            // Spring force.
            AddForce(ForceGenerator.GenerateForce_Spring(springAnchor.position, transform.position, springRestLength, springStiffness));
        }

        if (staticFrictionActive)
        {
            // Static friction force.
            AddForce(ForceGenerator.GenerateForce_Friction_Static(staticFrictionNormal, staticFrictionOpposingForce, staticFrictionCoefficient));
        }

        if (kinematicFrictionActive)
        {
            // Kinematic friction force.
            AddForce(ForceGenerator.GenerateForce_Friction_Kinetic(kinematicNormalForce, velocity, kinematicFrictionCoefficient));
        }

        if (slidingActive)
        {
            // Sliding force.
            AddForce(ForceGenerator.GenerateForce_Sliding(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, worldUp), slidingNormalForce));
        }

        if (dragActive)
        {
            // Drag force.
            AddForce(ForceGenerator.GenerateForce_Drag(velocity, fluidVelocity, fluidDensity, objectCrossSection, dragCoefficient));
        }

        if (normalActive)
        {
            // Normal force.
            AddForce(ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, worldUp), surfaceNormal));
        }

        if (dampingSpringActive)
        {
            // Damping spring force.
            AddForce(ForceGenerator.GenerateForce_Spring_Damping(position, springAnchor.position, springRestLength, springStiffness, springDamping, springConstant, velocity));
        }

        if (torqueActive)
        {
            AddTorque(ForceGenerator.GenerateForce_Torque(worldCenterOfMass, torqueArm, torqueForce));
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

    /*
     *  Lab 3 Step 2
     *  Apply torque to angular acceleration.
     */
     private void UpdateAngularAcceleration()
    {
        // Newton's second law.
        angularAcceleration = inertiaInverse * torque;

        torque = 0.0f;

        return;
    }

    /*
     *  Lab 3
     *  Add torque additional torque. 
     */
    private void AddTorque(float _newTorque)
    {
        torque += _newTorque;

        return;
    }

    /*
     *  Lab 3
     *  Inertia accessors.
     */
    public float Inertia
    {
        get
        {
            return inertia;
        }

        set
        {
            inertia = value > 0.0f ? value : 0.0f;
            inertiaInverse = inertia > 0.0f ? 1.0f / inertia : 0.0f;
        }
    }

    // Check the inspector for selected object type.
    private void CheckObjectType()
    {
        switch (inertiaType)
        {
            case Inertia2D.Type.Disk:
                // Get disk inertia. (Any local scale dimension will be diameter)
                Inertia = Inertia2D.Disk(mass, transform.localScale.x * 0.5f);
                break;
            case Inertia2D.Type.Ring:
                // Get the ring inertia.
                Inertia = Inertia2D.Ring(mass, innerRingRadius, transform.localScale.x * 0.5f);
                break;
            case Inertia2D.Type.Rectangle:
                // Get the rectangle inertia.
                Inertia = Inertia2D.Rectangle(mass, transform.localScale.x, transform.localScale.y);
                break;
            case Inertia2D.Type.Square:
                // Get the square inertia.
                Inertia = Inertia2D.Square(mass, transform.localScale.x);
                break;
            case Inertia2D.Type.Rod:
                // Get the longer of the two dimensions.
                float length = transform.localScale.x > transform.localScale.y ? transform.localScale.x : transform.localScale.y;
                // Get the rod inertia.
                Inertia = Inertia2D.Rod(mass, length);
                break;
            default:
                break;
        }
    }

    // Get world center of mass based on local center of mass and current position of particle.
    private void UpdateWorldCenterOfMass()
    {
        worldCenterOfMass = localCenterOfMass + position;

        return;
    }
}
