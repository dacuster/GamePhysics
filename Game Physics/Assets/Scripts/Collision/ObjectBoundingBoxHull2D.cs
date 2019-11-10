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
    private Vector2 boundingBox = Vector2.one;

    // Bounding axis for this object.
    private Vector2 xAxisBound;
    private Vector2 yAxisBound;

    new private void FixedUpdate()
    {
        // Update bounding box axis.
        CalculateBoundingBoxAxis();

        base.FixedUpdate();
    }

    // Check for collision OBB vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c)
    {
        // Perform check with circle. (Algorithm implemented there already).
        return other.TestCollisionVsOBB(this, ref c);
    }

    // Check for collision OBB vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c)
    {
        // Perform check with AABB. (Algorithm implemented there already).
        return other.TestCollisionVsOBB(this, ref c);
    }

    // Check for collision OBB vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c)
    {
        // AABB-OBB part 2 twice
        // 1. .....
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again

        // Other OBB bounding box axis.
        Vector2 xAxisBoundOther = other.X_AxisBound;
        Vector2 yAxisBoundOther = other.Y_AxisBound;

        // Calculate the max extents of the other OBB in this OBB's local space.
        WorldToLocal(other.LocalToWorld(), ref xAxisBoundOther, ref yAxisBoundOther);

        // Check if the extents of the OBBs are colliding in this OBB's local space.
        if (X_AxisBound.x <= xAxisBoundOther.y && X_AxisBound.y >= xAxisBoundOther.x && Y_AxisBound.y >= yAxisBoundOther.x && Y_AxisBound.x <= yAxisBoundOther.y)
        {
            // This OBB bounding box axis.
            Vector2 xAxisBoundThis = X_AxisBound;
            Vector2 yAxisBoundThis = Y_AxisBound;

            // Calculate the max extents of this OBB in the other OBB's local space.
            other.WorldToLocal(LocalToWorld(), ref xAxisBoundThis, ref yAxisBoundThis);

            // Check if the extents of the OBBs are colliding in the other OBB's local space.
            if (other.X_AxisBound.x <= xAxisBoundThis.y && other.X_AxisBound.y >= xAxisBoundThis.x && other.Y_AxisBound.y >= yAxisBoundThis.x && other.Y_AxisBound.x <= yAxisBoundThis.y)
            {
                return true;
            }
        }

        return false;
    }

    // Calculate the bounding box axis.
    public void CalculateBoundingBoxAxis()
    {
        // Get the particle for use when game is not running.
        xAxisBound.x = -BoundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + BoundingBox.x;
        yAxisBound.x = -BoundingBox.y * 0.5f;
        yAxisBound.y = yAxisBound.x + BoundingBox.y;

        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxLocal(ref Vector2 _xAxisBound, ref Vector2 _yAxisBound)
    {
        // Rotate bound corners.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Get the bounds position for each box corner.
        Vector2 vertex0 = new Vector2(_xAxisBound.x, _yAxisBound.x);
        Vector2 vertex1 = new Vector2(_xAxisBound.x, _yAxisBound.y);
        Vector2 vertex2 = new Vector2(_xAxisBound.y, _yAxisBound.y);
        Vector2 vertex3 = new Vector2(_xAxisBound.y, _yAxisBound.x);

        // Transform the corners from local to world space.
        vertex0 = WorldToLocal().MultiplyPoint3x4(vertex0);
        vertex1 = WorldToLocal().MultiplyPoint3x4(vertex1);
        vertex2 = WorldToLocal().MultiplyPoint3x4(vertex2);
        vertex3 = WorldToLocal().MultiplyPoint3x4(vertex3);

        if (debugMode)
        {
            // Draw the extents in world space.
            Debug.DrawLine(vertex0, vertex1, Color.white);
            Debug.DrawLine(vertex1, vertex2, Color.white);
            Debug.DrawLine(vertex2, vertex3, Color.white);
            Debug.DrawLine(vertex3, vertex0, Color.white);
        }

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(vertex0.x, vertex1.x, vertex2.x, vertex3.x);
        float maximumX = Mathf.Max(vertex0.x, vertex1.x, vertex2.x, vertex3.x);
        float minimumY = Mathf.Min(vertex0.y, vertex1.y, vertex2.y, vertex3.y);
        float maximumY = Mathf.Max(vertex0.y, vertex1.y, vertex2.y, vertex3.y);

        // Set new max extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);

        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxWorld(ref Vector2 _xAxisBound, ref Vector2 _yAxisBound)
    {
        // Rotate bound corners.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Get the bounds position for each box corner .
        Vector2 vertex0 = new Vector2(_xAxisBound.x, _yAxisBound.x);
        Vector2 vertex1 = new Vector2(_xAxisBound.x, _yAxisBound.y);
        Vector2 vertex2 = new Vector2(_xAxisBound.y, _yAxisBound.y);
        Vector2 vertex3 = new Vector2(_xAxisBound.y, _yAxisBound.x);

        // Transform the corners from local to world space.
        vertex0 = LocalToWorld().MultiplyPoint3x4(vertex0);
        vertex1 = LocalToWorld().MultiplyPoint3x4(vertex1);
        vertex2 = LocalToWorld().MultiplyPoint3x4(vertex2);
        vertex3 = LocalToWorld().MultiplyPoint3x4(vertex3);

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(vertex0.x, vertex1.x, vertex2.x, vertex3.x);
        float maximumX = Mathf.Max(vertex0.x, vertex1.x, vertex2.x, vertex3.x);
        float minimumY = Mathf.Min(vertex0.y, vertex1.y, vertex2.y, vertex3.y);
        float maximumY = Mathf.Max(vertex0.y, vertex1.y, vertex2.y, vertex3.y);

        // Create vector2 of new extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);

        if (debugMode)
        {
            // Debug drawing data for extents in world space.
            Vector2 drawVertex0 = new Vector2(_xAxisBound.x, _yAxisBound.x);
            Vector2 drawVertex1 = new Vector2(_xAxisBound.x, _yAxisBound.y);
            Vector2 drawVertex2 = new Vector2(_xAxisBound.y, _yAxisBound.y);
            Vector2 drawVertex3 = new Vector2(_xAxisBound.y, _yAxisBound.x);

            // Draw the extents in world space.
            Debug.DrawLine(drawVertex0, drawVertex1, Color.magenta);
            Debug.DrawLine(drawVertex1, drawVertex2, Color.magenta);
            Debug.DrawLine(drawVertex2, drawVertex3, Color.magenta);
            Debug.DrawLine(drawVertex3, drawVertex0, Color.magenta);
        }

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

    // Get the local to world transformation of this object.
    public Matrix4x4 LocalToWorld()
    {
        // Particle needed for scene editor when game isn't running.
        Particle2D particle = GetComponent<Particle2D>();

        // Get the position of this particle.
        Vector3 position = particle.Position;

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

        return model;
    }

    // Transform the given position into world space of this OBB.
    public void LocalToWorld(ref Vector2 position)
    {
        // Transform position into world space of this OBB.
        position = LocalToWorld().MultiplyPoint3x4(position);

        return;
    }

    // Get the max extents of the given axis bounds in local space of this object.
    // Passes in the world transformation matrix of the other object.
    public void WorldToLocal(Matrix4x4 otherWorld, ref Vector2 _xAxisBound, ref Vector2 _yAxisBound)
    {
        // Rotate bound vertices.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Get the bounds position for each box vertex.
        Vector2 vertex0 = new Vector2(_xAxisBound.x, _yAxisBound.x);
        Vector2 vertex1 = new Vector2(_xAxisBound.x, _yAxisBound.y);
        Vector2 vertex2 = new Vector2(_xAxisBound.y, _yAxisBound.y);
        Vector2 vertex3 = new Vector2(_xAxisBound.y, _yAxisBound.x);

        // Move the vertices of the other bounding box into the world space of the other object.
        vertex0 = otherWorld.MultiplyPoint3x4(vertex0);
        vertex1 = otherWorld.MultiplyPoint3x4(vertex1);
        vertex2 = otherWorld.MultiplyPoint3x4(vertex2);
        vertex3 = otherWorld.MultiplyPoint3x4(vertex3);

        // Transform the vertices from world space into the local space of this object.
        vertex0 = WorldToLocal().MultiplyPoint3x4(vertex0);
        vertex1 = WorldToLocal().MultiplyPoint3x4(vertex1);
        vertex2 = WorldToLocal().MultiplyPoint3x4(vertex2);
        vertex3 = WorldToLocal().MultiplyPoint3x4(vertex3);

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(vertex0.x, vertex1.x, vertex2.x, vertex3.x);
        float maximumX = Mathf.Max(vertex0.x, vertex1.x, vertex2.x, vertex3.x);
        float minimumY = Mathf.Min(vertex0.y, vertex1.y, vertex2.y, vertex3.y);
        float maximumY = Mathf.Max(vertex0.y, vertex1.y, vertex2.y, vertex3.y);

        // Set new max extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);

        return;
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

        // Get the local scale of this object.
        Vector3 localScale = particle.transform.localScale;

        // Translation matrix of this particle.
        Matrix4x4 translate = Matrix4x4.Translate(position);

        // Invert the translation.
        Matrix4x4 translateInverse = translate.inverse;

        // Rotation matrix of this particle.
        Matrix4x4 rotate = Matrix4x4.Rotate(rotation);

        // Invert the rotation.
        Matrix4x4 rotateInverse = rotate.inverse;

        Matrix4x4 scale = Matrix4x4.Scale(localScale);

        Matrix4x4 scaleInverse = scale.inverse;

        // Model matrix to convert from local to world space.
        Matrix4x4 model = scaleInverse * rotateInverse * translateInverse;
        //Matrix4x4 model = translateInverse * rotateInverse * scaleInverse;

        return model;
    }

    // Transform the given position into local space of this OBB.
    public void WorldToLocal(ref Vector2 position)
    {
        // Transform position into local space of this OBB.
        position = WorldToLocal().MultiplyPoint3x4(position);

        return;
    }
}

[CustomEditor(typeof(ObjectBoundingBoxHull2D))]
public class ObjectBoxEditor2D : Editor
{
    private void OnSceneGUI()
    {
        // Get the box hull associated with this object.
        ObjectBoundingBoxHull2D boxHull = (ObjectBoundingBoxHull2D)target;

        // Update bounding box axis.
        boxHull.CalculateBoundingBoxAxis();

        // Get the x and y bounding box axis.
        Vector2 xAxisBound = boxHull.X_AxisBound;
        Vector2 yAxisBound = boxHull.Y_AxisBound;

        // Draw the box collider in the scene view in local space.
        DrawVertices(xAxisBound, yAxisBound, Color.blue);

        // Translate the bounding box into world space.
        boxHull.CalculateBoundingBoxWorld(ref xAxisBound, ref yAxisBound);
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    // Draw a box with the given xAxis and yAxis and drawing color.
    private void DrawVertices(Vector2 xAxisBound, Vector2 yAxisBound, Color color)
    {
        // Save handles drawing color for reseting.
        Color savedColor = Handles.color;

        // Set the new drawing color.
        Handles.color = color;

        // Set each vertex based on the bounding axis.
        Vector3 vertex0 = new Vector3(xAxisBound.x, yAxisBound.x);
        Vector3 vertex1 = new Vector3(xAxisBound.x, yAxisBound.y);
        Vector3 vertex2 = new Vector3(xAxisBound.y, yAxisBound.y);
        Vector3 vertex3 = new Vector3(xAxisBound.y, yAxisBound.x);

        // Draw the bounding box to the screen.
        Handles.DrawLine(vertex0, vertex1);
        Handles.DrawLine(vertex1, vertex2);
        Handles.DrawLine(vertex2, vertex3);
        Handles.DrawLine(vertex3, vertex0);

        // Reset the drawing color.
        Handles.color = savedColor;

        return;
    }
}
