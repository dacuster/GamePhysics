#include "Vector3D.h"
#include "Game_Physics_DLL.h"

// Create a new Vector3D with the given values.
Vector3D* CreateVector3D(float x, float y, float z)
{
	return new Vector3D(x, y, z);
}

// Set the given coordinates to the coordinates of the given Vector3D.
void GetVector3(Vector3D* pVector, float& x, float& y, float& z)
{
	pVector->getCoordinates(x, y, z);

	return;
}

// Destroy the Vector3D.
void DestroyVector3D(Vector3D* pVector)
{
	if (pVector != nullptr)
	{
		delete pVector;
		pVector = nullptr;
	}

	return;
}

// f = mg
// Calculate a gravity force.
Vector3D* ForceGenerator::Gravity(float mass, float gravitationalConstant, Vector3D* worldUp)
{
	// Create a new force to return.
	Vector3D* pForce = new Vector3D;

	// Calculate the gravitational force.
	*pForce = (*worldUp) * mass * gravitationalConstant;

	// Return the calculated force.
	return pForce;
}

// f_normal = proj(f_gravity, surfaceNormal_unit)
// proj = (norm(x) * grav(x) + norm(y) * grav(y)) / grav(x)^2 + grav(y)^2
// finalProj = proj * grav
// Calculate a normal force.
Vector3D* ForceGenerator::Normal(Vector3D* gravity, Vector3D* surfaceNormal)
{

	// Get the projection of the surface normal onto gravity.
	float projection = gravity->dot(surfaceNormal) / (gravity->magnitudeSquared());

	// Create a vector to return.
	Vector3D* pForce = new Vector3D();
	
	// Apply the projection onto the force of gravity.
	*pForce = (*gravity) * projection;

	// Return the calculated force.
	return pForce;
}

// f_friction_s = -f_opposing if less than max, else -coefficient * f_normal (max amount is coefficient * f_normal.magnitude)
// Calculate a static friction force.
Vector3D* ForceGenerator::StaticFriction(Vector3D* normalForce, Vector3D* opposingForce, float staticCoefficient)
{

	// Maximum force needed to oppose the friction.
	float max = normalForce->magnitude() * staticCoefficient;

	// Create a new force to return.
	Vector3D* pForce = new Vector3D();

	// Opposing force isn't enough to overcome the friction.
	if (opposingForce->magnitude() < max)
	{
		*pForce = -(*opposingForce);
	}
	// Opposing force can overcome the friction.
	else
	{
		*pForce = (*normalForce)  * (-staticCoefficient);
	}

	// Return the calculated force.
	return pForce;
}

// f_friction_k = -coefficient * f_normal.magnitude * pVelocity.normalize
// Calculate a kinetic friction force.
Vector3D* ForceGenerator::KineticFriction(Vector3D* normalForce, Vector3D* particleVelocity, float kineticCoefficient)
{
	// Create a force to return.
	Vector3D* pForce = new Vector3D();

	// Calculate kinetic friction.
	*pForce = particleVelocity->normalized() * (-kineticCoefficient * normalForce->magnitude());

	// Return the calculated force.
	return pForce;
}

// Calculate a drag force.
Vector3D* ForceGenerator::Drag()
{
	return nullptr;
}

// Calculate a spring force.
Vector3D* ForceGenerator::Spring()
{
	return nullptr;
}