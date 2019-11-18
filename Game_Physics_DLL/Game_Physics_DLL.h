#ifndef GAME_PHYSICS_DLL_H
#define GAME_PHYSICS_DLL_H

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else	// !__cplusplus

#endif	// __cplusplus

GAME_PHYSICS_DLL_SYMBOL int InitFoo(int newFoo);
GAME_PHYSICS_DLL_SYMBOL int DoFoo(int bar);
GAME_PHYSICS_DLL_SYMBOL int TermFoo();
GAME_PHYSICS_DLL_SYMBOL float InitForceGenerator(float newForce);
GAME_PHYSICS_DLL_SYMBOL float TermForceGenerator();

GAME_PHYSICS_DLL_SYMBOL float* generateForce_Gravity(float mass, float gravitationalConstant, float worldX, float worldY, float worldZ);
GAME_PHYSICS_DLL_SYMBOL float* generateForce_Normal(float gravityForceX, float gravityForceY, float gravityForceZ, float surfaceNormalX, float surfaceNormalY, float surfaceNormalZ);
GAME_PHYSICS_DLL_SYMBOL float* generateForce_Static_Friction(float forceNormalX, float forceNormalY, float forceNormalZ, float forceOpposingX, float forceOpposingY, float forceOpposingZ, float staticCoeff);
GAME_PHYSICS_DLL_SYMBOL float* generateForce_Kinetic_Friction(float forceNormalX, float forceNormalY, float forceNormalZ, float pVelocityX, float pVelocityY, float pVelocityZ, float kineticCoeff);
GAME_PHYSICS_DLL_SYMBOL float* generateForce_Drag();
GAME_PHYSICS_DLL_SYMBOL float* generateForce_Spring();

#ifdef __cplusplus
}
#else	// !__cplusplus

#endif	// __cplusplus


#endif	// !GAME_PHYSICS_DLL_H
