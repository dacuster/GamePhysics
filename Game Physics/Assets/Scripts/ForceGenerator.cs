using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ForceGenerator
{
    public static unsafe Vector3 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector3 worldUp)
    {
        // f = mg
        // TODO: Create DLL
        // "init function" void generateForceGravity(float mass, float constant, float worldX, float worldY, float worldZ);

        // FUNCTION CALLS
        // ForceGeneratorDLL.generateForceGravity(float mass, float constant, float worldX, float worldY, float worldZ);
        // Vector3 force = new Vector3(ForceGeneratorDLL.getX(), ForceGeneratorDLL.getY(), ForceGeneratorDLL.getZ());
        // return force;

        float* gPtr = Game_Physics_DLL.generateForce_Gravity(particleMass, gravitationalConstant, worldUp.x, worldUp.y, worldUp.z);
        
        // DLL integration:
        float x = *gPtr;
        float y = *(gPtr + 1);
        float z = *(gPtr + 2);

        Vector3 gravity = new Vector3(x,y,z);
        return gravity;
        //return (particleMass * gravitationalConstant * worldUp);
    }

    public static unsafe Vector3 GenerateForce_Normal(Vector3 forceGravity, Vector3 surfaceNormalUnit)
    {
        // f_normal = proj(f_gravity, surfaceNormal_unit)
        // proj = (norm(x) * grav(x) + norm(y) * grav(y)) / grav(x)^2 + grav(y)^2
        // finalProj = proj * grav

        float* nPtr = Game_Physics_DLL.generateForce_Normal(forceGravity.x, forceGravity.y, forceGravity.z, surfaceNormalUnit.x, surfaceNormalUnit.y, surfaceNormalUnit.z); 
        // Calculate the projection of surface normal onto gravity.
        //float projection = (surfaceNormalUnit.x * forceGravity.x + surfaceNormalUnit.y * forceGravity.y) / (forceGravity.x * forceGravity.x + forceGravity.y * forceGravity.y);

        float x = *nPtr;
        float y = *(nPtr + 1);
        float z = *(nPtr + 2);
       
        // Apply projection onto gravity.
        Vector3 force = new Vector3 (x,y,z);
        Debug.Log(force);
        return force;
    }

    public static  Vector3 GenerateForce_Sliding(Vector3 forceGravity, Vector3 forceNormal)
    {
        // f_sliding = f_gravity + f_normal
        return (forceGravity + forceNormal);
    }

    public static unsafe Vector3 GenerateForce_Friction_Static(Vector3 forceNormal, Vector3 forceOpposing, float frictionCoefficientStatic)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)

        float* sfPtr = Game_Physics_DLL.generateForce_Static_Friction(forceNormal.x, forceNormal.y, forceNormal.z, forceOpposing.x, forceOpposing.y, forceOpposing.z, frictionCoefficientStatic);

        float x = *sfPtr;
        float y = *(sfPtr + 1);
        float z = *(sfPtr + 2);
        //float max = frictionCoefficientStatic * forceNormal.magnitude;

        //Vector3 force = frictionCoefficientStatic * forceNormal;

        Vector3 force = new Vector3(x, y, z);
        //if (forceOpposing.magnitude > max)
        //{
        //    force -= forceOpposing;
        //}
        //else
        //{
        //    force = -frictionCoefficientStatic * forceNormal;
        //}

        return force;
    }

    public static unsafe Vector3 GenerateForce_Friction_Kinetic(Vector3 forceNormal, Vector3 particleVelocity, float frictionCoefficientKinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)
        float* kfPtr = Game_Physics_DLL.generateForce_Kinetic_Friction(forceNormal.x, forceNormal.y, forceNormal.z, particleVelocity.x, particleVelocity.y, particleVelocity.z, frictionCoefficientKinetic);

        float x = *kfPtr;
        float y = *(kfPtr + 1);
        float z = *(kfPtr + 2);
        //Vector3 force = -frictionCoefficientKinetic * forceNormal.magnitude * particleVelocity;

        Vector3 force = new Vector3(x, y, z);
        return force;
    }

    public static unsafe Vector3 GenerateForce_Drag(Vector3 particleVelocity, Vector3 fluidVelocity, float fluidDensity, float objectAreaCrossSection, float objectDragCoefficient)
    {
        // f_drag = (p * u^2 * area * coeff)/2

        Vector3 force = (particleVelocity - fluidVelocity) * (fluidDensity * particleVelocity.magnitude * particleVelocity.magnitude * objectAreaCrossSection * objectDragCoefficient * 0.5f);

        return force;
    }

    public static unsafe Vector3 GenerateForce_Spring(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
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
