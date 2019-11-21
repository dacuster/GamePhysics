#include <math.h>

#include "Vector3D.h"

// Construct a Vector3D.
Vector3D::Vector3D(float x, float y, float z)
{
	mX = x;
	mY = y;
	mZ = z;

	return;
}

// Get the coordinate values of the vector.
void Vector3D::getCoordinates(float& x, float& y, float& z)
{
	x = mX;
	y = mY;
	z = mZ;

	return;
}

// Vector magnitude.
float Vector3D::magnitude()
{
	return sqrtf(magnitudeSquared());
}

// Vector squared magnitude.
float Vector3D::magnitudeSquared()
{
	return mX * mX + mY * mY + mZ * mZ;
}

Vector3D Vector3D::normalized()
{
	return *this / magnitude();
}

// Vector3D dot product.
float Vector3D::dot(Vector3D* const other)
{
	return mX * other->mX + mY * other->mY + mZ * other->mZ;
}

// Scalar multiplication.
Vector3D Vector3D::operator*(float const rightFloat)
{
	return Vector3D(mX * rightFloat, mY * rightFloat, mZ * rightFloat);
}

Vector3D Vector3D::operator-() const
{
	return Vector3D(-mX, -mY, -mZ);
}


Vector3D Vector3D::operator/(float const rightFloat)
{
	return Vector3D(mX / rightFloat, mY / rightFloat, mZ / rightFloat);
}
