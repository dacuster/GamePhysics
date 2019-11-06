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

    new private void FixedUpdate()
    {
        // Update the bounding box.
        CalculateBoundingBoxAxis();

        base.FixedUpdate();
    }

    // Check for collision AABB vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // Perform check with circle. (Algorithm implemented there already).
        return other.TestCollisionVsAABB(this, ref c);
    }

    // Check for collision AABB vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c)
    {
        // for each dimension, max extent of A >= min extent of B

        // Compare the minimum and maximum extents of the bounding box for this object and the other object.
        if (X_AxisBound.x <= other.X_AxisBound.y && X_AxisBound.y >= other.X_AxisBound.x && Y_AxisBound.y >= other.Y_AxisBound.x && Y_AxisBound.x <= other.Y_AxisBound.y)
        {
            // Collision.
            return true;
        }

        // No collision.
        return false;
    }

    // Check for collision AABB vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again

        // OBB max extents
        Vector2 xAxisBoundOBB = other.X_AxisBound;
        Vector2 yAxisBoundOBB = other.Y_AxisBound;

        // Transform the extents to OBB world space.
        other.CalculateBoundingBoxWorld(ref xAxisBoundOBB, ref yAxisBoundOBB);

        // Get the AABB bounding axis.
        Vector2 xAxisBoundAABB = X_AxisBound;
        Vector2 yAxisBoundAABB = Y_AxisBound;

        // Get the AABB max extents in world space.
        WorldBoundingBoxAxis(ref xAxisBoundAABB, ref yAxisBoundAABB);

        // Check if the extents for the AABB are colliding with the extents for the OBB in world space.
        if (xAxisBoundAABB.x <= xAxisBoundOBB.y && xAxisBoundAABB.y >= xAxisBoundOBB.x && yAxisBoundAABB.y >= yAxisBoundOBB.x && yAxisBoundAABB.x <= yAxisBoundOBB.y)
        {
            // Transform both object entents to OBB local space.
            other.CalculateBoundingBoxLocal(ref xAxisBoundAABB, ref yAxisBoundAABB);

            // Check if the extents for the AABB in OBB local space are colliding with the extents for the OBB in OBB world space.
            if (xAxisBoundAABB.x <= other.X_AxisBound.y && xAxisBoundAABB.y >= other.X_AxisBound.x && yAxisBoundAABB.y >= other.Y_AxisBound.x && yAxisBoundAABB.x <= other.Y_AxisBound.y)
            {
                // Collision.
                Debug.Log("Collision!");
                return true;
            }
        }

        return false;
    }

    // Transform given axis into world space from local space.
    private void LocalToWorld(ref Vector2 axis)
    {
        // Particle needed for scene editor when game isn't running.
        Particle2D particle = GetComponent<Particle2D>();

        // Get the position of this particle.
        Vector2 position = particle.Position;

        // Build a quaternion based on the euler rotation of this particle.
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, particle.Rotation);

        Vector3 localScale = particle.transform.localScale;

        // Translation matrix of this particle.
        Matrix4x4 translate = Matrix4x4.Translate(position);

        // Rotation matrix of this particle.
        Matrix4x4 rotate = Matrix4x4.Rotate(rotation);

        // Scale matrix of this particle.
        Matrix4x4 scale = Matrix4x4.Scale(localScale);

        // Model matrix to convert from local to world space.
        Matrix4x4 model = translate * rotate * scale;

        // Transform the axis by the model matrix.
        axis = model.MultiplyPoint3x4(axis);

        return;
    }

    public Vector2 WorldBoundingBox
    {
        get
        {
            Vector2 box = boundingBox;

            LocalToWorld(ref box);

            return box;
        }
    }

    public void WorldBoundingBoxAxis(ref Vector2 xAxis, ref Vector2 yAxis)
    {
        LocalToWorld(ref xAxis);
        LocalToWorld(ref yAxis);

        return;
    }

    // Calculate the bounding box axis.
    public void CalculateBoundingBoxAxis()
    {
        xAxisBound.x = -boundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + boundingBox.x;
        yAxisBound.x = -boundingBox.y * 0.5f;
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
    }
}

[CustomEditor(typeof(AxisAlignedBoundingBoxHull2D))]
public class AxisBoxEditor : Editor
{
    private void OnSceneGUI()
    {
        // Get the box hull associated with this object.
        AxisAlignedBoundingBoxHull2D boxHull = (AxisAlignedBoundingBoxHull2D)target;

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        Particle2D particle = boxHull.GetComponent<Particle2D>();

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Draw a cube to represent the collider on this object.
        Handles.DrawWireCube(particle.Position, boxHull.BoundingBox);
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}