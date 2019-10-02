using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollisionHull2D : CollisionHull2D
{
    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle) { }

    // Apply range as positive value.
    [Range(0.0f, 100.0f)]
    public float radius;

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // collision passes if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) squared
        // 1. get the centers locations
        // 2. difference between centers
        // 3. distance squared = dot(diff, diff)         (squared magnitued - built into unity)
        // 4. sum of radii
        // 5. square sum
        // 6. DO THE TEST: distSq <= sumSq

        Vector2 differenceCenters = particle.position - other.particle.position;

        float distanceSquared = differenceCenters.sqrMagnitude;

        float radiiSum = radius + other.radius;

        float radiiSumSquared = radiiSum * radiiSum;
        if (distanceSquared <= radiiSumSquared)
        {
            Debug.Log(particle.position.x + " " + particle.position.y);
            Debug.Log("Distance sq: " + distanceSquared + "Radii sum: " + radiiSum + "Radii sum sq: " + radiiSumSquared + " Collision");
        }

        return distanceSquared <= radiiSumSquared;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        // Calculate closest point by clamping circle centers on each dimension
        // passes if closest point vs circle passes
        // 1. .....


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // same as above, but first...
        // transform circle position by multiplying by box world matrix inverse
        // 1. .....


        return false;
    }
}
