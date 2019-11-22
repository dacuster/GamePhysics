using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.InteropServices;

using Vector3D = System.IntPtr;

public class Game_Physics_DLL
{
    [DllImport("Game_Physics_DLL")]
    public static extern Vector3D CreateVector3D(float x, float y, float z);

    [DllImport("Game_Physics_DLL")]
    public static extern void GetVector3(Vector3D vector, ref float x, ref float y, ref float z);

    [DllImport("Game_Physics_DLL")]
    public static extern void DestroyVector3D(Vector3D vector);

    [DllImport("Game_Physics_DLL")]
    public static extern Vector3D Gravity(float mass, float gravitationalConstant, Vector3D worldUp);

    [DllImport("Game_Physics_DLL")]
    public static extern Vector3D Normal(Vector3D gravity, Vector3D surfaceNormal);

    [DllImport("Game_Physics_DLL")]
    public static extern Vector3D StaticFriction(Vector3D normalForce, Vector3D opposingForce, float staticCoefficient);

    [DllImport("Game_Physics_DLL")]
    public static extern Vector3D KineticFriction(Vector3D normalForce, Vector3D particleVelocity, float kineticCoefficient);

    [DllImport("Game_Physics_DLL")]
    public static extern Vector3D Drag(Vector3D particleVelocity, Vector3D fluidVelocity, float fluidDensity, float areaCrossSection, float dragCoefficient);

    [DllImport("Game_Physics_DLL")]
    public static extern Vector3D Spring(Vector3D particlePosition, Vector3D anchorPosition, float restingLength, float stiffnessCoefficient);

    [DllImport("Game_Physics_DLL")]
    public static extern void UpdatePositionEulerExplicit(Vector3D position, Vector3D velocity, Vector3D acceleration, float deltaTime);
}

public class DllConversion
{

    // Convert a Unity Vector3 struct to a Vector3D C++ class.
    public static Vector3D ConvertToVector3D(Vector3 vector)
    {
        return Game_Physics_DLL.CreateVector3D(vector.x, vector.y, vector.z);
    }

    // Convert the Vector3D C++ class to a Unity Vector3 struct.
    public static Vector3 ConvertToVector3(Vector3D vector)
    {
        Vector3 vector3 = Vector3.zero;

        Game_Physics_DLL.GetVector3(vector, ref vector3.x, ref vector3.y, ref vector3.z);

        Game_Physics_DLL.DestroyVector3D(vector);

        return vector3;
    }
}

namespace PhysicsIntegrationDLL
{
    // Using static will allow class integration into namespaces.
    using static DllConversion;
    public class PhysicsIntegration
    {
        public static void UpdatePositionEulerExplicit(ref Vector3 position, ref Vector3 velocity, Vector3 acceleration, float deltaTime)
        {
            // Convert position to a Vector3D pointer.
            Vector3D pPosition = ConvertToVector3D(position);

            // Convert velocity to a Vector3D pointer.
            Vector3D pVelocity = ConvertToVector3D(velocity);

            // Convert acceleration to a Vector3D pointer.
            Vector3D pAcceleration = ConvertToVector3D(acceleration);

            // Calculate the position integration in the DLL.
            Game_Physics_DLL.UpdatePositionEulerExplicit(pPosition, pVelocity, pAcceleration, deltaTime);

            // Assign the calculated value back to position.
            position = ConvertToVector3(pPosition);

            // Assign the calculated value back to velocity.
            velocity = ConvertToVector3(pVelocity);

            // Prevent memory leaks of the acceleration Vector3D.
            Game_Physics_DLL.DestroyVector3D(pAcceleration);

            return;
        }
    }
}

namespace ForceGeneratorDLL
{
    // Using static will allow class integration into namespaces.
    using static DllConversion;

    public class GenerateForce
    {
        public static Vector3 Gravity(float mass, float gravitationalConstant, Vector3 worldUp)
        {
            // Convert worldUp to a Vector3D pointer.
            Vector3D pWorldUp = ConvertToVector3D(worldUp);

            // Calculate the force in the DLL and convert it to a Unity Vector3.
            Vector3 force = ConvertToVector3(Game_Physics_DLL.Gravity(mass, gravitationalConstant, pWorldUp));

            // Prevent memory leaks of the worldUp Vector3D.
            Game_Physics_DLL.DestroyVector3D(pWorldUp);

            // Return the calculated force.
            return force;
        }
        public static Vector3 Normal(Vector3 gravity, Vector3 surfaceNormal)
        {
            // Convert gravity to Vector3D pointer.
            Vector3D pGravity = ConvertToVector3D(gravity);

            // Convert surfaceNormal to Vector3D pointer.
            Vector3D pSurafaceNormal = ConvertToVector3D(surfaceNormal);

            // Calculate the force in the DLL and convert it to a Unity Vector3.
            Vector3 force = ConvertToVector3(Game_Physics_DLL.Normal(pGravity, pSurafaceNormal));

            // Prevent memory leaks of the gravity Vector3D.
            Game_Physics_DLL.DestroyVector3D(pGravity);

            // Prevent memory leaks of the surfaceNormal Vector3D.
            Game_Physics_DLL.DestroyVector3D(pSurafaceNormal);

            // Return the calculated force.
            return force;
        }

