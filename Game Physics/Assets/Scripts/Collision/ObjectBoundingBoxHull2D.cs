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

        //// This OBB max extends.
        //Vector2 _xAxisBoundThis = X_AxisBound;
        //Vector2 _yAxisBoundThis = Y_AxisBound;

        //// Transform the entends to OBB local space.
        //other.CalculateBoundingBoxLocal(ref _xAxisBoundThis, ref _yAxisBoundThis);

        //// Other OBB max extends.
        //Vector2 _xAxisBoundOther = other.X_AxisBound;
        //Vector2 _yAxisBoundOther = other.Y_AxisBound;

        //// Transform the entends to this OBB world space.
        //CalculateBoundingBoxLocal(ref _xAxisBoundOther, ref _yAxisBoundOther);

        //// Check if the extends for the AABB are colliding with the extends for the OBB in OBB world space.
        //if (_xAxisBoundOther.x <= _xAxisBoundThis.y && _xAxisBoundOther.y >= _xAxisBoundThis.x && _yAxisBoundOther.y >= _yAxisBoundThis.x && _yAxisBoundOther.x <= _yAxisBoundThis.y)
        //{
        //    Matrix4x4 otherWorldSpace = other.WorldToLocal();

        //    _xAxisBoundOther = otherWorldSpace.MultiplyPoint3x4(other.X_AxisBound);
        //    _yAxisBoundOther = otherWorldSpace.MultiplyPoint3x4(other.Y_AxisBound);
        //    _xAxisBoundThis = WorldToLocal().MultiplyPoint3x4(X_AxisBound);
        //    _yAxisBoundThis = WorldToLocal().MultiplyPoint3x4(Y_AxisBound);

        //    // Check if the extends for the AABB in OBB local space are colliding with the extends for the OBB in OBB world space.
        //    if (_xAxisBoundOther.x <= _xAxisBoundThis.y && _xAxisBoundOther.y >= _xAxisBoundThis.x && _yAxisBoundOther.y >= _yAxisBoundThis.x && _yAxisBoundOther.x <= _yAxisBoundThis.y)
        //    {
        //        // Collision.
        //        return true;
        //    }
        //}

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

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxLocal(ref Vector2 _xBound, ref Vector2 _yBound)
    {
        // Rotate bound corners.
        // Find min/max extends for x and y.
        // return min and max extends into ref variables.

        // Get the bounds position for each box corner.
        Vector2 corner1 = new Vector2(_xBound.x, _yBound.x);
        Vector2 corner2 = new Vector2(_xBound.x, _yBound.y);
        Vector2 corner3 = new Vector2(_xBound.y, _yBound.y);
        Vector2 corner4 = new Vector2(_xBound.y, _yBound.x);

        // Transform the corners from local to world space.
        corner1 = WorldToLocal().MultiplyPoint3x4(corner1);
        corner2 = WorldToLocal().MultiplyPoint3x4(corner2);
        corner3 = WorldToLocal().MultiplyPoint3x4(corner3);
        corner4 = WorldToLocal().MultiplyPoint3x4(corner4);

        // Determine the minimum and maximum extends for each axis.
        float minimumX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maximumX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minimumY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maximumY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        // Create vector2 of new extends.
        Vector2 newBoundX = new Vector2(minimumX, maximumX);
        Vector2 newBoundY = new Vector2(minimumY, maximumY);

        // Assign referenced variables the new extend values.
        _xBound = newBoundX;
        _yBound = newBoundY;

        // Debug drawing data for extends in world space.
        Vector2 drawCorner1 = new Vector2(newBoundX.x, newBoundY.x);
        Vector2 drawCorner2 = new Vector2(newBoundX.x, newBoundY.y);
        Vector2 drawCorner3 = new Vector2(newBoundX.y, newBoundY.y);
        Vector2 drawCorner4 = new Vector2(newBoundX.y, newBoundY.x);

        // Draw the extends in world space.
        Debug.DrawLine(drawCorner1, drawCorner2, Color.red);
        Debug.DrawLine(drawCorner2, drawCorner3, Color.red);
        Debug.DrawLine(drawCorner3, drawCorner4, Color.red);
        Debug.DrawLine(drawCorner4, drawCorner1, Color.red);

        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxWorld(ref Vector2 _xBound, ref Vector2 _yBound)
    {
        // Rotate bound corners.
        // Find min/max extends for x and y.
        // return min and max extends into ref variables.

        // Get the bounds position for each box corner.
        Vector2 corner1 = new Vector2(_xBound.x, _yBound.x);
        Vector2 corner2 = new Vector2(_xBound.x, _yBound.y);
        Vector2 corner3 = new Vector2(_xBound.y, _yBound.y);
        Vector2 corner4 = new Vector2(_xBound.y, _yBound.x);

        // Transform the corners from local to world space.
        corner1 = LocalToWorld().MultiplyPoint3x4(corner1);
        corner2 = LocalToWorld().MultiplyPoint3x4(corner2);
        corner3 = LocalToWorld().MultiplyPoint3x4(corner3);
        corner4 = LocalToWorld().MultiplyPoint3x4(corner4);

        // Determine the minimum and maximum extends for each axis.
        float minimumX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maximumX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minimumY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maximumY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        // Create vector2 of new extends.
        Vector2 newBoundX = new Vector2(minimumX, maximumX);
        Vector2 newBoundY = new Vector2(minimumY, maximumY);

        // Assign referenced variables the new extend values.
        _xBound = newBoundX;
        _yBound = newBoundY;

        // Debug drawing data for extends in world space.
        Vector2 drawCorner1 = new Vector2(newBoundX.x, newBoundY.x);
        Vector2 drawCorner2 = new Vector2(newBoundX.x, newBoundY.y);
        Vector2 drawCorner3 = new Vector2(newBoundX.y, newBoundY.y);
        Vector2 drawCorner4 = new Vector2(newBoundX.y, newBoundY.x);

        // Draw the extends in world space.
        Debug.DrawLine(drawCorner1, drawCorner2, Color.red);
        Debug.DrawLine(drawCorner2, drawCorner3, Color.red);
        Debug.DrawLine(drawCorner3, drawCorner4, Color.red);
        Debug.DrawLine(drawCorner4, drawCorner1, Color.red);

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
        Matrix4x4 model = translate * rotate * translateInverse;

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
        Matrix4x4 model = translateInverse * rotateInverse * translate;

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
        Matrix4x4 matrix = boxHull.LocalToWorld();

        // Transform position to world space of the box hull.
        position = matrix.MultiplyPoint3x4(position);

        // Update bounding box axis.
        boxHull.CalculateBoundingBoxAxis();

        // Change gizmo drawing color.
        Handles.color = Color.blue;

        // Get the bounds position for each box edge.
        Vector3 corner1 = new Vector3(boxHull.X_AxisBound.x, boxHull.Y_AxisBound.x);
        Vector3 corner2 = new Vector3(boxHull.X_AxisBound.x, boxHull.Y_AxisBound.y);
        Vector3 corner3 = new Vector3(boxHull.X_AxisBound.y, boxHull.Y_AxisBound.y);
        Vector3 corner4 = new Vector3(boxHull.X_AxisBound.y, boxHull.Y_AxisBound.x);

        // Draw the axis for the bounds. (Rotated automatically by the new handles matrix. WorldToLocal)
        Handles.DrawLine(corner1, corner2);
        Handles.DrawLine(corner2, corner3);
        Handles.DrawLine(corner3, corner4);
        Handles.DrawLine(corner4, corner1);

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
