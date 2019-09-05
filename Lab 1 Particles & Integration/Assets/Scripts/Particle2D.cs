using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    /*
     *  Step 1
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


    private void Awake()
    {
        SetInitailVelocity();
    }
    // Start is called before the first frame update
    void Start()
    {

        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check user selection from menu items.
        GetInspectorItems();

        //AngularVelocityScalar(-Mathf.Sin(50f) * 2f);

        // Apply to transform.
        transform.position = position;

        transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);

        /*
         *  Step 4
         */

        // Test
        // Note: Set the initial velocity to integral value
        acceleration.x = -Mathf.Sin(Time.fixedTime);
        angularAcceleration = -Mathf.Sin(Time.fixedTime) * 360f;

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
         *  Step 3
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
     *  Step 2
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
}
