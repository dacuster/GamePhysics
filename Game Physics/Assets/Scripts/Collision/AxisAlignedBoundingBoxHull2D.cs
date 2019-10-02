using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxHull2D : CollisionHull2D
{
    public AxisAlignedBoundingBoxHull2D() : base(CollisionHullType2D.hull_aabb) { }
    private void FixedUpdate()
    {
        foreach (CollisionHull2D hull in GameObject.FindObjectsOfType<CollisionHull2D>())
        {
            Collision collision = new Collision();

            if (hull == this)
            {
                break;
            }

            if (hull.Type == CollisionHullType2D.hull_circle)
            {
                TestCollisionVsCircle(hull as CircleCollisionHull2D, ref collision);
            }
            else if (hull.Type == CollisionHullType2D.hull_aabb)
            {
                TestCollisionVsAABB(hull as AxisAlignedBoundingBoxHull2D, ref collision);
            }
            else if (hull.Type == CollisionHullType2D.hull_obb)
            {
                TestCollisionVsOBB(hull as ObjectBoundingBoxHull2D, ref collision);
            }
        }
    }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // see circle
        // other.TestCollisionVsCircle(this);

        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c)
    {
        // for each dimension, max extent of A >= min extent of B
        // 1. .....


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again
        // 1. .....


        return false;
    }
}