        public static Vector3 StaticFriction(Vector3 normalForce, Vector3 opposingForce, float staticCoefficient)
        {
            // Convert normalForce to Vector3D pointer.
            Vector3D pNormalForce = ConvertToVector3D(normalForce);

            // Convert opposingForce to Vector3D pointer.
            Vector3D pOpposingForce = ConvertToVector3D(opposingForce);

            // Calculate the force in the DLL and convert it to a Unity Vector3.
            Vector3 force = ConvertToVector3(Game_Physics_DLL.StaticFriction(pNormalForce, pOpposingForce, staticCoefficient));

            // Prevent memory leaks of the normalForce Vector3D.
            Game_Physics_DLL.DestroyVector3D(pNormalForce);

            // Prevent memory leaks of the opposingForce Vector3D.
            Game_Physics_DLL.DestroyVector3D(pOpposingForce);

            // Return the calculated force.
            return force;
        }

        public static Vector3 KineticFriction(Vector3 normalForce, Vector3 particleVelocity, float kineticCoefficient)
        {
            // Convert normalForce to Vector3D pointer.
            Vector3D pNormalForce = ConvertToVector3D(normalForce);

            // Convert particleVelocity to Vector3D pointer.
            Vector3D pParticleVelocity = ConvertToVector3D(particleVelocity);

            // Calculate the force in the DLL and convert it to a Unity Vector3.
            Vector3 force = ConvertToVector3(Game_Physics_DLL.KineticFriction(pNormalForce, pParticleVelocity, kineticCoefficient));

            // Prevent memory leaks of the normalForce Vector3D.
            Game_Physics_DLL.DestroyVector3D(pNormalForce);

            // Prevent memory leaks of the particleVelocity Vector3D.
            Game_Physics_DLL.DestroyVector3D(pParticleVelocity);

            // Return the calculated force.
            return force;
        }

        public static Vector3 Spring(Vector3 particlePosition, Vector3 anchorPosition, float restingLength, float stiffnessCoefficient)
        {
            // Convert particlePosition to Vector3D pointer.
            Vector3D pParticlePosition = ConvertToVector3D(particlePosition);

            // Convert anchorPosition to Vector3D pointer.
            Vector3D pAnchorPosition = ConvertToVector3D(anchorPosition);

            // Calculate the force in the DLL and convert it to a Unity Vector3.
            Vector3 force = ConvertToVector3(Game_Physics_DLL.Spring(pParticlePosition, pAnchorPosition, restingLength, stiffnessCoefficient));

            // Prevent memory leaks of the particlePosition Vector3D.
            Game_Physics_DLL.DestroyVector3D(pParticlePosition);

            // Prevent memory leaks of the anchorPosition Vector3D.
            Game_Physics_DLL.DestroyVector3D(pAnchorPosition);

            // Return the calculated force.
            return force;
        }

        public static Vector3 Drag(Vector3 particleVelocity, Vector3 fluidVelocity, float fluidDensity, float areaCrossSection, float dragCoefficient)
        {
            // Convert particleVelocity to Vector3D pointer.
            Vector3D pParticleVelocity = ConvertToVector3D(particleVelocity);

            // Convert particleVelocity to Vector3D pointer.
            Vector3D pFluidVelocity = ConvertToVector3D(fluidVelocity);

            // Calculate the force in the DLL and convert it to a Unity Vector3.
            Vector3 force = ConvertToVector3(Game_Physics_DLL.Drag(pParticleVelocity, pFluidVelocity, fluidDensity, areaCrossSection, dragCoefficient));

            // Prevent memory leaks of the particleVelocity Vector3D.
            Game_Physics_DLL.DestroyVector3D(pParticleVelocity);

            // Prevent memory leaks of the particleVelocity Vector3D.
            Game_Physics_DLL.DestroyVector3D(pFluidVelocity);

            // Return the calculated force.
            return force;
        }
    }
}