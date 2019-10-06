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
        // Update bounding box axis.
        CalculateBoundingBoxAxis();

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
    public void CalculateBoundingBoxAxis()
    {
        // Get the particle for use when game is not running.
        Particle2D particle = GetComponent<Particle2D>();
        xAxisBound.x = particle.Position.x - BoundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + BoundingBox.x;
        yAxisBound.x = particle.Position.y - BoundingBox.y * 0.5f;
        yAxisBound.y = yAxisBound.x + BoundingBox.y;

        return;
    }

    // Calculate the bounding box axis.
    public void CalculateBoundingBoxAxis(Particle2D particle, Vector2 boundingBox, ref Vector2 _xAxisBound, ref Vector2 _yAxisBound)
    {
        // Bring the particle to world space of this object.
        Vector3 position = WorldToLocal().MultiplyPoint3x4(particle.Position);
        _xAxisBound.x = position.x - boundingBox.x * 0.5f;
        _xAxisBound.y = _xAxisBound.x + boundingBox.x;
        _yAxisBound.x = position.y - boundingBox.y * 0.5f;
        _yAxisBound.y = _yAxisBound.x + boundingBox.y;

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

    // Get the local to world transformation.
    public Matrix4x4 LocalToWorld()
    {
        // Particle needed for scene editor when game isn't running.
        Particle2D particle = GetComponent<Particle2D>();

        // Get the position of this particle.
        Vector3 position = particle.Position;

        // Build a quaternion based on the euler rotation of this particle.
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, particle.Rotation);

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

    // Get the world to local transformation.
    public Matrix4x4 WorldToLocal()
    {
        // Particle needed for scene editor when game isn't running.
        Particle2D particle = GetComponent<Particle2D>();

        // Get the position of this particle.
        Vector3 position = particle.Position;

        // Build a quaternion based on the euler rotation of this particle.
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, particle.Rotation);

        // Translation matrix of this particle.
        Matrix4x4 translate = Matrix4x4.Translate(position);

        // Invert the translation.
        Matrix4x4 translateInverse = translate.inverse;

        // Rotation matrix of this particle.
        Matrix4x4 rotate = Matrix4x4.Rotate(rotation);

        // Invert the rotation.
        Matrix4x4 rotateInverse = rotate.inverse;

        // Model matrix to convert from local to world space.
        Matrix4x4 model = translate * rotate * translateInverse;

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

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        Particle2D particle = boxHull.GetComponent<Particle2D>();

        // Get the position of the box hull.
        Vector3 position = particle.Position;

        // Get the world matrix of the box hull.
        Matrix4x4 matrix = boxHull.WorldToLocal();

        // Transform position to world space of the box hull.
        position = matrix.MultiplyPoint3x4(position);

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;

        // Change the gizmo drawing matrix.
        Handles.matrix = matrix;

        // Update bounding box axis.
        boxHull.CalculateBoundingBoxAxis();

        // Get the bounds position for each box edge.
        Vector3 leftBound = new Vector3(boxHull.X_AxisBound.x, particle.Position.y);
        Vector3 rightBound = new Vector3(boxHull.X_AxisBound.y, particle.Position.y);
        Vector3 bottomBound = new Vector3(particle.Position.x, boxHull.Y_AxisBound.x);
        Vector3 topBound = new Vector3(particle.Position.x, boxHull.Y_AxisBound.y);

        // Draw the axis for the bounds. (Rotated automatically by the new handles matrix. WorldToLocal)
        Handles.DrawLine(leftBound, rightBound);
        Handles.DrawLine(bottomBound, topBound);

        // Draw a cube to represent the collider on this object.
        Handles.DrawWireCube(position, boxHull.BoundingBox);

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
