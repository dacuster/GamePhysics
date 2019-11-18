using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

using System.Runtime.InteropServices;

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
