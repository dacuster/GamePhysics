using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Implement 3D inertia tensors.
public class Inertia3D
{
    // Solid sphere Interia Tensor
    public static Matrix4x4 SolidSphereInertiaTensor(float radius, float mass)
    {
        Debug.Log("Solid Sphere");

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 2/5 * mass * (radius * radius),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 2 / 5 * mass * (radius * radius),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 2 / 5 * mass * (radius * radius),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };

        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid sphere Inverse Interia Tensor
    public static Matrix4x4 SolidSphereInverseInertiaTensor(float radius, float mass)
    {
        Matrix4x4 invInertia = new Matrix4x4
        {
            m00 = 5/(2 * mass * (radius * radius)),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 5 / (2 * mass * (radius * radius)),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 5 / (2 * mass * (radius * radius)),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };

        return invInertia;
    }

    // Hollow Sphere Inertia Tensor
    public static Matrix4x4 HollowSphereInertiaTensor(float radius, float mass)
    {
        Debug.Log("Hollow Sphere");

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 2 / 3 * mass * (radius * radius),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 2 / 3 * mass * (radius * radius),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 2 / 3 * mass * (radius * radius),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Hollow Sphere Inverse Inertia Tensor
    public static Matrix4x4 HollowSphereInverseInertiaTensor(float radius, float mass)
    {

        Matrix4x4 invInertia = new Matrix4x4
        {
            m00 = 3/(2 * mass * (radius * radius)),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 3 / (2 * mass * (radius * radius)),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 3 / (2 * mass * (radius * radius)),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return invInertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid Cube Inertia Tensor
    public static Matrix4x4 SolidCubeInertiaTensor(float width, float height, float depth, float mass)
    {
        Debug.Log("Solid Cube");

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 1 / 12 * mass *((height * height) + (depth * depth)),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 1 / 12 * mass * ((depth * depth) + (width * width)),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 1 / 12 * mass * ((width * width) + (height * height)),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid Cube Inverse Inertia Tensor
    public static Matrix4x4 SolidCubeInverseInertiaTensor(float width, float height, float depth, float mass)
    {
        Debug.Log("Solid Cube");

        Matrix4x4 invInertia = new Matrix4x4
        {
            m00 = 12 / (mass * ((height * height) + (depth * depth))),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 12 / (mass * ((depth * depth) + (width * width))),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 12 / (mass * ((width * width) + (height * height))),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return invInertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Hollow Cube Inertia Tensor
    public static Matrix4x4 HollowCubeInertiaTensor(float width, float height, float depth, float mass)
    {
        Debug.Log("Hollw Cube");

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 5/3 * mass * ((height * height) + (depth * depth)),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 5 / 3 * mass * ((depth * depth) + (width * width)),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 5 / 3 * mass * ((width * width) + (height * height)),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Hollow Cube Inverse Inertia Tensor
    public static Matrix4x4 HollowCubeInverseInertiaTensor(float width, float height, float depth, float mass)
    {
        Debug.Log("Hollw Cube");

        Matrix4x4 invInertia = new Matrix4x4
        {
            m00 = 3 / (5 * mass * ((height * height) + (depth * depth))),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 3 / (5 * mass * ((depth * depth) + (width * width))),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 3 / (5 * mass * ((width * width) + (height * height))),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return invInertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid Cylinder Inertia Tensors
    public static Matrix4x4 SolidCylinderInertiaTensor(float radius, float height, float mass)
    {
        Debug.Log("Solid Cylinder");

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 1 / 12 * mass * (3 * (radius * radius) + (height * height)),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 1 / 12 * mass * (3 * (radius * radius) + (height * height)),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 1/2 * mass * (radius * radius),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid Cylinder Inverse Inertia Tensors
    public static Matrix4x4 SolidCylinderInverseInertiaTensor(float radius, float height, float mass)
    {
        Debug.Log("Solid Cylinder");

        Matrix4x4 invInertia = new Matrix4x4
        {
            m00 = 4 / (mass * (3 * (radius * radius) + (height * height))),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 1 / 12 * mass * (3 * (radius * radius) + (height * height)),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 2 / (mass * (radius * radius)),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return invInertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid Cone Intertia Tensor
    public static Matrix4x4 SolidConeInertiaTensor(float radius, float height, float mass)
    {
        Debug.Log("Hollow Cylinder");

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 3/5 * mass * (height * height) + 3/ 20 * mass * (height * height),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 3 / 5 * mass * (height * height) + 3 / 20 * mass * (height * height),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 3 / 10 * mass * (height * height),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid Cone Inverse Intertia Tensor
    public static Matrix4x4 SolidConeInverseInertiaTensor(float radius, float height, float mass)
    {
        Debug.Log("Hollow Cylinder");

        Matrix4x4 invinertia = new Matrix4x4
        {
            m00 = 20/ (12 * (height * height) * mass  + 3 * mass * (radius * radius)),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 20 / (12 * (height * height) * mass + 3 * mass * (radius * radius)),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 10 / (mass * (radius * radius)),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return invinertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }
}
