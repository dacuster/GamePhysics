#ifndef GAME_PHYSICS_DLL_H
#define GAME_PHYSICS_DLL_H

// Marshaling C++ classes https://www.youtube.com/watch?v=w3jGgTHJoCY

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else	// !__cplusplus

#endif	// __cplusplus

// Create a Vector3D.
GAME_PHYSICS_DLL_SYMBOL Vector3D* CreateVector3D(float x, float y, float z);

// Set referenced parameters of Vector3D.
GAME_PHYSICS_DLL_SYMBOL void GetVector3(Vector3D* pVector, float& x, float& y, float& z);

// Destroy the Vector3D.
GAME_PHYSICS_DLL_SYMBOL void DestroyVector3D(Vector3D* pVector);

namespace ForceIntegration
{
	GAME_PHYSICS_DLL_SYMBOL void UpdatePositionEulerExplicit(Vector3D* pPosition, Vector3D* pVelocity, Vector3D* pAcceleration, float deltaTime);
}

namespace ForceGenerator
{
	// Calculate a gravity force.
	GAME_PHYSICS_DLL_SYMBOL Vector3D* Gravity(float mass, float gravitationalConstant, Vector3D* worldUp);

	// Calculate a normal force.
	GAME_PHYSICS_DLL_SYMBOL Vector3D* Normal(Vector3D* gravity, Vector3D* surfaceNormal);

	// Calculate a static friction force.
	GAME_PHYSICS_DLL_SYMBOL Vector3D* StaticFriction(Vector3D* normalForce, Vector3D* opposingForce, float staticCoefficient);

	// Calculate a kinetic friction force.
	GAME_PHYSICS_DLL_SYMBOL Vector3D* KineticFriction(Vector3D* normalForce, Vector3D* particleVelocity, float kineticCoefficient);

	// Calculate a drag force.
	GAME_PHYSICS_DLL_SYMBOL Vector3D* Drag(Vector3D* particleVelocity, Vector3D* fluidVelocity, float fluidDensity, float areaCrossSection, float dragCoefficient);

	// Calculate a spring force.
	GAME_PHYSICS_DLL_SYMBOL Vector3D* Spring(Vector3D* particlePosition, Vector3D* anchorPosition, float restingLength, float stiffnessCoefficient);
}

#ifdef __cplusplus
}
#else	// !__cplusplus

#endif	// __cplusplus

#endif	// !GAME_PHYSICS_DLL_H