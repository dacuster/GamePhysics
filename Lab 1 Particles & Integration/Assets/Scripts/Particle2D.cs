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

    // Setup enums for menu options.
    private enum PositionType { Euler, Kinematic };
    private enum RotationType { Euler, Kinematic };

    // Initialize default menu option types.
    [SerializeField]
    private PositionType positionType = PositionType.Euler;
    [SerializeField]
    private RotationType rotationType = RotationType.Euler;

    // Update is called once per frame.
    void FixedUpdate()
    {
        // Check user selection from menu items.
        GetInspectorItems();

        // Apply to transform.
        transform.position = position;
        transform.eulerAngles = new Vector3(0.0f, 0.0f, rotation);

        /*
         *  Lab 1 Step 4 Test
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
         *  Lab 1 Step 3 Integrate.
         */
        // Oscillate the particle back and forth.
        if (positionType == PositionType.Euler)
        {
            UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        }
        else if (positionType == PositionType.Kinematic)
        {
            UpdatePositionKinematic(Time.fixedDeltaTime);
        }

        // Rotate the particle back and forth.
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
     // Update position using euler's method. (Better used for programming.)
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

    // Update position using the kinematic algorithm. (Actual movement algorithm.)
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

    // Update the rotation using Euler's method. (Best used for programming.)
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

    // Update rotation using the kinematic algorithm. (Actual rotation algorithm.)
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
}