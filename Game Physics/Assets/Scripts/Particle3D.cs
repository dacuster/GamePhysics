using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Particle3D : MonoBehaviour
{
    // Quaternion Class
    public class Custom_Quaternion
    {
        // w scalar
        private float w = 1.0f;
        // x rotation    
        private float x = 0.0f;
        // y rotation     
        private float y = 0.0f;
        // z rotation    
        private float z = 0.0f;



        // variable getters and setters
        public float W { get => w; set => w = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }

        public Custom_Quaternion(float _x, float _y, float _z, float _w)
        {
            X = _x;
            Y = _y;
            Z = _z;
            W = _w;
        }

        public Custom_Quaternion() { }

        public void normalize()
        {
                X /= magnitude;
                Y /= magnitude;
                Z /= magnitude;
                W /= magnitude;

            return;
        }

        public Custom_Quaternion normalized
        {
            get
            {
                return new Custom_Quaternion
                (
                    X / magnitude,
                    Y / magnitude,
                    Z / magnitude,
                    W / magnitude
                );
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z + w * w;
            }
        }

        // calculate magnitude
        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(sqrMagnitude);
            }
        }

        public static Custom_Quaternion identity
        {
            get
            {
                return new Custom_Quaternion(1.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        public Vector3 eulerAngles
        {
            get
            {
                return new Vector3(X, Y, Z);

            }
        }

        // TODO: implement scalar and quaternion multiplacation
        public static Custom_Quaternion operator* (Custom_Quaternion left, float right)
        {
            Custom_Quaternion quaternion = new Custom_Quaternion
            {
                W = left.W * right,
                X = left.X * right,
                Y = left.Y * right,
                Z = left.Z * right
            };

            return quaternion;
        }

        public Custom_Quaternion AngleAxisRotate(float angle, Vector3 axis)
        {
            float halfAngle = angle * 0.5f;
            float sinHalfAngle = Mathf.Sin(halfAngle);

            Custom_Quaternion newQuaternion = new Custom_Quaternion
            {
                W = Mathf.Cos(halfAngle),
                X = axis.x * sinHalfAngle,
                Y = axis.y * sinHalfAngle,
                Z = axis.z * sinHalfAngle
            };

            newQuaternion = newQuaternion * this;

            return newQuaternion;
        }

        // TODO: implement vector and quaternion multiplacation
        public static Custom_Quaternion operator* (Vector3 left, Custom_Quaternion right)
        {
            //right = right.normalized;

            float X = right.X;
            float Y = right.Y;
            float Z = right.Z;
            float W = right.W;

            Matrix4x4 rotationMatrix = new Matrix4x4
            {

                // X tranformation.
                m00 = 1.0f - 2.0f * Y * Y - 2.0f * Z * Z,
                m01 = 2.0f * X * Y - 2.0f * W * Z,
                m02 = 2.0f * X * Z + 2.0f * W * Y,
                m03 = 0.0f,

                // Y tranformation.
                m10 = 2.0f * X * Y + 2.0f * W * Z,
                m11 = 1.0f - 2.0f * X * X - 2.0f * Z * Z,
                m12 = 2.0f * Y * Z + 2.0f * W * X,
                m13 = 0.0f,

                // Z tranformation.
                m20 = 2.0f * X * Z - 2.0f * W * Y,
                m21 = 2.0f * Y * Z - 2.0f * W * X,
                m22 = 1.0f - 2.0f * X * X - 2.0f * Y * Y,
                m23 = 0.0f,

                // W tranformation.
                m30 = 0.0f,
                m31 = 0.0f,
                m32 = 0.0f,
                m33 = 1.0f
            };

            rotationMatrix = rotationMatrix.transpose;

            left = rotationMatrix.MultiplyPoint3x4(left);

            Custom_Quaternion newQuaternion = new Custom_Quaternion
            {
                X = left.x,
                Y = left.y,
                Z = left.z,
                W = 1.0f
            };

            return newQuaternion;
        }

        // Multiply two quaternions together.
        public static Custom_Quaternion operator * (Custom_Quaternion left, Custom_Quaternion right)
        {
            Vector3 leftVector = new Vector3(left.X, left.Y, left.Z);
            Vector3 rightVector = new Vector3(right.X, right.Y, right.Z);
            
            // Calculate the real(W) of the new quaternion.
            float real = left.W * right.W - Vector3.Dot(leftVector, rightVector);

            Vector3 newVector = left.W * rightVector +right.W * leftVector + Vector3.Cross(leftVector, rightVector);

            //Quaternion quaternion = new Quaternion
            //{
            //    W = left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z,

            //    X = left.W * right.X + left.X * right.W + left.Y * right.Z - left.Z * right.Y,

            //    Y = left.W * right.Y - left.X * right.Z + left.Y * right.W + left.Z * right.X,

            //    Z = left.W * right.Z + left.X * right.Y - left.Y * right.X + left.Z * right.W
            //};

            Custom_Quaternion quaternion = new Custom_Quaternion(real, newVector.x, newVector.y, newVector.z);

            return quaternion;
        }

        // Add two quaternions together.
        public static Custom_Quaternion operator + (Custom_Quaternion left, Custom_Quaternion right)
        {
            Custom_Quaternion quaternion = new Custom_Quaternion
            {
                W = left.W + right.W,
                X = left.X + right.X,
                Y = left.Y + right.Y,
                Z = left.Z + right.Z
            };

            return quaternion;
        }

        public void Rotate(ref Vector3 rotation)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4
            {

                // X tranformation.
                m00 = 1.0f - 2.0f * Y * Y - 2.0f * Z * Z,
                m01 = 2.0f * X * Y - 2.0f * W * Z,
                m02 = 2.0f * X * Z + 2.0f * W * Y,
                m03 = 0.0f,

                // Y tranformation.
                m10 = 2.0f * X * Y + 2.0f * W * Z,
                m11 = 1.0f - 2.0f * X * X - 2.0f * Z * Z,
                m12 = 2.0f * Y * Z + 2.0f * W * X,
                m13 = 0.0f,

                // Z tranformation.
                m20 = 2.0f * X * Z - 2.0f * W * Y,
                m21 = 2.0f * Y * Z - 2.0f * W * X,
                m22 = 1.0f - 2.0f * X * X - 2.0f * Y * Y,
                m23 = 0.0f,

                // W tranformation.
                m30 = 0.0f,
                m31 = 0.0f,
                m32 = 0.0f,
                m33 = 1.0f
            };

            rotation = rotationMatrix.MultiplyPoint3x4(rotation);

            return;
        }

    }

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
    // 3D rotation of this object.
    private Custom_Quaternion rotation = Custom_Quaternion.identity;
    [SerializeField]
    // Angular velocity of this object.
    private Vector3 angularVelocity = Vector3.zero;
    [SerializeField]
    // Angular acceleration of this object.
    private Vector3 angularAcceleration = Vector3.zero;

    // Position and rotation algorithm types.
    private enum PositionType { Euler, Kinematic };
    private enum RotationType { Euler, Kinematic };

    private enum ShapeType { SolidSphere, HollowSphere, SolidCude, HollowCube, SolidCylinder, HollowCylinder };
    // Velocity option fields.
    [Header("Velocity Options")]

    [SerializeField]
    // Algorithm used for position.
    private PositionType positionType = PositionType.Euler;
    [SerializeField]
    // Algorithm used for rotation.
    private RotationType rotationType = RotationType.Euler;

    [Header("Shape Options")]
    [SerializeField]
    // shape Types
    private ShapeType shapeType = ShapeType.SolidSphere;
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
    private Vector3 torque = Vector3.zero;
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

       // Quaternion quaternion = new Quaternion(1, 1, 1, 1);
       // quaternion = quaternion.normalized;

        return;
    }

    // Physics update method.
    void FixedUpdate()
    {

        // Check algorithm type from user selection menu items.
        GetInspectorItems();

        // Update acceleration before setting transforms.
        UpdateAcceleration();

        if(name == "Player")
        {
            UpdateAngularAcceleration();
            Torque = Vector3.zero;

            PlayerController.instance.PlayerControls();
        }

        // Apply position to Unity's transform component.
        transform.position = Position;
        // Apply rotation to Unity's transform component.
        //transform.eulerAngles = AngularVelocity;
        
        transform.rotation = new Quaternion(Rotation.W, Rotation.X, Rotation.Y, Rotation.Z);
        

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

        // Shape Types
        if(shapeType == ShapeType.SolidSphere)
        {
            GetComponent<Inertia3D>().SolidSphereIneritaTensor();
        }

        if (shapeType == ShapeType.HollowSphere)
        {
            GetComponent<Inertia3D>().HollowSphereIneritaTensor();
        }

        if (shapeType == ShapeType.SolidCude)
        {
            GetComponent<Inertia3D>().SolidCubeIneritaTensor();
        }

        if (shapeType == ShapeType.HollowCube)
        {
            GetComponent<Inertia3D>().HollowCubeIneritaTensor();
        }

        if (shapeType == ShapeType.SolidCylinder)
        {
            GetComponent<Inertia3D>().SolidCylinderIneritaTensor();
        }

        if (shapeType == ShapeType.HollowCylinder)
        {
            GetComponent<Inertia3D>().HollowCylinderIneritaTensor();
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
        //Rotation += AngularVelocity * deltaTime;

        Rotation += (AngularVelocity * Rotation) * deltaTime * 0.5f;
        //Rotation = Rotation.AngleAxisRotate(10.0f, AngularVelocity);

        Rotation.normalize();

        // v(t + dt) = v(t) + a(t)dt
        AngularVelocity += AngularAcceleration * deltaTime;

        return;
    }

    // Update rotation using kinematic method.
    private void UpdateRotationKinematic(float deltaTime)
    {
        // x(t + dt) = x(t) + v(t)dt + 1/2 a(t) dt^2
        //Rotation += AngularVelocity * deltaTime + 0.5f * AngularAcceleration * deltaTime * deltaTime;

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
        // TODO: Fix angular acceleration implementation.
        AngularAcceleration = inertiaInv * Torque;
    }

   

    // Apply torque
    public void ApplyTorque(Vector3 pos, Vector3 newForce)
    {
        // t = px*fy - py*fx
        Torque = Vector3.Cross(pos, newForce);
    }

    // get direction to move forward
    public Vector3 CalculateDirection()
    {
        // TODO: Calculate direction convert for quaternion use.
        //float radians = Rotation * Mathf.PI / 180.0f;
        //Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
        //return direction;
        return Vector3.zero;
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
    public Custom_Quaternion Rotation { get => rotation; set => rotation = value; }

    // Angular Velocity Accessor.
    public Vector3 AngularVelocity { get => angularVelocity; set => angularVelocity = value; }
    
    // Angular Acceleration Accessor.
    public Vector3 AngularAcceleration { get => angularAcceleration; set => angularAcceleration = value; }

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
    public Vector3 Torque { get => torque; set => torque = value; }

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

            // TODO: Implement quaternion rotation into scene GUI.
            // Get eulerAngles from transform and make Quaternion with w = 1 and x, y, z = eulerAngles???
            //particle.Rotation = particle.transform.rotation.eulerAngles.z;
        }
    }
}
