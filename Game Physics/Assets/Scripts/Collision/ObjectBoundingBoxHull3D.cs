using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectBoundingBoxHull3D : CollisionHull3D
{
    // Constructor.
    public ObjectBoundingBoxHull3D() : base(CollisionHullType3D.hull_obb) { }

    [SerializeField]
    // Bounding box for this object.
    private Vector3 boundingBox = Vector3.one;

    // Bounding axis for this object.
    private Vector2 xAxisBound;
    private Vector2 yAxisBound;
    private Vector2 zAxisBound;

    new private void FixedUpdate()
    {
        // Update bounding box axis.
        CalculateBoundingBoxAxis();

        base.FixedUpdate();
    }

    // Check for collision OBB vs circle.
    public override bool TestCollisionVsCircle(CircleCollisionHull3D other, ref Collision collision)
    {
        // Perform check with circle. (Algorithm implemented there already).
        return other.TestCollisionVsOBB(this, ref collision);
    }

    // Check for collision OBB vs AABB.
    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D other, ref Collision collision)
    {
        // Perform check with AABB. (Algorithm implemented there already).
        return other.TestCollisionVsOBB(this, ref collision);
    }

    // Check for collision OBB vs OBB.
    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D other, ref Collision collision)
    {
        // AABB-OBB part 2 twice
        // 1. .....
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again

        // Other OBB bounding box axis.
        Vector2 xAxisBoundOther = other.X_AxisBound;
        Vector2 yAxisBoundOther = other.Y_AxisBound;
        Vector2 zAxisBoundOther = other.Z_AxisBound;

        // Calculate the max extents of the other OBB in this OBB's local space.
        WorldToLocal(other.LocalToWorld(), ref xAxisBoundOther, ref yAxisBoundOther, ref zAxisBoundOther);

        // Check if the extents of the OBBs are colliding in this OBB's local space.
        if (X_AxisBound.x <= xAxisBoundOther.y && X_AxisBound.y >= xAxisBoundOther.x && Y_AxisBound.y >= yAxisBoundOther.x && Y_AxisBound.x <= yAxisBoundOther.y && Z_AxisBound.x <= zAxisBoundOther.y && Z_AxisBound.y >= zAxisBoundOther.x)
        {
            // This OBB bounding box axis.
            Vector2 xAxisBoundThis = X_AxisBound;
            Vector2 yAxisBoundThis = Y_AxisBound;
            Vector2 zAxisBoundThis = Z_AxisBound;

            // Calculate the max extents of this OBB in the other OBB's local space.
            other.WorldToLocal(LocalToWorld(), ref xAxisBoundThis, ref yAxisBoundThis, ref zAxisBoundThis);

            collision.Status = other.X_AxisBound.x <= xAxisBoundThis.y
                            && other.X_AxisBound.y >= xAxisBoundThis.x
                            && other.Y_AxisBound.y >= yAxisBoundThis.x
                            && other.Y_AxisBound.x <= yAxisBoundThis.y
                            && other.Z_AxisBound.x <= zAxisBoundThis.y
                            && other.Z_AxisBound.y >= zAxisBoundThis.x;

            // Collision
            if (collision.Status)
            {
                collision.A = this;
                collision.B = other;
            }
        }

        return collision.Status;
    }

    // Calculate the bounding box axis.
    public void CalculateBoundingBoxAxis()
    {
        // Get the particle for use when game is not running.
        xAxisBound.x = -BoundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + BoundingBox.x;
        yAxisBound.x = -BoundingBox.y * 0.5f;
        yAxisBound.y = yAxisBound.x + BoundingBox.y;
        zAxisBound.x = -BoundingBox.z * 0.5f;
        zAxisBound.y = zAxisBound.x + BoundingBox.z;

        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxLocal(ref Vector2 _xAxisBound, ref Vector2 _yAxisBound, ref Vector2 _zAxisBound)
    {
        TransformBoundingBoxAxis(WorldToLocal(), ref _xAxisBound, ref _yAxisBound, ref _zAxisBound);

        if (debugMode)
        {
            DrawCube(_xAxisBound, _yAxisBound, _zAxisBound, Color.white);
        }

        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxWorld(ref Vector2 _xAxisBound, ref Vector2 _yAxisBound, ref Vector2 _zAxisBound)
    {
		// Rotate bound corners.
		// Find min/max extents for x and y.
		// return min and max extents into ref variables.

		if (debugMode)
		{
			//DrawCube(_xAxisBound, _yAxisBound, _zAxisBound, Color.magenta);
		}
		TransformBoundingBoxAxis(LocalToWorld(), ref _xAxisBound, ref _yAxisBound, ref _zAxisBound);

        if (debugMode)
        {
            DrawCube(_xAxisBound, _yAxisBound, _zAxisBound, Color.magenta);
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

    // Get the local to world transformation of this object.
    public Matrix4x4 LocalToWorld()
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

        return model;
    }

    // Transform the given position into world space of this OBB.
    public void LocalToWorld(ref Vector3 position)
    {
        // Transform position into world space of this OBB.
        position = LocalToWorld().MultiplyPoint3x4(position);

        return;
    }

    // Get the max extents of the given axis bounds in local space of this object.
    // Passes in the world transformation matrix of the other object.
    public void WorldToLocal(Matrix4x4 otherWorld, ref Vector2 _xAxisBound, ref Vector2 _yAxisBound, ref Vector2 _zAxisBound)
    {
        // Rotate bound vertices.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Cube has 8 vertices.
        Vector3[] vertex = new Vector3[8];

        // Calculate the vertices' positions.
        CalculateVertices(ref vertex, _xAxisBound, _yAxisBound, _zAxisBound);

        for (int currentVertex = 0; currentVertex < vertex.Length; currentVertex++)
        {
            // Transform vertex into other object's world space.
            vertex[currentVertex] = otherWorld.MultiplyPoint3x4(vertex[currentVertex]);
            // Transform vertex into this object's local space.
            vertex[currentVertex] = WorldToLocal().MultiplyPoint3x4(vertex[currentVertex]);
        }

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(vertex[0].x, vertex[1].x, vertex[2].x, vertex[3].x, vertex[4].x, vertex[5].x, vertex[6].x, vertex[7].x);
        float maximumX = Mathf.Max(vertex[0].x, vertex[1].x, vertex[2].x, vertex[3].x, vertex[4].x, vertex[5].x, vertex[6].x, vertex[7].x);
        float minimumY = Mathf.Min(vertex[0].y, vertex[1].y, vertex[2].y, vertex[3].y, vertex[4].y, vertex[5].y, vertex[6].y, vertex[7].y);
        float maximumY = Mathf.Max(vertex[0].y, vertex[1].y, vertex[2].y, vertex[3].y, vertex[4].y, vertex[5].y, vertex[6].y, vertex[7].y);
        float minimumZ = Mathf.Min(vertex[0].z, vertex[1].z, vertex[2].z, vertex[3].z, vertex[4].z, vertex[5].z, vertex[6].z, vertex[7].z);
        float maximumZ = Mathf.Max(vertex[0].z, vertex[1].z, vertex[2].z, vertex[3].z, vertex[4].z, vertex[5].z, vertex[6].z, vertex[7].z);

        // Set new max extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);
        _zAxisBound = new Vector2(minimumZ, maximumZ);

        return;
    }

    // Get the world to local transformation.
    public Matrix4x4 WorldToLocal()
    {
        // Particle needed for scene editor when game isn't running.
        Particle3D particle = GetComponent<Particle3D>();

        // Get the position of this particle.
        Vector3 position = particle.Position;

        // Build a quaternion based on the euler rotation of this particle.
        Quaternion rotation = particle.Rotation.GetUnityQuaternion();

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

        return model;
    }

    // Transform the given position into local space of this OBB.
    public void WorldToLocal(ref Vector3 position)
    {
        // Transform position into local space of this OBB.
        position = WorldToLocal().MultiplyPoint3x4(position);

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
}

[CustomEditor(typeof(ObjectBoundingBoxHull3D))]
public class ObjectBoxEditor3D : Editor
{
    private void OnSceneGUI()
    {
        // Get the box hull associated with this object.
        ObjectBoundingBoxHull3D boxHull = (ObjectBoundingBoxHull3D)target;

        // Update bounding box axis.
        boxHull.CalculateBoundingBoxAxis();

        // Get the x and y bounding box axis.
        Vector2 xAxisBound = boxHull.X_AxisBound;
        Vector2 yAxisBound = boxHull.Y_AxisBound;
        Vector2 zAxisBound = boxHull.Z_AxisBound;

        // Draw the box collider in the scene view in local space.
        DrawVertices(xAxisBound, yAxisBound, Color.blue);

        // Translate the bounding box into world space.
        boxHull.CalculateBoundingBoxWorld(ref xAxisBound, ref yAxisBound, ref zAxisBound);
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