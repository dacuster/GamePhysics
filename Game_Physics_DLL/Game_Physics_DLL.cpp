#include "Game_Physics_DLL.h"
#include <math.h>
#include "Foo.h"
// Turn into singleton if wanted.
Foo* instance = 0;
int InitFoo(int newFoo)
{
	if (!instance)
	{
		instance = new Foo(newFoo);

		return 1;
	}
	
	return 0;
}

int DoFoo(int bar)
{
	if (instance)
	{
		int result = instance->foo(bar);
		
		return result;
	}
	return 0;
}

int TermFoo()
{
	if (instance)
	{
		delete instance;
		instance = 0;
		return 1;
	}

	return 0;
}


float* generateForce_Gravity(float mass, float gravitationalConstant, float worldX, float worldY, float worldZ)
{
	float gArray[3];
	gArray[0] = (mass * gravitationalConstant) * worldX;
	gArray[1] = (mass * gravitationalConstant) * worldY;
	gArray[2] = (mass * gravitationalConstant) * worldZ;

	return gArray;
}

float* generateForce_Normal(float forceGravityX, float forceGravityY, float forceGravityZ, float surfaceNormalX, float surfaceNormalY, float surfaceNormalZ)
{
	float nArray[3];
	float projection = (surfaceNormalX * forceGravityX + surfaceNormalY * forceGravityY + surfaceNormalZ * forceGravityZ) /
					   (forceGravityX * forceGravityX + forceGravityY * forceGravityY + forceGravityZ * forceGravityZ);

	nArray[0] = projection * forceGravityX;
	nArray[1] = projection * forceGravityY;
	nArray[2] = projection * forceGravityZ;

	return nArray;
}

float* generateForce_Static_Friction(float forceNormalX, float forceNormalY, float forceNormalZ, float forceOpposingX, float forceOpposingY, float forceOpposingZ, float staticCoeff)
{
	float forceNormalMagnitude = sqrt(forceNormalX * forceNormalX + forceNormalY * forceNormalY + forceNormalZ * forceNormalZ);
	float forceOpposingMagnitude = sqrt(forceOpposingX * forceOpposingX + forceOpposingY * forceOpposingY + forceOpposingZ * forceOpposingZ);
	float forceOpposingNormalizedX = forceNormalX / forceNormalMagnitude;
	float forceOpposingNormalizedY = forceNormalY / forceNormalMagnitude;
	float forceOpposingNormalizedZ = forceNormalZ / forceNormalMagnitude;

	float sfArray[3];

	float max = forceNormalMagnitude * staticCoeff;

	if (forceOpposingMagnitude < max)
	{
		sfArray[0] = -forceOpposingX;
		sfArray[1] = -forceOpposingY;
		sfArray[2] = -forceOpposingZ;
	}

	else
	{
		sfArray[0] = -max * forceOpposingNormalizedX;
		sfArray[1] = -max * forceOpposingNormalizedY;
		sfArray[2] = -max * forceOpposingNormalizedZ;
	}


	return sfArray;
}

float* generateForce_Kinetic_Friction(float forceNormalX, float forceNormalY, float forceNormalZ, float pVelocityX, float pVelocityY, float pVelocityZ, float kineticCoeff)
{
	float forceNormalMagnitude = sqrt(forceNormalX * forceNormalX + forceNormalY * forceNormalY + forceNormalZ * forceNormalZ);
	
	float pvelocityNormalizedX = forceNormalX / forceNormalMagnitude;
	float pvelocityNormalizedY = forceNormalY / forceNormalMagnitude;
	float pvelocityNormalizedZ = forceNormalZ / forceNormalMagnitude;

	float kfArray[3];

	kfArray[0] = fabs(forceNormalX);
	kfArray[1] = fabs(forceNormalY);
	kfArray[2] = fabs(forceNormalZ);

	kfArray[0] *= kineticCoeff;
	kfArray[1] *= kineticCoeff;
	kfArray[2] *= kineticCoeff;

	kfArray[0] *= pvelocityNormalizedX;
	kfArray[1] *= pvelocityNormalizedY;
	kfArray[2] *= pvelocityNormalizedZ;

	return kfArray;
}
