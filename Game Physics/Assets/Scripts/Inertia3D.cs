using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inertia3D
{
    // Solid sphere Interia Tensor
    public static Matrix4x4 SolidSphereInertiaTensor(float radius, float mass)
    {

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 0.4f * mass * (radius * radius),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 0.4f * mass * (radius * radius),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 0.4f * mass * (radius * radius),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };

        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Hollow Sphere Inertia Tensor
    public static Matrix4x4 HollowSphereInertiaTensor(float radius, float mass)
    {

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

    // Solid Cube Inertia Tensor
    public static Matrix4x4 SolidCubeInertiaTensor(float width, float height, float depth, float mass)
    {

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

    // Hollow Cube Inertia Tensor
    public static Matrix4x4 HollowCubeInertiaTensor(float width, float height, float depth, float mass)
    {

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

    // Solid Cylinder Inertia Tensors
    public static Matrix4x4 SolidCylinderInertiaTensor(float radius, float height, float mass)
    {

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
            m22 = 0.5f * mass * (radius * radius),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    // Solid Cone Intertia Tensor
    public static Matrix4x4 SolidConeInertiaTensor(float radius, float height, float mass)
    {

        Matrix4x4 inertia = new Matrix4x4
        {
            m00 = 0.6f * mass * (height * height) + 0.15f * mass * (radius * radius),
            m01 = 0,
            m02 = 0,
            m03 = 0,

            m10 = 0,
            m11 = 0.6f * mass * (height * height) + 0.15f * mass * (radius * radius),
            m12 = 0,
            m13 = 0,

            m20 = 0,
            m21 = 0,
            m22 = 0.3f * mass * (height * height),
            m23 = 0,

            m30 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1,

        };
        return inertia;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }
}
