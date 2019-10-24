using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator
{
    public static Vector3 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector3 worldUp)
    {
        // f = mg
        return (particleMass * gravitationalConstant * worldUp);
    }

    public static Vector3 GenerateForce_Normal(Vector3 forceGravity, Vector3 surfaceNormalUnit)
    {
        // f_normal = proj(f_gravity, surfaceNormal_unit)
        // proj = (norm(x) * grav(x) + norm(y) * grav(y)) / grav(x)^2 + grav(y)^2
        // finalProj = proj * grav

        // Calculate the projection of surface normal onto gravity.
        float projection = (surfaceNormalUnit.x * forceGravity.x + surfaceNormalUnit.y * forceGravity.y) / (forceGravity.x * forceGravity.x + forceGravity.y * forceGravity.y);

        // Apply projection onto gravity.
        Vector3 force = projection * forceGravity;

        return force;
    }

    public static Vector3 GenerateForce_Sliding(Vector3 forceGravity, Vector3 forceNormal)
    {
        // f_sliding = f_gravity + f_normal
        return (forceGravity + forceNormal);
    }

    public static Vector3 GenerateForce_Friction_Static(Vector3 forceNormal, Vector3 forceOpposing, float frictionCoefficientStatic)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)

        float max = frictionCoefficientStatic * forceNormal.magnitude;

        Vector3 force = frictionCoefficientStatic * forceNormal;

        if (forceOpposing.magnitude > max)
        {
            force -= forceOpposing;
        }
        else
        {
            force = -frictionCoefficientStatic * forceNormal;
        }

        return force;
    }

    public static Vector3 GenerateForce_Friction_Kinetic(Vector3 forceNormal, Vector3 particleVelocity, float frictionCoefficientKinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)

        Vector3 force = -frictionCoefficientKinetic * forceNormal.magnitude * particleVelocity;

        return force;
    }

    public static Vector3 GenerateForce_Drag(Vector3 particleVelocity, Vector3 fluidVelocity, float fluidDensity, float objectAreaCrossSection, float objectDragCoefficient)
    {
        // f_drag = (p * u^2 * area * coeff)/2

        Vector3 force = (particleVelocity - fluidVelocity) * (fluidDensity * particleVelocity.magnitude * particleVelocity.magnitude * objectAreaCrossSection * objectDragCoefficient * 0.5f);

        return force;
    }

    public static Vector3 GenerateForce_Spring(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // Page 107
        // f_spring = -coeff*(spring length - spring resting length)

        // Calculate relative position of the particle to the anchor.
        Vector3 position = particlePosition - anchorPosition;

        // Generate the force.
        float force = -springStiffnessCoefficient * (position.magnitude - springRestingLength);

        // Return the force at the current position.
        return position * force;
    }
    public static Vector3 GenerateForce_Spring_Damping(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient, float springDamping, float springConstant, Vector3 particleVelocity)
    {
        // Page 107
        // f_spring = -coeff*(spring length - spring resting length)

        // Calculate relative position of the particle to the anchor.
        Vector3 position = particlePosition - anchorPosition;

        // Calculate the damping
        float gamma = 0.5f * Mathf.Sqrt(4 * springConstant - springDamping * springDamping);

        if (gamma == 0.0f)
        {
            return Vector3.zero;
        }

        Vector3 c = position * (springDamping / (2.0f * gamma)) + particleVelocity * (1.0f / gamma);

        Vector3 target = position * Mathf.Cos(gamma * Time.fixedDeltaTime) + c * Mathf.Sin(gamma * Time.fixedDeltaTime);

        target *= Mathf.Exp(-0.5f * Time.fixedDeltaTime * springDamping);

        // Generate the force.
        Vector3 force = (target - position) * (1.0f / Time.fixedDeltaTime * Time.fixedDeltaTime) - particleVelocity * Time.fixedDeltaTime;

        // Return the force at the current position.
        return force;
    }
}
