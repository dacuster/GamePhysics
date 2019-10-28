using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inertia3D : MonoBehaviour
{
    [SerializeField]
    private float inertia = 0.0f;
    private float inertiaInv = 0.0f;
    public void SolidSphereIneritaTensor()
    {
        Debug.Log("Solid Sphere");
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public void HollowSphereIneritaTensor()
    {
        Debug.Log("Hollow Sphere");
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public void SolidCubeIneritaTensor()
    {
        Debug.Log("Solid Cube");
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public void HollowCubeIneritaTensor()
    {
        Debug.Log("Hollw Cube");
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public void SolidCylinderIneritaTensor()
    {
        Debug.Log("Solid Cylinder");
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }

    public void HollowCylinderIneritaTensor()
    {
        Debug.Log("Hollow Cylinder");
        //inertiaInv = 12 / Mass * (transform.localScale.x * transform.localScale.x + transform.localScale.y * transform.localScale.y);
    }
}
