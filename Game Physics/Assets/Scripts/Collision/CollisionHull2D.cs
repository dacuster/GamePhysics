using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract prevent anything from instantiating it.
public abstract class CollisionHull2D : MonoBehaviour
{
    // Different types of collision hulls.
    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }


    // Constructor.
    protected CollisionHull2D(CollisionHullType2D type)
    {
        // Set the type to the given type.
        Type = type;

        return;
    }

    void Awake()
    {
        // Assign the Particle2D component.
        Particle = gameObject.GetComponent<Particle2D>();

        return;
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b)
    {		
        return false;
    }

    /********************************
    **  Abstract Collision Checks  **
    ********************************/

    // Check collision of this vs circle.
    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other);
    // Check collision of this vs AABB.
    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other);
    // Check collision of this vs OBB.
    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other);

    // Particle accessor.
    public Particle2D Particle { get; set; }

    // Accessor for collision hull type.
    public CollisionHullType2D Type { get; }

    //protected CollisionHullType2D Type { set; }
}