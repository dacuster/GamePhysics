﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AxisAlignedBoundingBoxHull3D : CollisionHull3D
{
    // Constructor.
    public AxisAlignedBoundingBoxHull3D() : base(CollisionHullType3D.hull_aabb) { }

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

        base.FixedUpdate();

        //// Set the mesh color of this object to white. (No collision)
        //GetComponent<MeshRenderer>().material.color = Color.white;

        //// Check for collision with all types of collision hulls currently in the game.
        //foreach (CollisionHull2D hull in GameObject.FindObjectsOfType<CollisionHull2D>())
        //{
        //    // TODO: Properly implement.
        //    Collision collision = new Collision();

        //    // Don't check collision with itself.
        //    if (hull == this)
        //    {
        //        continue;
        //    }
        //    // Other hull is a circle.
        //    if (hull.Type == CollisionHullType2D.hull_circle)
        //    {
        //        // Check for collision vs circle.
        //        if (TestCollisionVsCircle(hull as CircleCollisionHull2D, ref collision))
        //        {
        //            // Change the mesh color of both colliding objects.
        //            GetComponent<MeshRenderer>().material.color = Color.red;
        //            hull.GetComponent<MeshRenderer>().material.color = Color.red;
        //        }
        //    }
        //    // Other hull is AABB.
        //    else if (hull.Type == CollisionHullType2D.hull_aabb)
        //    {
        //        // Check collision vs AABB.
        //        if (TestCollisionVsAABB(hull as AxisAlignedBoundingBoxHull2D, ref collision))
        //        {
        //            // Change the mesh color of both colliding objects.
        //            GetComponent<MeshRenderer>().material.color = Color.red;
        //            hull.GetComponent<MeshRenderer>().material.color = Color.red;
        //        }
        //    }
        //    // Other hull is OBB.
        //    else if (hull.Type == CollisionHullType2D.hull_obb)
        //    {
        //        // Check collision vs OBB.
        //        if (TestCollisionVsOBB(hull as ObjectBoundingBoxHull2D, ref collision))
        //        {
        //            // Change the mesh color of both colliding objects.
        //            GetComponent<MeshRenderer>().material.color = Color.red;
        //            hull.GetComponent<MeshRenderer>().material.color = Color.red;
        //        }
        //    }
        //}
    }

    // Check for collision AABB vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull3D other, ref Collision c)
    {
        // Perform check with circle. (Algorithm implemented there already).
        return other.TestCollisionVsAABB(this, ref c);
    }

    // Check for collision AABB vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D other, ref Collision c)
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
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D other, ref Collision c)
    {
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again

        // Transform the extents to OBB world space.
        other.CalculateBoundingBoxWorld();

        // OBB max extents
        Vector2 xAxisBoundOBB = other.X_AxisMaxBound;
        Vector2 yAxisBoundOBB = other.Y_AxisMaxBound;

        // Check if the extents for the AABB are colliding with the extents for the OBB in OBB world space.
        if (X_AxisBound.x <= xAxisBoundOBB.y && X_AxisBound.y >= xAxisBoundOBB.x && Y_AxisBound.y >= yAxisBoundOBB.x && Y_AxisBound.x <= yAxisBoundOBB.y)
        {
            // AABB max extents
            Vector2 _xAxisBoundAABB = X_AxisBound;
            Vector2 _yAxisBoundAABB = Y_AxisBound;

            // Transform the entents to OBB local space.
            other.CalculateBoundingBoxWorld(ref _xAxisBoundAABB, ref _yAxisBoundAABB);

            // Check if the extents for the AABB in OBB local space are colliding with the extents for the OBB in OBB world space.
            if (_xAxisBoundAABB.x <= other.X_AxisBound.y && _xAxisBoundAABB.y >= other.X_AxisBound.x && _yAxisBoundAABB.y >= other.Y_AxisBound.x && _yAxisBoundAABB.x <= other.Y_AxisBound.y)
            {
                // Collision.
                Debug.Log("Collision!");
                return true;
            }
        }

        return false;
    }

    // Calculate the bounding box axis.
    public void CalculateBoundingBoxAxis()
    {
        // Get the particle for use when game is not running.
        Particle3D particle = GetComponent<Particle3D>();
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

[CustomEditor(typeof(AxisAlignedBoundingBoxHull3D))]
public class AxisBoxEditor3D : Editor
{
    private void OnSceneGUI()
    {
        // Get the box hull associated with this object.
        AxisAlignedBoundingBoxHull3D boxHull = (AxisAlignedBoundingBoxHull3D)target;

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        Particle3D particle = boxHull.GetComponent<Particle3D>();

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Update bounding box axis.
        boxHull.CalculateBoundingBoxAxis();

        // Draw a cube to represent the collider on this object.
        Handles.DrawWireCube(particle.Position, boxHull.BoundingBox);
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}