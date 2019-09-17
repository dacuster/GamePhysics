using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator
{
    public static Vector2 GenerateForce_Gravity(float _particleMass, float _gravitationalConstant, Vector2 _worldUp)
    {
        // f = mg
        return (_particleMass * _gravitationalConstant * _worldUp);
    }

    public static Vector2 GenerateForce_Normal(Vector2 _forceGravity, Vector2 _surfaceNormal_unit)
    {
        // f_normal = proj(f_gravity, surfaceNormal_unit)
        // TODO: figure out projection matrix
        // proj = (norm(x) * grav(x) + norm(y) * grav(y)) / grav(x)^2 + grav(y)^2
        // finalProj = proj * grav

        // Calculate the projection of surface normal onto gravity.
        float projection = (_surfaceNormal_unit.x * _forceGravity.x + _surfaceNormal_unit.y * _forceGravity.y) / (_forceGravity.x * _forceGravity.x + _forceGravity.y * _forceGravity.y);
        
        // Apply projection onto gravity.
        Vector2 force = projection * _forceGravity;

        return force;
    }

    public static Vector2 GenerateForce_Sliding(Vector2 _forceGravity, Vector2 _forceNormal)
    {
        // f_sliding = f_gravity + f_normal
        return (_forceGravity + _forceNormal);
    }

    public static Vector2 GenerateForce_Friction_Static(Vector2 _forceNormal, Vector2 _forceOpposing, float _frictionCoefficient_Static)
    {
        // Page 152?
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)

        return Vector2.zero;
    }

    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // Page 152?
        // f_friction_k = -coeff*|f_normal| * unit(vel)

        return Vector2.zero;
    }

    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // Page 85
        // f_drag = (p * u^2 * area * coeff)/2

        return Vector2.zero;
    }

    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // Page 107
        // f_spring = -coeff*(spring length - spring resting length)

        // Calculate relative position of the particle to the anchor.
        Vector2 position = particlePosition - anchorPosition;

        // Generate the force.
        float force = -springStiffnessCoefficient * (position.magnitude - springRestingLength);

        // Return the force at the current position.
        return position * force;
    }
}
