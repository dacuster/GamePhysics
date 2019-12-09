using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CircleCollisionHull3D : CollisionHull3D
{
    // Constructor.
    public CircleCollisionHull3D() : base(CollisionHullType3D.hull_circle) { }

    // Apply range as positive value.
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float radius = 0.5f;

    // TODO: Return bool from DLL.
    // Pass in (Vector3(3 floats))this.position, (float)this.radius, (Vector3(3 float))other.position, (float)other.radius
    // bool testCollisionVsCircle(float thisX, float thisY, float thisZ, float thisRadius, float otherX, float otherY, float otherZ, float otherRadius);

    // Check for collision circle vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull3D other, ref Collision c)
    {
        // collision passes if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) squared
        // START DLL OPERATIONS
        // Get the centers of each circle. Calculate the difference in centers.
        Vector3 differenceCenters = Particle.Position - other.Particle.Position;

        // Square the distance in centers.
        float distanceSquared = differenceCenters.sqrMagnitude;

        // Get the radius of each circle. Calculate the sum of both.
        float radiiSum = Radius + other.Radius;

        // Square the sum of the radii.
        float radiiSumSquared = radiiSum * radiiSum;

        // Check if the distance from centers is less than or equal to sum of the radii.
        c.Status = distanceSquared <= radiiSumSquared;
        // END DLL OPERATIONS (c.Status = testCollisionVsCircle(float thisX, float thisY, float thisZ, float thisRadius, float otherX, float otherY, float otherZ, float otherRadius);)

        // Set the collsion data.
        if (c.Status)
        {
            c.ContactCount = 1;
            c.A = this;
            c.B = other;
            Vector3 normal = differenceCenters.normalized;
            float penetration = radiiSum - Mathf.Sqrt(distanceSquared);
            c.AddContact(Particle.Position + differenceCenters * 0.5f, normal, restitutionCoefficient, penetration * 0.5f, 0);
        }

        // Return the collision status.
        return c.Status;
    }

    // TODO: Return bool from DLL.
    // testCollisionVsAABB(
    // Check for collision circle vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D other, ref Collision c)
    {
        // Calculate closest point by clamping circle centers on each dimension
        // passes if closest point vs circle passes

        // TODO: Vector3D.Init(Particle.Position.x, Particle.Position.y, Particle.Position.z);
        // Store the particle's position locally.
        float xPosition = Particle.Position.x;
        float yPosition = Particle.Position.y;
        float zPosition = Particle.Position.z;

        // Get the AABB bounding box axis.
        Vector2 xAxisBoundAABB = other.X_AxisBound;
        Vector2 yAxisBoundAABB = other.Y_AxisBound;
        Vector2 zAxisBoundAABB = other.Z_AxisBound;

        // Transform the AABB bouding box into world space.
        other.WorldBoundingBoxAxis(ref xAxisBoundAABB, ref yAxisBoundAABB, ref zAxisBoundAABB);

        // Clamp the circle to the other object to get the nearest point on the object to the circle.
        float nearestX = Mathf.Clamp(xPosition, xAxisBoundAABB.x, xAxisBoundAABB.y);
        float nearestY = Mathf.Clamp(yPosition, yAxisBoundAABB.x, yAxisBoundAABB.y);
        float nearestZ = Mathf.Clamp(zPosition, zAxisBoundAABB.x, zAxisBoundAABB.y);

        // Calculate the distance from the circle to the nearest point on the other object.
        float deltaX = xPosition - nearestX;
        float deltaY = yPosition - nearestY;
        float deltaZ = zPosition - nearestZ;

        // Draw a debug line from the circle center to the nearest point on the other object.
        Vector3 start = new Vector3(nearestX, nearestY, nearestZ);
        Vector3 end = Particle.Position;
        Debug.DrawLine(start, end, Color.red);

        // Check if the sum squared of the difference from the nearest point is less than or equal to the radius squared.
        return (deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ) <= (Radius * Radius);
    }

    // Check for collision circle vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D other, ref Collision c)
    {
        // same as above, but first...
        // transform circle position by multiplying by box world matrix inverse

        // Get the circle position for efficient calculations.
        Vector3 circlePosition = Particle.Position;

        // Transform the circle position into the OBB's local space.
        other.WorldToLocal(ref circlePosition);

        // Clamp the circle to the other object.
        float nearestX = Mathf.Clamp(circlePosition.x, other.X_AxisBound.x, other.X_AxisBound.y);
        float nearestY = Mathf.Clamp(circlePosition.y, other.Y_AxisBound.x, other.Y_AxisBound.y);
        float nearestZ = Mathf.Clamp(circlePosition.z, other.Z_AxisBound.x, other.Z_AxisBound.y);

        // Calculate the distance from the nearest point on the other object to the circle center.
        //float deltaX = circlePosition.x - nearestX;
        //float deltaY = circlePosition.y - nearestY;
        //float deltaZ = circlePosition.z - nearestZ;

        // Create a vector with the delta position



        // Create a vector of the nearest position for matrix multiplication.
        Vector3 nearestPosition = new Vector3(nearestX, nearestY, nearestZ);

        // Calculate the distance from sphere position to the closest point on the OBB.
        Vector3 distance = nearestPosition - circlePosition;

        // Transform the nearest point from the OBB's local space into world space.
        other.LocalToWorld(ref nearestPosition);

        // Debug drawing.
        if (debugMode)
        {
            Debug.DrawLine(nearestPosition, Particle.Position, Color.red);
        }

        // Check if the distance from centers is less than or equal to sum of the radii.
        c.Status = distance.sqrMagnitude <= (Radius * Radius);

        // Set the collsion data.
        if (c.Status)
        {
            //// Calculate the max extents of the OBB.
            //other.CalculateBoundingBoxWorld();

            //// Tracker for points of contact count.
            //int numberOfContacts = 0;

            //// Add each point of contact that is colliding with the circle.
            //for (int currentExtent = 0; currentExtent < 4; currentExtent++)
            //{
            //    if ()
            //}

            c.ContactCount = 1;
            c.A = this;
            c.B = other;
            // Normalized vector from the nearest point on the OBB to the position in world space of the sphere.
            Vector3 normal = (Particle.Position - nearestPosition).normalized;

            Debug.DrawLine(nearestPosition - normal,nearestPosition + normal, Color.yellow);
            float penetration = Radius - distance.magnitude;
            c.AddContact(nearestPosition, normal, restitutionCoefficient, penetration * 0.5f, 0);
        }

        // Return the collision status.
        return c.Status;
    }

    // Radius Accessor
    public float Radius
    {
        get
        {
            return radius;
        }

        set
        {
            radius = value;

            return;
        }
    }

}
[CustomEditor(typeof(CircleCollisionHull3D))]
public class CircleEditor3D : Editor
{
    private void OnSceneGUI()
    {
        // Get the circle hull attached to this script.
        CircleCollisionHull3D circleHull = (CircleCollisionHull3D)target;

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        Particle3D particle = circleHull.GetComponent<Particle3D>();

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Draw a disc to represent the collider on this circle.
        Handles.DrawWireDisc(particle.Position, Vector3.back, circleHull.Radius);
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
