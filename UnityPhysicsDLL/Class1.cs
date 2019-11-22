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
    using static DllConversion;
    public class PhysicsIntegration
    {
        public static void UpdatePositionEulerExplicit(ref Vector3 position, ref Vector3 velocity, Vector3 acceleration, float deltaTime)
        {

            Vector3D pPosition = ConvertToVector3D(position);
            Vector3D pVelocity = ConvertToVector3D(velocity);

            Game_Physics_DLL.UpdatePositionEulerExplicit(pPosition, pVelocity, ConvertToVector3D(acceleration), deltaTime);

            position = ConvertToVector3(pPosition);
            velocity = ConvertToVector3(pVelocity);

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
            return ConvertToVector3(Game_Physics_DLL.Normal(ConvertToVector3D(gravity), ConvertToVector3D(surfaceNormal)));
        }

        public static Vector3 StaticFriction(Vector3 normalForce, Vector3 opposingForce, float staticCoefficient)
        {
            return ConvertToVector3(Game_Physics_DLL.StaticFriction(ConvertToVector3D(normalForce), ConvertToVector3D(opposingForce), staticCoefficient));
        }

        public static Vector3 KineticFriction(Vector3 normalForce, Vector3 particleVelocity, float kineticCoefficient)
        {
            return ConvertToVector3(Game_Physics_DLL.KineticFriction(ConvertToVector3D(normalForce), ConvertToVector3D(particleVelocity), kineticCoefficient));
        }
    }
}