#ifndef VECTOR3D_H
#define VECTOR3D_H

// Vector in 3D space.
class Vector3D
{
public:
	// Construct a Vector3D.
	Vector3D(float x = 0.0f, float y = 0.0f, float z = 0.0f);
	
	// Destruct the Vector3D.
	~Vector3D() { }

	// Get the coordinate values of the vector.
	void getCoordinates(float& x, float& y, float& z);

	// Vector magnitude.
	float magnitude();
	
	// Vector squared magnitude.
	float magnitudeSquared();

	// Convert to unit vector.
	Vector3D normalized();

	// Vector dot product.
	float dot(Vector3D* const other);

	// Scalar multiplication.
	Vector3D operator*(float const rightFloat);

	// Overload the addition operator.
	Vector3D operator+(Vector3D const other);

	// Overload the subtraction operator.
	Vector3D operator-(Vector3D const other);

	// Overload the negation operator.
	Vector3D operator-() const;

private:
	float mX;
	float mY;
	float mZ;

	Vector3D operator/(float const rightFloat);
};

#endif // !VECTOR3D_H