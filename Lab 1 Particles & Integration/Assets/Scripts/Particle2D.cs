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
    private Vector2 position;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private Vector2 acceleration;
    [SerializeField]
    private float rotation;
    [SerializeField]
    private float angularVelocity;

    // Start is called before the first frame update
    void Start()
    {

        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
         *  Step 3
         */

        // Integrate.
        //UpdatePositionEulerExplicit(Time.fixedDeltaTime);
        UpdatePositionKinematic(Time.fixedDeltaTime);

        // Apply to transform.
        transform.position = position;

        /*
         *  Step 4
         */

        // Test
        acceleration.x = -Mathf.Sin(Time.fixedTime);

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

        return;
    }

    private void UpdateRotationKinematic(float _deltaTime)
    {

        return;
    }
}
