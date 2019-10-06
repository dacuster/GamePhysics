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
        // Update the bounding box.
        CalculateBoundingBoxAxis();

        // Set the mesh color of this object to white. (No collision)
        GetComponent<MeshRenderer>().material.color = Color.white;

        // Check for collision with all types of collision hulls currently in the game.
        foreach (CollisionHull2D hull in GameObject.FindObjectsOfType<CollisionHull2D>())
        {
            // TODO: Properly implement.
            Collision collision = new Collision();

            // Don't check collision with itself.
            if (hull == this)
            {
                continue;
            }
            // Other hull is a circle.
            if (hull.Type == CollisionHullType2D.hull_circle)
            {
                // Check for collision vs circle.
                if (TestCollisionVsCircle(hull as CircleCollisionHull2D, ref collision))
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
                if (TestCollisionVsAABB(hull as AxisAlignedBoundingBoxHull2D, ref collision))
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
                if (TestCollisionVsOBB(hull as ObjectBoundingBoxHull2D, ref collision))
                {
                    // Change the mesh color of both colliding objects.
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
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

        // OBB max extends
        Vector2 _xAxisBoundOBB = other.X_AxisBound;
        Vector2 _yAxisBoundOBB = other.Y_AxisBound;

        // Transform the entends to OBB world space.
        other.CalculateBoundingBoxWorld(ref _xAxisBoundOBB, ref _yAxisBoundOBB);

        // Check if the extends for the AABB are colliding with the extends for the OBB in OBB world space.
        if (X_AxisBound.x <= _xAxisBoundOBB.y && X_AxisBound.y >= _xAxisBoundOBB.x && Y_AxisBound.y >= _yAxisBoundOBB.x && Y_AxisBound.x <= _yAxisBoundOBB.y)
        {
            // AABB max extends
            Vector2 _xAxisBoundAABB = X_AxisBound;
            Vector2 _yAxisBoundAABB = Y_AxisBound;

            // Transform the entends to OBB local space.
            other.CalculateBoundingBoxWorld(ref _xAxisBoundAABB, ref _yAxisBoundAABB);

            // Check if the extends for the AABB in OBB local space are colliding with the extends for the OBB in OBB world space.
            if (_xAxisBoundAABB.x <= other.X_AxisBound.y && _xAxisBoundAABB.y >= other.X_AxisBound.x && _yAxisBoundAABB.y >= other.Y_AxisBound.x && _yAxisBoundAABB.x <= other.Y_AxisBound.y)
            {
                // Collision.
                return true;
            }
        }

        return false;
    }

    // Calculate the bounding box axis.
    public void CalculateBoundingBoxAxis()
    {
        // Get the particle for use when game is not running.
        Particle2D particle = GetComponent<Particle2D>();
        xAxisBound.x = particle.Position.x - boundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + boundingBox.x;
        yAxisBound.x = particle.Position.y - boundingBox.y * 0.5f;
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

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        Particle2D particle = boxHull.GetComponent<Particle2D>();

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Update bounding box axis.
        boxHull.CalculateBoundingBoxAxis();

        // Draw a cube to represent the collider on this object.
        Handles.DrawWireCube(particle.Position, boxHull.BoundingBox);

        // Update Particle2D position and rotation based on the Transform component. Only works when the editor isn't in play mode.
        if (!Application.isPlaying)
        {
            particle.Position = boxHull.transform.position;
            particle.Rotation = boxHull.transform.rotation.eulerAngles.z;
        }
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}