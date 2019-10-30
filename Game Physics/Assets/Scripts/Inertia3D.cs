using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Implement 3D inertia tensors.
public class Inertia3D
{
    public static float SolidSphereIneritaTensor()
    {
        Debug.Log("Solid Sphere");

        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float HollowSphereIneritaTensor()
    {
        Debug.Log("Hollow Sphere");

        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float SolidCubeIneritaTensor()
    {
        Debug.Log("Solid Cube");

        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float HollowCubeIneritaTensor()
    {
        Debug.Log("Hollw Cube");

        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float SolidCylinderIneritaTensor()
    {
        Debug.Log("Solid Cylinder");

        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public static float HollowCylinderIneritaTensor()
    {
        Debug.Log("Hollow Cylinder");

        return 0.0f;
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }
}
