using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AxisAlignedBoundingBoxHull3D : CollisionHull3D
{
    // Constructor.
    public AxisAlignedBoundingBoxHull3D() : base(CollisionHullType3D.hull_aabb) { }

    [SerializeField]
    // Bounding box for this object.
    private Vector3 boundingBox = Vector3.one;

    // Bounding axis for this object.
    private Vector2 xAxisBound;
    private Vector2 yAxisBound;
    private Vector2 zAxisBound;

    new private void FixedUpdate()
    {
        // Update the bounding box.
        CalculateBoundingBoxAxis();

        base.FixedUpdate();
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

        // Get the axis bounds of this AABB.
        Vector2 xAxisBoundThis = X_AxisBound;
        Vector2 yAxisBoundThis = Y_AxisBound;
        Vector2 zAxisBoundThis = Z_AxisBound;

        // Transform the axis bounds into world space.
        WorldBoundingBoxAxis(ref xAxisBoundThis, ref yAxisBoundThis, ref zAxisBoundThis);

        // Get the axis bounds of the other AABB.
        Vector2 xAxisBoundOther = other.X_AxisBound;
        Vector2 yAxisBoundOther = other.Y_AxisBound;
        Vector2 zAxisBoundOther = other.Z_AxisBound;

        // Transform the axis bounds into world space.
        other.WorldBoundingBoxAxis(ref xAxisBoundOther, ref yAxisBoundOther, ref zAxisBoundOther);


        // Compare the minimum and maximum extents of the bounding box for this object and the other object.
        if (xAxisBoundThis.x <= xAxisBoundOther.y && xAxisBoundThis.y >= xAxisBoundOther.x && yAxisBoundThis.y >= yAxisBoundOther.x && yAxisBoundThis.x <= yAxisBoundOther.y && zAxisBoundThis.x <= zAxisBoundOther.y && zAxisBoundThis.y >= zAxisBoundOther.x)
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

        // OBB max extents
        Vector2 xAxisBoundOBB = other.X_AxisBound;
        Vector2 yAxisBoundOBB = other.Y_AxisBound;
        Vector2 zAxisBoundOBB = other.Z_AxisBound;

        // Transform the extents to OBB world space.
        other.CalculateBoundingBoxWorld(ref xAxisBoundOBB, ref yAxisBoundOBB, ref zAxisBoundOBB);

        // Get the AABB bounding axis.
        Vector2 xAxisBoundAABB = X_AxisBound;
        Vector2 yAxisBoundAABB = Y_AxisBound;
        Vector2 zAxisBoundAABB = Z_AxisBound;

        // Transform the bounding box to world space.
        WorldBoundingBoxAxis(ref xAxisBoundAABB, ref yAxisBoundAABB, ref zAxisBoundAABB);

        // Check if the extents for the AABB are colliding with the extents for the OBB in world space.
        if (xAxisBoundAABB.x <= xAxisBoundOBB.y && xAxisBoundAABB.y >= xAxisBoundOBB.x && yAxisBoundAABB.y >= yAxisBoundOBB.x && yAxisBoundAABB.x <= yAxisBoundOBB.y && zAxisBoundAABB.x <= zAxisBoundOBB.y && zAxisBoundAABB.y >= zAxisBoundOBB.x)
        {
            // Transform AABB entents to OBB local space.
            other.CalculateBoundingBoxLocal(ref xAxisBoundAABB, ref yAxisBoundAABB, ref zAxisBoundAABB);

            // Check if the extents for the AABB in OBB local space are colliding with the extents for the OBB in OBB world space.
            if (xAxisBoundAABB.x <= other.X_AxisBound.y && xAxisBoundAABB.y >= other.X_AxisBound.x && yAxisBoundAABB.y >= other.Y_AxisBound.x && yAxisBoundAABB.x <= other.Y_AxisBound.y && zAxisBoundAABB.x <= other.Z_AxisBound.y && zAxisBoundAABB.y >= other.Z_AxisBound.x)
            {
                // Collision.
                return true;
            }
        }

        return false;
    }

    // Transform given axis into world space from local space.
    private Matrix4x4 LocalToWorld()
    {
        // Particle needed for scene editor when game isn't running.
        Particle3D particle = GetComponent<Particle3D>();

        // Get the position of this particle.
        Vector3 position = particle.Position;

        // Build a quaternion based on the euler rotation of this particle.
        Quaternion rotation = particle.Rotation.GetUnityQuaternion();

        Vector3 localScale = particle.transform.localScale;

        // Translation matrix of this particle.
        Matrix4x4 translate = Matrix4x4.Translate(position);

        // Rotation matrix of this particle.
        Matrix4x4 rotate = Matrix4x4.Rotate(rotation);

        // Scale matrix of this particle.
        Matrix4x4 scale = Matrix4x4.Scale(localScale);

        // Model matrix to convert from local to world space.
        Matrix4x4 model = translate * rotate * scale;

        // Return the model matrix.
        return model;
    }

    // Calculate the bounding box limits in local to world space.
    public void WorldBoundingBoxAxis(ref Vector2 _xAxisBound, ref Vector2 _yAxisBound, ref Vector2 _zAxisBound)
    {
        // Rotate bound corners.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Cube has 8 vertices.
        Vector3[] vertex = new Vector3[8];

        // Transform the axis into world space.
        TransformBoundingBoxAxis(LocalToWorld(), ref _xAxisBound, ref _yAxisBound, ref _zAxisBound);

        if (debugMode)
        {
            DrawCube(_xAxisBound, _yAxisBound, zAxisBound, Color.magenta);
        }

        return;
    }

    private void CalculateVertices(ref Vector3[] vertex, Vector2 _xAxisBound, Vector2 _yAxisBound, Vector2 _zAxisBound)
    {
        // Get the bounds position for each box corner .
        vertex[0] = new Vector3(_xAxisBound.x, _yAxisBound.x, _zAxisBound.x);
        vertex[1] = new Vector3(_xAxisBound.x, _yAxisBound.y, _zAxisBound.x);
        vertex[2] = new Vector3(_xAxisBound.y, _yAxisBound.y, _zAxisBound.x);
        vertex[3] = new Vector3(_xAxisBound.y, _yAxisBound.x, _zAxisBound.x);
        vertex[4] = new Vector3(_xAxisBound.x, _yAxisBound.x, _zAxisBound.y);
        vertex[5] = new Vector3(_xAxisBound.x, _yAxisBound.y, _zAxisBound.y);
        vertex[6] = new Vector3(_xAxisBound.y, _yAxisBound.y, _zAxisBound.y);
        vertex[7] = new Vector3(_xAxisBound.y, _yAxisBound.x, _zAxisBound.y);

        return;
    }

    // Transform the given bounding box axis by the given transformation.
    public void TransformBoundingBoxAxis(Matrix4x4 transformation, ref Vector2 _xAxisBound, ref Vector2 _yAxisBound, ref Vector2 _zAxisBound)
    {
        // 8 Vertices in the cube.
        Vector3[] vertex = new Vector3[8];

        // Calculate the vertices' positions.
        CalculateVertices(ref vertex, _xAxisBound, _yAxisBound, _zAxisBound);

        // Transform the vertices into the given transformation space.
        vertex[0] = transformation.MultiplyPoint3x4(vertex[0]);
        vertex[1] = transformation.MultiplyPoint3x4(vertex[1]);
        vertex[2] = transformation.MultiplyPoint3x4(vertex[2]);
        vertex[3] = transformation.MultiplyPoint3x4(vertex[3]);
        vertex[4] = transformation.MultiplyPoint3x4(vertex[4]);
        vertex[5] = transformation.MultiplyPoint3x4(vertex[5]);
        vertex[6] = transformation.MultiplyPoint3x4(vertex[6]);
        vertex[7] = transformation.MultiplyPoint3x4(vertex[7]);

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(vertex[0].x, vertex[1].x, vertex[2].x, vertex[3].x, vertex[4].x, vertex[5].x, vertex[6].x, vertex[7].x);
        float maximumX = Mathf.Max(vertex[0].x, vertex[1].x, vertex[2].x, vertex[3].x, vertex[4].x, vertex[5].x, vertex[6].x, vertex[7].x);
        float minimumY = Mathf.Min(vertex[0].y, vertex[1].y, vertex[2].y, vertex[3].y, vertex[4].y, vertex[5].y, vertex[6].y, vertex[7].y);
        float maximumY = Mathf.Max(vertex[0].y, vertex[1].y, vertex[2].y, vertex[3].y, vertex[4].y, vertex[5].y, vertex[6].y, vertex[7].y);
        float minimumZ = Mathf.Min(vertex[0].z, vertex[1].z, vertex[2].z, vertex[3].z, vertex[4].z, vertex[5].z, vertex[6].z, vertex[7].z);
        float maximumZ = Mathf.Max(vertex[0].z, vertex[1].z, vertex[2].z, vertex[3].z, vertex[4].z, vertex[5].z, vertex[6].z, vertex[7].z);

        // Create Vector3 of new extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);
        _zAxisBound = new Vector2(minimumZ, maximumZ);

        return;
    }

    private void DrawCube(Vector2 _xAxisBound, Vector2 _yAxisBound, Vector2 _zAxisBound, Color color)
    {
        // 8 Vertices in a cube.
        Vector3[] vertex = new Vector3[8];

        // Calculate all the vertices' positions.
        CalculateVertices(ref vertex, _xAxisBound, _yAxisBound, _zAxisBound);

        // Draw the bounding box.
        Debug.DrawLine(vertex[0], vertex[1], color);
        Debug.DrawLine(vertex[1], vertex[2], color);
        Debug.DrawLine(vertex[2], vertex[3], color);
        Debug.DrawLine(vertex[3], vertex[0], color);
        Debug.DrawLine(vertex[4], vertex[5], color);
        Debug.DrawLine(vertex[5], vertex[6], color);
        Debug.DrawLine(vertex[6], vertex[7], color);
        Debug.DrawLine(vertex[7], vertex[4], color);
        Debug.DrawLine(vertex[0], vertex[4], color);
        Debug.DrawLine(vertex[1], vertex[5], color);
        Debug.DrawLine(vertex[2], vertex[6], color);
        Debug.DrawLine(vertex[3], vertex[7], color);

        return;
    }

    // Calculate the bounding box axis.
    public void CalculateBoundingBoxAxis()
    {
        xAxisBound.x = -boundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + boundingBox.x;
        yAxisBound.x = -boundingBox.y * 0.5f;
        yAxisBound.y = yAxisBound.x + boundingBox.y;
        zAxisBound.x = -boundingBox.z * 0.5f;
        zAxisBound.y = zAxisBound.x + boundingBox.z;

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

    // Accessor for the bounding box z-axis.
    public Vector2 Z_AxisBound
    {
        get
        {
            return zAxisBound;
        }
    }

    // Accessor for the bounding box.
    public Vector3 BoundingBox
    {
        get
        {
            return boundingBox;
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

        // Draw a cube to represent the collider on this object.
        Handles.DrawWireCube(particle.Position, boxHull.BoundingBox);
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}