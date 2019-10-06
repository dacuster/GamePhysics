using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    // Constructor.
    public ObjectBoundingBoxHull2D() : base(CollisionHullType2D.hull_obb) { }

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

    // Check for collision OBB vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // Perform check with circle. (Algorithm implemented there already).
        return other.TestCollisionVsOBB(this);
    }

    // Check for collision OBB vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        // Perform check with AABB. (Algorithm implemented there already).
        return other.TestCollisionVsOBB(this);
    }

    // Check for collision OBB vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // AABB-OBB part 2 twice
        // 1. .....


        return false;
    }

    // Calculate the bounding box axis.
    private void CalculateBoundingBox()
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

    // Transform the bounding x-axis.
    public Vector2 LocalToWorldAxisX()
    {
        return LocalToWorld().MultiplyPoint3x4(xAxisBound);
    }

    // Transform the bounding y-axis.
    public Vector2 LocalToWorldAxisY()
    {
        return LocalToWorld().MultiplyPoint3x4(yAxisBound);
    }

    // Get the local to world transformation.
    public Matrix4x4 LocalToWorld()
    {
        // Get the position of this particle.
        Vector3 position = Particle.Position;
        // Build a quaternion based on the euler rotation of this particle.
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Particle.Rotation);

        // Translation matrix of this particle.
        Matrix4x4 translate = Matrix4x4.Translate(position);
        // Invert the translation.
        Matrix4x4 translateInverse = translate.inverse;
        // Rotation matrix of this particle.
        Matrix4x4 rotate = Matrix4x4.Rotate(rotation);
        // Invert the rotation.
        Matrix4x4 rotateInverse = rotate.inverse;

        // Model matrix to convert from local to world space.
        Matrix4x4 model = translate * rotateInverse * translateInverse;

        return model;
    }
}

[CustomEditor(typeof(ObjectBoundingBoxHull2D))]
public class ObjectBoxEditor : Editor
{
    private void OnSceneGUI()
    {
        // Get the box hull associated with this object.
        ObjectBoundingBoxHull2D boxHull = (ObjectBoundingBoxHull2D)target;

        // Get the position of the box hull.
        Vector3 position = boxHull.Particle.Position;

        // Get the world matrix of the box hull.
        Matrix4x4 matrix = boxHull.LocalToWorld();

        // Transform position to world space of the box hull.
        position = matrix.MultiplyPoint3x4(position);

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Change the gizmo drawing matrix.
        Handles.matrix = matrix;

        // Draw a cube to represent the collider on this object.
        Handles.DrawWireCube(position, boxHull.BoundingBox);

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
