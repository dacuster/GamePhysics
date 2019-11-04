using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Particle3D : MonoBehaviour
{
    [System.Serializable]
    // Quaternion Class
    public class Custom_Quaternion
    {
        [SerializeField]
        // Quaternion in Vector4 form
        Vector4 quaternion = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        //[SerializeField]
        //// w scalar
        //private float w = 1.0f;
        //[SerializeField]
        //// x rotation    
        //private float x = 0.0f;
        //[SerializeField]
        //// y rotation     
        //private float y = 0.0f;
        //[SerializeField]
        //// z rotation    
        //private float z = 0.0f;



        // variable getters and setters
        public float W { get => quaternion.w; set => quaternion.w = value; }
        public float X { get => quaternion.x; set => quaternion.x = value; }
        public float Y { get => quaternion.y; set => quaternion.y = value; }
        public float Z { get => quaternion.z; set => quaternion.z = value; }

        public Custom_Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Custom_Quaternion(float angleOfRotation, Vector3 rotationAxis)
        {
            // Get half angle for quicker quaternion creation.
            float halfAngle = angleOfRotation * 0.5f;

            // W = Cos(halfAngle)
            W = Mathf.Cos(halfAngle);

            // Normalize rotation axis in case it isn't already.
            rotationAxis.Normalize();

            // v = n * sin(halfAngle)
            rotationAxis *= Mathf.Sin(halfAngle);

            // Apply vector values to new quaternion.
            X = rotationAxis.x;
            Y = rotationAxis.y;
            Z = rotationAxis.z;

            return;
        }

        public Custom_Quaternion() { }

        public void Normalize()
        {
            quaternion.Normalize();

            return;
        }

        public Custom_Quaternion normalized
        {
            get
            {
                Custom_Quaternion newQuaternion = new Custom_Quaternion();
                newQuaternion.quaternion = quaternion.normalized;
                return newQuaternion;
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return quaternion.sqrMagnitude;
            }
        }

        // calculate magnitude
        public float magnitude
        {
            get
            {
                return quaternion.magnitude;
            }
        }

        public static Custom_Quaternion identity
        {
            get
            {
                return new Custom_Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
            }
        }

        public Vector3 eulerAngles
        {
            get
            {
                Vector3 axis = quaternion;
                return axis;

            }
            set
            {
                // "Half" angle. Angle = 2 * acos(w)
                float angle = Mathf.Acos(W);
                
                // Normalize new axis.
                value.Normalize();
                
                // Apply rotation to new axis based on current rotation.
                value *= Mathf.Sin(angle);

                // Set the axis values of the current quaternion.
                X = value.x;
                Y = value.y;
                Z = value.z;

                return;
            }
        }


        public UnityEngine.Quaternion GetUnityQuaternion()
        {
            UnityEngine.Quaternion result = new UnityEngine.Quaternion(X, Y, Z, W);

            return result;
        }

        // Multiply two quaternions together.
        public static Custom_Quaternion operator* (Custom_Quaternion leftQuaternion, Custom_Quaternion rightQuaternion)
        {
            Vector3 leftVector = leftQuaternion.quaternion;
            Vector3 rightVector = rightQuaternion.quaternion;
            
            // Calculate the real(W) of the new quaternion.
            float real = leftQuaternion.W * rightQuaternion.W - Vector3.Dot(leftVector, rightVector);

            Vector3 newVector = leftQuaternion.W * rightVector + rightQuaternion.W * leftVector + Vector3.Cross(leftVector, rightVector);

            Custom_Quaternion newQuaternion = new Custom_Quaternion(newVector.x, newVector.y, newVector.z, real);

            return newQuaternion;
        }

        public static Custom_Quaternion operator* (Vector3 leftVector, Custom_Quaternion rightQuaternion)
        {
            // Slides implementation. PG 44
            // v' = v + 2r x (r x v + wv)       v = vector to rotate, r = quaternion vector, w = angle of rotationAxis

            float real = -(Vector3.Dot(leftVector, rightQuaternion.eulerAngles));

            Vector3 newVector = rightQuaternion.W * leftVector + (Vector3.Cross(leftVector, rightQuaternion.eulerAngles));


            Custom_Quaternion quaternion = new Custom_Quaternion(newVector.x, newVector.y, newVector.z, real);

            return quaternion;


            //// The quaternion's current axis of rotation.
            //Vector3 axis = rightQuaternion.eulerAngles;

            //Vector3 axisCross = Vector3.Cross(axis, leftVector);

            //Vector3 doubleAxis = 2.0f * axis;

            //Vector3 axisRotationScalar = rightQuaternion.W * leftVector;

            //Vector3 rotation = leftVector + Vector3.Cross(doubleAxis, axisCross + axisRotationScalar);


            //rightQuaternion.X = rotation.x;
            //rightQuaternion.Y = rotation.y;
            //rightQuaternion.Z = rotation.z;

            //return rightQuaternion;
        }

        public static Custom_Quaternion operator* (Custom_Quaternion left, float right)
        {
            left.X *= right;
            left.Y *= right;
            left.Z *= right;
            left.W *= right;

            return left;
        }

        public static Custom_Quaternion operator+ (Custom_Quaternion left, Custom_Quaternion right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;

            return left;
        }

        public void Rotate(Custom_Quaternion newRotation)
        {
            Custom_Quaternion newQuaternion = newRotation * this;

            quaternion = newQuaternion.quaternion;

            return;
        }

        public Custom_Quaternion Rotate(float angle, Vector3 rotationAxis)
        {
            // Slides implementation.
            // v' = v + 2r x (r x v + wv)       v = vector to rotate, r = quaternion vector, w = angle of rotationAxis

            float rotationAngle = Mathf.Cos(angle * 0.5f);

            // The quaternion's current axis of rotation.
            Vector3 axis = new Vector3(X, Y, Z);

            Custom_Quaternion result = new Custom_Quaternion();

            Vector3 axisCross = Vector3.Cross(axis, rotationAxis);

            Vector3 doubleAxis = 2.0f * axis;

            Vector3 axisRotationScalar = rotationAngle * rotationAxis;

            Vector3 rotation = rotationAxis + Vector3.Cross(doubleAxis, axisCross + axisRotationScalar);


            result.W = 0.0f;
            result.eulerAngles = rotation;

            return result;
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

    // 3D rotation of this object.
    [SerializeField]
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

    private enum ShapeType { SolidSphere, HollowSphere, SolidCube, HollowCube, SolidCylinder, SolidCone };

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
    [SerializeField]
    private Vector3 worldCenterOfMass = Vector3.zero;
    [SerializeField]
    private Vector3 localCenterOfMass = Vector3.zero;

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

    /*******************************
    **  Lab 2. Torque variables.  **
    *******************************/
    [Header("Torque")]

    [SerializeField]
    private Vector3 torque = Vector3.zero;
    [SerializeField]
    private Matrix4x4 inertia = Matrix4x4.identity;
    [SerializeField]
    private Matrix4x4 inertiaInv;
    [SerializeField]
    public Vector3 torqueForce = Vector3.zero;
    [SerializeField]
    public Vector3 momentArm = Vector3.zero;
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
        CalculateWorldInertia();

        CalculateWorldCenterOfMass();
        // Check algorithm type from user selection menu items.
        GetInspectorItems();

        // Update acceleration before setting transforms.
        //UpdateAcceleration();

        // apply torque
        ApplyTorque(momentArm, torqueForce);

        // update angular acceleration
        UpdateAngularAcceleration();
        
        // Apply position to Unity's transform component.
        transform.position = Position;
        // Apply rotation to Unity's transform component.
        //transform.eulerAngles = AngularVelocity;
        
        transform.rotation = Rotation.GetUnityQuaternion();

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

        // Apply inverse inertia tensor based on object shape type.
        switch (shapeType)
        {
            case ShapeType.SolidSphere:
                inertia = Inertia3D.SolidSphereInertiaTensor(transform.localScale.x * 0.5f, mass);
                break;
            case ShapeType.HollowSphere:
                inertia = Inertia3D.HollowSphereInertiaTensor(transform.localScale.x * 0.5f, mass);
                break;
            case ShapeType.SolidCube:
                inertia = Inertia3D.SolidCubeInertiaTensor(transform.localScale.x, transform.localScale.y, transform.localScale.z, mass) ;
                break;
            case ShapeType.HollowCube:
                inertia = Inertia3D.HollowCubeInertiaTensor(transform.localScale.x, transform.localScale.y, transform.localScale.z, mass);
                break;
            case ShapeType.SolidCylinder:
                inertia = Inertia3D.SolidCylinderInertiaTensor(transform.localScale.x * 0.5f, transform.localScale.y, mass);
                break;
            case ShapeType.SolidCone:
                inertia = Inertia3D.SolidConeInertiaTensor(transform.localScale.x * 0.5f, transform.localScale.y, mass);
                break;
            default: break;
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

        // Slide implementation
        //Rotation += (AngularVelocity * Rotation) * deltaTime * 0.5f;
        Rotation = Rotation + (AngularVelocity * Rotation) * deltaTime * 0.5f;
        Rotation.Normalize();

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

    //public Matrix4x4 WorldToLocalMatrix()
    //{
    //    Matrix4x4 model = Matrix4x4.TRS(position, Rotation.GetUnityQuaternion(), transform.localScale);

    //    return model;
    //}

    public Matrix4x4 LocalToWorld()
    {
        // Translation matrix of this particle.
        Matrix4x4 translate = Matrix4x4.Translate(Position);

        // Rotation matrix of this particle.
        Matrix4x4 rotate = Matrix4x4.Rotate(Rotation.GetUnityQuaternion());

        // Model matrix to convert from local to world space.
        Matrix4x4 model = translate * rotate;

        return model;
    }

    public Matrix4x4 WorldTransormationMatrix()
    {
        return Matrix4x4.TRS(position, Rotation.GetUnityQuaternion(), transform.localScale);
    }

    public Matrix4x4 WorldInverseTransormationMatrix()
    {
        return WorldTransormationMatrix().inverse;
    }

    public void CalculateWorldCenterOfMass()
    {
        worldCenterOfMass = LocalToWorld().MultiplyPoint3x4(localCenterOfMass);
    }

    public void CalculateWorldInertia()
    {
        // Calculate the inverse inertia.
        CalculateInverseInertia();

        // Transform the inverse inertia to world space.
        inertiaInv = LocalToWorld() * inertiaInv;

    }

    public void CalculateInverseInertia()
    {
        inertiaInv = inertia.inverse;
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
            UnityEngine.Quaternion quaternion = particle.transform.rotation;
            particle.Rotation = new Particle3D.Custom_Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
            particle.Rotation.Normalize();

            // TODO: Implement quaternion rotation into scene GUI.
            // Get eulerAngles from transform and make Quaternion with w = 1 and x, y, z = eulerAngles???
            //particle.Rotation = particle.transform.rotation.eulerAngles.z;
        }
    }
}

// Quaternion inspector drawer
[CustomPropertyDrawer(typeof(Particle3D.Custom_Quaternion))]
[CanEditMultipleObjects]
public class QuaternionDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        // Using BeginProperty / EndProperty on the parent property means that
        // __prefab__ override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect rect = new Rect(position.x, position.y, 312.0f, position.height);

        Vector4 quaternion = property.FindPropertyRelative("quaternion").vector4Value;

        quaternion = EditorGUI.Vector4Field(rect, GUIContent.none, quaternion);

        //quaternion.Normalize();

        //property.FindPropertyRelative("quaternion").vector4Value = quaternion;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

