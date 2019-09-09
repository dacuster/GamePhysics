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
        return Vector2.zero;
    }

    public static Vector2 GenerateForce_Sliding(Vector2 _forceGravity, Vector2 _forceNormal)
    {
        // f_sliding = f_gravity + f_normal
        return (_forceGravity + _forceNormal);
    }

    public static Vector2 GenerateForce_Friction_Static(Vector2 _forceNormal, Vector2 _forceOpposing, float _frictionCoefficient_Static)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)

        return Vector2.zero;
    }

    public static Vector2 GenerateForce_friction_kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)

        return Vector2.zero;
    }

    public static Vector2 GenerateForce_drag(Vector2 particleVelocity, Vector2 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // f_drag = (p * u^2 * area * coeff)/2

        return Vector2.zero;
    }

    public static Vector2 GenerateForce_spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // f_spring = -coeff*(spring length - spring resting length)

        return Vector2.zero;
    }
}
