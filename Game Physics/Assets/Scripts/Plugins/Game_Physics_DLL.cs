using System;
using System.Runtime.InteropServices;

// Create struct to implement "typedef" for custom C++ pointer as System.IntPtr
public struct CppClass
{
    private IntPtr Value;

    public static implicit operator IntPtr(CppClass _cppClass)
    {
        return ((_cppClass == IntPtr.Zero) ? IntPtr.Zero : _cppClass.Value);
    }

    public static implicit operator CppClass(IntPtr _intPtr)
    {
        return new CppClass { Value = _intPtr } ;
    }
}

// ForceGeneratorDLL
public class Game_Physics_DLL
{
    [DllImport("Game_Physics_DLL")]
    public static extern int InitFoo(int newFoo = 0);

    [DllImport("Game_Physics_DLL")]
    public static extern int DoFoo(int bar = 0);

    [DllImport("Game_Physics_DLL")]
    public static extern int TermFoo();

    [DllImport("Game_Physics_DLL")]
    public static extern CppClass CreateCppClass(int newInt);

    [DllImport("Game_Physics_DLL")]
    public static extern void DeleteCppClass(CppClass pObject);

    [DllImport("Game_Physics_DLL")]
    public static extern int CppAdd(CppClass pObject, int newInt);

    [DllImport("Game_Physics_DLL")]
    public static unsafe extern float* generateForce_Gravity(float mass, float gravitationalConstant, float worldX, float worldY, float worldZ);

    [DllImport("Game_Physics_DLL")]
    public static unsafe extern float* generateForce_Normal(float gravityForceX, float gravityForceY, float gravityForceZ, float surfaceNormalX, float surfaceNormalY, float surfaceNormalZ);

    [DllImport("Game_Physics_DLL")]
    public static unsafe extern float* generateForce_Drag();

    [DllImport("Game_Physics_DLL")]
    public static unsafe extern float* generateForce_Static_Friction(float forceNormalX, float forceNormalY, float forceNormalZ, float forceOpposingX, float forceOpposingY, float forceOpposingZ, float staticCoeff);

    [DllImport("Game_Physics_DLL")]
    public static unsafe extern float* generateForce_Kinetic_Friction(float forceNormalX, float forceNormalY, float forceNormalZ, float pVelocityX, float pVelocityY, float pVelocityZ, float kineticCoeff);

    [DllImport("Game_Physics_DLL")]
    public static unsafe extern float* generateForce_Spring();
}