using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CircleCollisionHull2D : CollisionHull2D
{
    // Constructor.
    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle) { }

    // Apply range as positive value.
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float radius;

    // Check for collision circle vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // collision passes if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) squared

        // Get the centers of each circle. Calculate the difference in centers.
        Vector2 differenceCenters = Particle.Position - other.Particle.Position;

        // Square the distance in centers.
        float distanceSquared = differenceCenters.sqrMagnitude;

        // Get the radius of each circle. Calculate the sum of both.
        float radiiSum = Radius + other.Radius;

        // Square the sum of the radii.
        float radiiSumSquared = radiiSum * radiiSum;

        // Check if the distance from centers is less than or equal to sum of the radii.
        c.Status = distanceSquared <= radiiSumSquared;

        // Set the collsion data.
        if (c.Status)
        {
            c.ContactCount = 1;
            c.A = this;
            c.B = other;
            Vector2 normal = differenceCenters.normalized;
            c.AddContact(normal * Radius, normal, 0.9f, 0);
        }

        // Return the collision status.
        return c.Status;
    }

    // Check for collision circle vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c)
    {
        // Calculate closest point by clamping circle centers on each dimension
        // passes if closest point vs circle passes

        // Store the particle's position locally.
        float xPosition = Particle.Position.x;
        float yPosition = Particle.Position.y;

        // Get the bounding box axis of the AABB.
        Vector2 xAxisBoundAABB = other.X_AxisBound;
        Vector2 yAxisBoundAABB = other.Y_AxisBound;

        // Translate the bounding box axis to world space.
        other.WorldBoundingBoxAxis(ref xAxisBoundAABB, ref yAxisBoundAABB);

        // Clamp the circle to the other object to get the nearest point on the object to the circle.
        float nearestX = Mathf.Clamp(xPosition, xAxisBoundAABB.x, xAxisBoundAABB.y);
        float nearestY = Mathf.Clamp(yPosition, yAxisBoundAABB.x, yAxisBoundAABB.y);

        // Calculate the distance from the circle to the nearest point on the other object.
        float deltaX = xPosition - nearestX;
        float deltaY = yPosition - nearestY;

        if (debugMode)
        {
            // Draw a debug line from the circle center to the nearest point on the other object.
            Vector2 start = new Vector2(nearestX, nearestY);
            Vector2 end = new Vector2(xPosition, yPosition);
            Debug.DrawLine(start, end, Color.red);
        }

        // Check if the sum squared of the difference from the nearest point is less than or equal to the radius squared.
        return (deltaX * deltaX + deltaY * deltaY) <= (Radius * Radius);
    }

    // Check for collision circle vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // same as above, but first...
        // transform circle position by multiplying by box world matrix inverse

        // Get the circle position for efficient calculations.
        Vector2 circlePosition = Particle.Position;

        // Transform the circle position into the OBB's local space.
        other.WorldToLocal(ref circlePosition);

        // Clamp the circle to the other object.
        float nearestX = Mathf.Clamp(circlePosition.x, other.X_AxisBound.x, other.X_AxisBound.y);
        float nearestY = Mathf.Clamp(circlePosition.y, other.Y_AxisBound.x, other.Y_AxisBound.y);

        // Calculate the distance from the nearest point on the other object to the circle center.
        float deltaX = circlePosition.x - nearestX;
        float deltaY = circlePosition.y - nearestY;

        // Create a vector of the nearest position for matrix multiplication.
        Vector2 nearestPosition = new Vector2(nearestX, nearestY);

        // Transform the nearest point from the OBB's local space into world space.
        other.LocalToWorld(ref nearestPosition);

        if (debugMode)
        {
            // Debug drawing.
            Vector2 start = new Vector2(nearestPosition.x, nearestPosition.y);
            Vector2 end = new Vector2(Particle.Position.x, Particle.Position.y);
            Debug.DrawLine(start, end, Color.red);
        }

        // Check if the distance from centers is less than or equal to sum of the radii.
        c.Status = (deltaX * deltaX + deltaY * deltaY) <= (Radius * Radius);

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
            Vector2 normal = (Particle.Position - other.Particle.Position).normalized;
            c.AddContact(normal * Radius, normal, 0.9f, 0);
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
[CustomEditor(typeof(CircleCollisionHull2D))]
public class CircleEditor2D : Editor
{
    private void OnSceneGUI()
    {
        // Get the circle hull attached to this script.
        CircleCollisionHull2D circleHull = (CircleCollisionHull2D)target;

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        Particle2D particle = circleHull.GetComponent<Particle2D>();

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
