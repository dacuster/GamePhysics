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
}

namespace ForceGeneratorDLL
{
    public class GenerateForce
    {
        public static Vector3 Gravity(float mass, float gravitationalConstant, Vector3 worldUp)
        {
            return ConvertToVector3(Game_Physics_DLL.Gravity(mass, gravitationalConstant, ConvertToVector3D(worldUp)));
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

        // Convert a Unity Vector3 struct to a Vector3D C++ class.
        private static Vector3D ConvertToVector3D(Vector3 vector)
        {
            return Game_Physics_DLL.CreateVector3D(vector.x, vector.y, vector.z);
        }

        // Convert the Vector3D C++ class to a Unity Vector3 struct.
        private static Vector3 ConvertToVector3(Vector3D vector)
        {
            Vector3 vector3 = Vector3.zero;

            Game_Physics_DLL.GetVector3(vector, ref vector3.x, ref vector3.y, ref vector3.z);

            Game_Physics_DLL.DestroyVector3D(vector);

            return vector3;
        }
    }
}