using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public ObjectBoundingBoxHull2D() : base(CollisionHullType2D.hull_obb) { }
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
        // see AABB


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // AABB-OBB part 2 twice
        // 1. .....


        return false;
    }
}
