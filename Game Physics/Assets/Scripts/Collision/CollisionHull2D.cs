using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract prevent anything from instantiating it.
public abstract class CollisionHull2D : MonoBehaviour
{
    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }

    private CollisionHullType2D Type { get; }


    protected CollisionHull2D(CollisionHullType2D _type)
    {
        Type = _type;
    }

    protected Particle2D particle;


    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b)
    {
        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other);
    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other);
    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other);
}
