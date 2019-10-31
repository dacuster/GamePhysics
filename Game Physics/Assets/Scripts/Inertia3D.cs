using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Implement 3D inertia tensors.
public class Inertia3D
{
    [SerializeField]
    private float inertia;
    private float invertiaInv;

    public static float SolidSphereIneritaTensor(float radius, float mass)
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

        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float HollowSphereIneritaTensor(float radius, float mass)
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
        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float SolidCubeIneritaTensor(float width, float height, float depth, float mass)
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
        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float HollowCubeIneritaTensor(float width, float height, float depth, float mass)
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
        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float SolidCylinderIneritaTensor(float radius, float height, float mass)
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
        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float SolidConeIneritaTensor(float radius, float height, float mass)
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
        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }
}
