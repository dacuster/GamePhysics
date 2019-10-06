using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CircleCollisionHull2D : CollisionHull2D
{
    // Constructor.
    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle) { }

    // Apply range as positive value.
    [Range(0.0f, 100.0f)]
    public float radius;

    private void FixedUpdate()
    {
        // Set the mesh color of this object to white. (No collision)
        GetComponent<MeshRenderer>().material.color = Color.white;

        // Check for collision with all types of collision hulls currently in the game.
        foreach (CollisionHull2D hull in GameObject.FindObjectsOfType<CollisionHull2D>())
        {
            // Don't check collision with itself.
            if (hull == this)
            {
                continue;
            }
            // Other hull is a circle.
            if (hull.Type == CollisionHullType2D.hull_circle)
            {
                // Check for collision vs circle.
                if (TestCollisionVsCircle(hull as CircleCollisionHull2D))
                {
                    // Change the mesh color of both colliding objects.
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
            // Other hull is AABB.
            else if (hull.Type == CollisionHullType2D.hull_aabb)
            {
                // Check collision vs AABB.
                if (TestCollisionVsAABB(hull as AxisAlignedBoundingBoxHull2D))
                {
                    // Change the mesh color of both colliding objects.
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
            // Other hull is OBB.
            else if (hull.Type == CollisionHullType2D.hull_obb)
            {
                // Check collision vs OBB.
                if (TestCollisionVsOBB(hull as ObjectBoundingBoxHull2D))
                {
                    // Change the mesh color of both colliding objects.
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    // Check for collision circle vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // collision passes if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) squared

        // Get the centers of each circle. Calculate the difference in centers.
        Vector2 differenceCenters = Particle.Position - other.Particle.Position;

        // Square the distance in centers.
        float distanceSquared = differenceCenters.sqrMagnitude;

        // Get the radius of each circle. Calculate the sum of both.
        float radiiSum = radius + other.radius;

        // Square the sum of the radii.
        float radiiSumSquared = radiiSum * radiiSum;

        // Check if the distance from centers is less than or equal to sum of the radii.
        return distanceSquared <= radiiSumSquared;
    }

    // Check for collision circle vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        // Calculate closest point by clamping circle centers on each dimension
        // passes if closest point vs circle passes

        // Store the particle's position locally.
        float xPosition = Particle.Position.x;
        float yPosition = Particle.Position.y;

        // Clamp the circle to the other object to get the nearest point on the object to the circle.
        float nearestX = Mathf.Clamp(xPosition, other.X_AxisBound.x, other.X_AxisBound.y);
        float nearestY = Mathf.Clamp(yPosition, other.Y_AxisBound.x, other.Y_AxisBound.y);

        // Calculate the distance from the circle to the nearest point on the other object.
        float deltaX = xPosition - nearestX;
        float deltaY = yPosition - nearestY;

        // Draw a debug line from the circle center to the nearest point on the other object.
        Vector2 start = new Vector2(nearestX, nearestY);
        Vector2 end = new Vector2(xPosition, yPosition);
        Debug.DrawLine(start, end, Color.red);

        // Check if the sum squared of the difference from the nearest point is less than or equal to the radius squared.
        return (deltaX * deltaX + deltaY * deltaY) <= (radius * radius);
    }

    // Check for collision circle vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // same as above, but first...
        // transform circle position by multiplying by box world matrix inverse

        // Get the circle position for efficient calculations.
        Vector3 circlePosition = Particle.Position;

        // Get the local to world matrix of the other object.
        Matrix4x4 matrix = other.LocalToWorld();

        // Transform the circle position.
        circlePosition = matrix.MultiplyPoint3x4(circlePosition);

        // Clamp the circle to the other object.
        float nearestX = Mathf.Clamp(circlePosition.x, other.X_AxisBound.x, other.X_AxisBound.y);
        float nearestY = Mathf.Clamp(circlePosition.y, other.Y_AxisBound.x, other.Y_AxisBound.y);

        // Calculate the distance from the nearest point on the other object to the circle center.
        float deltaX = circlePosition.x - nearestX;
        float deltaY = circlePosition.y - nearestY;

        // Create a vector of the nearest position for matrix multiplication.
        Vector3 nearestPosition = new Vector2(nearestX, nearestY);

        // Muliply position by inverse model view matrix to position and rotate properly.
        nearestPosition = matrix.inverse.MultiplyPoint3x4(nearestPosition);

        // Debug drawing.
        Vector2 start = new Vector2(nearestPosition.x, nearestPosition.y);
        Vector2 end = new Vector2(circlePosition.x, circlePosition.y);
        Debug.DrawLine(start, end, Color.red);

        // Check if the nearest point is colliding with the circle.
        return (deltaX * deltaX + deltaY * deltaY) <= (radius * radius);
    }

}
[CustomEditor(typeof(CircleCollisionHull2D))]
public class CircleEditor : Editor
{
    private void OnSceneGUI()
    {
        // Get the circle hull attached to this script.
        CircleCollisionHull2D circleHull = (CircleCollisionHull2D)target;

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Draw a disc to represent the collider on this circle.
        Handles.DrawWireDisc(circleHull.Particle.Position, Vector3.back, circleHull.radius);

        // Update Particle2D position and rotation based on the Transform component. Only works when the editor isn't in play mode.
        if (!Application.isPlaying)
        {
            circleHull.Particle.Position = circleHull.transform.position;
            circleHull.Particle.Rotation = circleHull.transform.rotation.eulerAngles.z;
        }
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
