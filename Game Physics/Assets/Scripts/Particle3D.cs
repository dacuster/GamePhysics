using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Particle3D : MonoBehaviour
{
    /***********************************************
    **  Lab 1 Step 1. Define particle variables.  **
    ***********************************************/

    // Particle transform fields.
    [Header("Transform")]

    [SerializeField]
    // Position of this object.
    private Vector3 position = Vector3.zero;
    [SerializeField]
    // Velocity of this object.
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    // Acceleration of this object.
    private Vector3 acceleration = Vector3.zero;
    
    // Add a space in the inspector.
    [Space]

    [SerializeField]
    // 2D rotation of this object. (z rotation)
    private float rotation = 0.0f;
    [SerializeField]
    // Angular velocity of this object.
    private float angularVelocity = 0.0f;
    [SerializeField]
    // Angular acceleration of this object.
    private float angularAcceleration = 0.0f;

    // Position and rotation algorithm types.
    private enum PositionType { Euler, Kinematic };
    private enum RotationType { Euler, Kinematic };

    // Velocity option fields.
    [Header("Velocity Options")]

    [SerializeField]
    // Algorithm used for position.
    private PositionType positionType = PositionType.Euler;
    [SerializeField]
    // Algorithm used for rotation.
    private RotationType rotationType = RotationType.Euler;

    
    /********************
    **  Lab 2 Step 1.  **
    ********************/

    // Mass fields.
    [Header("Mass")]

    [SerializeField]
    // Starting mass of this object.
    private float startingMass = 0.0f;
    // Current mass of this object.
    private float mass = 0.0f;
    // Current invert mass of this object.
    private float massInverse = 0.0f;

    
    /********************
    **  Lab 2 Step 2.  **
    ********************/

    // Force applied to this object.
    [SerializeField]
    private Vector3 force;


    /********************************
    **  Lab 2. Gravity variables.  **
    ********************************/

    // Gravity fields.
    [Header("Gravity")]

    // Gravity constant.
    private const float GRAVITY = -9.8f;
    [SerializeField]
    // Upward direction of the world.
    private Vector3 worldUp = Vector3.up;

    // Torque Stuff
    [Header("Torque")]

    [SerializeField]
    private float torque = 0.0f;
    private float inertia = 0.0f;
    private float inertiaInv = 0.0f;

    /*******************************
    **  Lab 2. Spring variables.  **
    *******************************/

    // Spring fields.
    [Header("Spring")]

    [SerializeField]
    // Spring anchor point object.
    private Transform springAnchor = null;
    [SerializeField]
    // Spring resting length.
    private float springRestLength = 0.0f;
    [SerializeField]
    // Spring stiffness.
    private float springStiffness = 0.0f;
    [SerializeField]
    // Spring damping.
    private float springDamping = 0.0f;
    [SerializeField]
    // Spring constant.
    private float springConstant = 0.0f;


    /***************************************
    **  Lab 2. Surface normal variables.  **
    ***************************************/

    // Surface normal fields.
    [Header("Surface Normal")]

    [SerializeField]
    // Normal force direction.
    private Vector3 surfaceNormal = Vector3.zero;


    /******************************
    **  Lab 2.  Drag variables.  **
    ******************************/

    // Drag fields.
    [Header("Drag")]

    [SerializeField]
    // Fluid velocity.
    private Vector3 fluidVelocity = Vector3.zero;
    [SerializeField]
    // Fluid density.
    private float fluidDensity = 0.0f;
    [SerializeField]
    // Object cross section.
    private float objectCrossSection = 0.0f;
    [SerializeField]
    // Drag coefficient.
    private float dragCoefficient = 0.0f;


    /*********************************
    **  Lab 2. Friction variables.  **
    *********************************/

    // Friction fields.
    [Header("Friction")]

    [SerializeField]
    // Kinematic friction coefficient.
    private float kinematicFrictionCoefficient = 0.0f;
    [SerializeField]
    // Normal force of kinematic friction.
    private Vector3 kinematicNormalForce = Vector3.zero;
    [SerializeField]
    // Static friction coefficient.
    private float staticFrictionCoefficient = 0.0f;
    [SerializeField]
    // Normal force of static friction.
    private Vector3 staticFrictionNormal = Vector3.zero;
    [SerializeField]
    // Opposing force to static friction.
    private Vector3 staticFrictionOpposingForce = Vector3.zero;


    /********************************
    **  Lab 2. Sliding variables.  **
    ********************************/

    // Sliding fields.
    [Header("Sliding")]

    [SerializeField]
    // Sliding normal force.
    private Vector3 slidingNormalForce = Vector3.zero;


    /***********************************
    **  Lab 2. Inspector selections.  **
    ***********************************/

    // Active forces options.
    [Header("Activate Forces")]

    [SerializeField]
    // Gravitational force option.
    private bool gravityActive = false;
    [SerializeField]
    // Normal force option.
    private bool normalActive = false;
    [SerializeField]
    // Sliding force option.
    private bool slidingActive = false;
    [SerializeField]
    // Static friction force option.
    private bool staticFrictionActive = false;
    [SerializeField]
    // Kinematic friction force option.
    private bool kinematicFrictionActive = false;
    [SerializeField]
    // Drag force option.
    private bool dragActive = false;
    [SerializeField]
    // Spring force option.
    private bool springActive = false;
    [SerializeField]
    // Damping spring force option.
    private bool dampingSpringActive = false;

    public bool isProjectile = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set the mass as the starting mass.
        Mass = startingMass;

        return;
    }

    // Physics update method.
    void FixedUpdate()
    {
        CubeInvInertiaCalc();

        // Check algorithm type from user selection menu items.
        GetInspectorItems();

        // Update acceleration before setting transforms.
        UpdateAcceleration();

        if(name == "Player")
        {
            UpdateAngularAcceleration();
            Torque = 0;

            PlayerController.instance.PlayerControls();
        }

        // Apply position to Unity's transform component.
        transform.position = Position;
        // Apply rotation to Unity's transform component. (z rotation)
        transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);

        return;
    }


    /*******************************************
    **  Lab 1. Integrate user friendly menu.  **
    *******************************************/

    // Get selectable items from the inspector menu.
    private void GetInspectorItems()
    {
        /*******************************
        **  Lab 1 Step 3. Integrate.  **
        *******************************/

        /**************************************
        **  Update position based on method. **
        **************************************/

        // Euler explicit method.
        if (positionType == PositionType.Euler)
        {
            UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        }
        // Kinematic method.
        else if (positionType == PositionType.Kinematic)
        {
            UpdatePositionKinematic(Time.fixedDeltaTime);
        }

        // Euler explicit rotation method.
        if (rotationType == RotationType.Euler)
        {
            UpdateRotationEulerExplicit(Time.fixedDeltaTime);
        }
        // Kinematic rotation method.
        else if (rotationType == RotationType.Kinematic)
        {
            UpdateRotationKinematic(Time.fixedDeltaTime);
        }

        // Apply gravitational force if it is active.
        if (gravityActive)
        {
            // Gravitational force.
            AddForce(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, Vector3.up));
        }

        // Apply spring force if it is active.
        if (springActive)
        {
            // Spring force.
            AddForce(ForceGenerator.GenerateForce_Spring(Position, springAnchor.position, springRestLength, springStiffness));
        }

        // Apply static friction force if it is active.
        if (staticFrictionActive)
        {
            // Static friction force.
            AddForce(ForceGenerator.GenerateForce_Friction_Static(staticFrictionNormal, staticFrictionOpposingForce, staticFrictionCoefficient));
        }

        // Apply kinematic friction force if it is active.
        if (kinematicFrictionActive)
        {
            // Kinematic friction force.
            AddForce(ForceGenerator.GenerateForce_Friction_Kinetic(kinematicNormalForce, Velocity, kinematicFrictionCoefficient));
        }

        // Apply sliding force if it is active.
        if (slidingActive)
        {
            // Sliding force.
            AddForce(ForceGenerator.GenerateForce_Sliding(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, worldUp), slidingNormalForce));
        }

        // Apply drag force if it is active.
        if (dragActive)
        {
            // Drag force.
            AddForce(ForceGenerator.GenerateForce_Drag(Velocity, fluidVelocity, fluidDensity, objectCrossSection, dragCoefficient));
        }

        // Apply normal force if it is active.
        if (normalActive)
        {
            // Normal force.
            AddForce(ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(mass, GRAVITY, worldUp), surfaceNormal));
        }

        // Apply damping spring force if it is active.
        if (dampingSpringActive)
        {
            // Damping spring force.
            AddForce(ForceGenerator.GenerateForce_Spring_Damping(Position, springAnchor.position, springRestLength, springStiffness, springDamping, springConstant, Velocity));
        }

        return;
    }


    /********************************************
    **  Lab 1 Step 2. Integration algorithms.  **
    ********************************************/

    // Update position using euler explicit method.
    private void UpdatePositionEulerExplicit(float deltaTime)
    {
        /*************************************
        **  x(t + dt) = x(t) + v(t)dt       **
        **  Euler's method:                 **
        **  F(t + dt) = F(t) + f(t)dt       **
        **                   + (dF / dt)dt  **
        *************************************/
        Position += Velocity * deltaTime;

        // v(t + dt) = v(t) + a(t)dt
        Velocity += Acceleration * deltaTime;

        Debug.DrawRay(Position, Velocity.normalized * 3.0f, Color.white);

        return;
    }

    // Update position using kinematic method.
    private void UpdatePositionKinematic(float deltaTime)
    {
        // x(t + dt) = x(t) + v(t)dt + 1/2 a(t) dt^2
        Position += Velocity * deltaTime + 0.5f * Acceleration * deltaTime * deltaTime;

        // v(t + dt) = v(t) + a(t)dt
        Velocity += Acceleration * deltaTime;

        return;
    }

    // Update rotation using euler explicit method.
    private void UpdateRotationEulerExplicit(float deltaTime)
    {
        /*************************************  
        **  x(t + dt) = x(t) + v(t)dt       **
        **  Euler's method:                 **
        **  F(t + dt) = F(t) + f(t)dt       **
        **                   + (dF / dt)dt  **
        *************************************/
        Rotation += AngularVelocity * deltaTime;

        // v(t + dt) = v(t) + a(t)dt
        AngularVelocity += AngularAcceleration * deltaTime;

        return;
    }

    // Update rotation using kinematic method.
    private void UpdateRotationKinematic(float deltaTime)
    {
        // x(t + dt) = x(t) + v(t)dt + 1/2 a(t) dt^2
        Rotation += AngularVelocity * deltaTime + 0.5f * AngularAcceleration * deltaTime * deltaTime;

        // v(t + dt) = v(t) + a(t)dt
        AngularVelocity += AngularAcceleration * deltaTime;

        return;
    }

    // Add a new force to the current force.
    public void AddForce(Vector3 newForce)
    {
        // D'Alembert's law.
        Force += newForce;

        return;
    }

    // Update the acceleration.
    private void UpdateAcceleration()
    {
        // Newton's second law.
        Acceleration = massInverse * Force;

        // Reset force so new forces can be applied.
        Force = Vector3.zero;

        return;
    }

    // Update angular acceleration
    void UpdateAngularAcceleration()
    {
        AngularAcceleration = inertiaInv * Torque;
    }

    // Calulate Inverse Cube Inertia
    void CubeInvInertiaCalc()
    {
        inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Apply torque
    public void ApplyTorque(Vector3 pos, Vector3 newForce)
    {
        // t = px*fy - py*fx
        Torque = pos.x * newForce.y - pos.y * newForce.x;
    }

    // get direction to move forward
    public Vector3 CalculateDirection()
    {
        float radians = Rotation * Mathf.PI / 180.0f;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
        return direction;
    }
    /**************************************
    **          Start Accessors          ** 
    **************************************/

    /*********************************
    **  Position Related Accessors  **
    *********************************/

    // Position Accessor
    public Vector3 Position { get => position; set => position = value; }

    // Velocity Accessor.
    public Vector3 Velocity { get => velocity; set => velocity = value; }

    // Acceleration Accessor.
    public Vector3 Acceleration { get => acceleration; set => acceleration = value; }

    /*************************************
    **  End Position Related Accessors  **
    *************************************/


    /***************************************
    **  Start Rotation Related Accessors  **
    ***************************************/

    // Rotation Accessor.
    public float Rotation { get => rotation; set => rotation = value; }

    // Angular Velocity Accessor.
    public float AngularVelocity { get => angularVelocity; set => angularVelocity = value; }
    
    // Angular Acceleration Accessor.
    public float AngularAcceleration { get => angularAcceleration; set => angularAcceleration = value; }

    /*************************************
    **  End Rotation Related Accessors  **
    *************************************/

    // Mass Accessor.
    public float Mass
    {
        get => mass;

        set
        {
            // Prevent negative mass.
            mass = value > 0.0f ? value : 0.0f;
            
            // Invert the mass. (Prevent divide by 0)
            massInverse = mass > 0.0f ? 1.0f / mass : 0.0f;

            return;
        }
    }

    public float MassInverse
    {
        get => massInverse;
    }

    // Force Accessor.
    public Vector3 Force { get => force; set => force = value; }

    // Torque Accessor.
    public float Torque { get => torque; set => torque = value; }

    /************************************
    **          End Accessors          ** 
    ************************************/
}

[CustomEditor(typeof(Particle3D))]
public class Particle3DEditor : Editor
{
    private void OnSceneGUI()
    {
        // Get the particle associated with this object.
        Particle3D particle = (Particle3D)target;

        // Update Particle2D position and rotation based on the Transform component. Only works when the editor isn't in play mode.
        if (!Application.isPlaying)
        {
            particle.Position = particle.transform.position;
            particle.Rotation = particle.transform.rotation.eulerAngles.z;
        }
    }
}
