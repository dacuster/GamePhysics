using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AxisAlignedBoundingBoxHull2D : CollisionHull2D
{
    // Constructor.
    public AxisAlignedBoundingBoxHull2D() : base(CollisionHullType2D.hull_aabb) { }

    [SerializeField]
    // Bounding box for this object.
    private Vector2 boundingBox;

    // Bounding axis for this object.
    private Vector2 xAxisBound;
    private Vector2 yAxisBound;

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

    // Check for collision AABB vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // Perform check with circle. (Algorithm implemented there already).
        return other.TestCollisionVsAABB(this);
    }

    // Check for collision AABB vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        // for each dimension, max extent of A >= min extent of B

        // Compare the minimum and maximum extents of the bounding box for this object and the other object.
        if (xAxisBound.x <= other.xAxisBound.y && xAxisBound.y >= other.xAxisBound.x && yAxisBound.y >= other.yAxisBound.x && yAxisBound.x <= other.yAxisBound.y)
        {
            // Collision.
            return true;
        }

        // No collision.
        return false;
    }

    // Check for collision AABB vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again
        // 1. .....

        if (xAxisBound.x <= other.X_AxisBound.y && xAxisBound.y >= other.X_AxisBound.x && yAxisBound.y >= other.Y_AxisBound.x && yAxisBound.x <= other.Y_AxisBound.y)
        {

            return true;
        }

        return false;
    }

    // Calculate the bounding box axis.
    private void CalculateBoundingBoxAxis()
    {
        xAxisBound.x = Particle.Position.x - boundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + boundingBox.x;
        yAxisBound.x = Particle.Position.y - boundingBox.y * 0.5f;
        yAxisBound.y = yAxisBound.x + boundingBox.y;

        return;
    }

    // Accessor for the bounding box x-axis.
    public Vector2 X_AxisBound
    {
        get
        {
            return xAxisBound;
        }
    }

    // Accessor for the bounding box y-axis.
    public Vector2 Y_AxisBound
    {
        get
        {
            return yAxisBound;
        }
    }

    // Accessor for the bounding box.
    public Vector2 BoundingBox
    {
        get
        {
            return boundingBox;
        }

        set
        {
            boundingBox = value;
        }
    }
}

[CustomEditor(typeof(AxisAlignedBoundingBoxHull2D))]
public class AxisBoxEditor : Editor
{
    private void OnSceneGUI()
    {
        // Get the box hull associated with this object.
        AxisAlignedBoundingBoxHull2D boxHull = (AxisAlignedBoundingBoxHull2D)target;

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Draw a cube to represent the collider on this object.
        Handles.DrawWireCube(boxHull.Particle.Position, boxHull.BoundingBox);

        // Update Particle2D position and rotation based on the Transform component. Only works when the editor isn't in play mode.
        if (!Application.isPlaying)
        {
            boxHull.Particle.Position = boxHull.transform.position;
            boxHull.Particle.Rotation = boxHull.transform.rotation.eulerAngles.z;
        }
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}