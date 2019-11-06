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

    new private void FixedUpdate()
    {
        // Update bounding box axis.
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

        if (name == "Cube")
        {
            return false;
        }

        // Other OBB bounding box axis.
        Vector2 xAxisBoundOther = other.X_AxisBound;
        Vector2 yAxisBoundOther = other.Y_AxisBound;

        // Calculate the max extents of the other OBB in this OBB's local space.
        WorldToLocal(other.LocalToWorld(), ref xAxisBoundOther, ref yAxisBoundOther);

        // Check if the extents of the OBBs are colliding in this OBB's local space.
        if (X_AxisBound.x <= xAxisBoundOther.y && X_AxisBound.y >= xAxisBoundOther.x && Y_AxisBound.y >= yAxisBoundOther.x && Y_AxisBound.x <= yAxisBoundOther.y)
        {
            Debug.Log("First!");
            // This OBB bounding box axis.
            Vector2 xAxisBoundThis = X_AxisBound;
            Vector2 yAxisBoundThis = Y_AxisBound;

            // Calculate the max extents of this OBB in the other OBB's local space.
            other.WorldToLocal(LocalToWorld(), ref xAxisBoundThis, ref yAxisBoundThis);

            if (other.X_AxisBound.x <= xAxisBoundThis.y && other.X_AxisBound.y >= xAxisBoundThis.x && other.Y_AxisBound.y >= yAxisBoundThis.x && other.Y_AxisBound.x <= yAxisBoundThis.y)
            // Check if the extents of the OBBs are colliding in the other OBB's local space.
            //if (xAxisBoundThis.x <= other.X_AxisBound.y && xAxisBoundThis.y >= other.X_AxisBound.x && yAxisBoundThis.y >= other.Y_AxisBound.x && yAxisBoundThis.x <= other.Y_AxisBound.y)
            {
                // Collision.
                Debug.Log("Collision!!!");
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
        Vector2 corner1 = new Vector2(_xAxisBound.x, _yAxisBound.x);
        Vector2 corner2 = new Vector2(_xAxisBound.x, _yAxisBound.y);
        Vector2 corner3 = new Vector2(_xAxisBound.y, _yAxisBound.y);
        Vector2 corner4 = new Vector2(_xAxisBound.y, _yAxisBound.x);

        // Transform the corners from local to world space.
        corner1 = WorldToLocal().MultiplyPoint3x4(corner1);
        corner2 = WorldToLocal().MultiplyPoint3x4(corner2);
        corner3 = WorldToLocal().MultiplyPoint3x4(corner3);
        corner4 = WorldToLocal().MultiplyPoint3x4(corner4);

        if (debugMode)
        {
            // Draw the extents in world space.
            Debug.DrawLine(corner1, corner2, Color.white);
            Debug.DrawLine(corner2, corner3, Color.white);
            Debug.DrawLine(corner3, corner4, Color.white);
            Debug.DrawLine(corner4, corner1, Color.white);
        }

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maximumX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minimumY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maximumY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        // Set new max extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);

        if (debugMode)
        {
            //// Debug drawing data for extents in world space.
            //Vector2 drawCorner1 = new Vector2(_xAxisBound.x, _yAxisBound.x);
            //Vector2 drawCorner2 = new Vector2(_xAxisBound.x, _yAxisBound.y);
            //Vector2 drawCorner3 = new Vector2(_xAxisBound.y, _yAxisBound.y);
            //Vector2 drawCorner4 = new Vector2(_xAxisBound.y, _yAxisBound.x);

            //// Draw the extents in world space.
            //Debug.DrawLine(drawCorner1, drawCorner2, Color.white);
            //Debug.DrawLine(drawCorner2, drawCorner3, Color.white);
            //Debug.DrawLine(drawCorner3, drawCorner4, Color.white);
            //Debug.DrawLine(drawCorner4, drawCorner1, Color.white);
        }

        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxWorld(ref Vector2 _xAxisBound, ref Vector2 _yAxisBound)
    {
        // Rotate bound corners.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Get the bounds position for each box corner .
        Vector2 corner1 = new Vector2(_xAxisBound.x, _yAxisBound.x);
        Vector2 corner2 = new Vector2(_xAxisBound.x, _yAxisBound.y);
        Vector2 corner3 = new Vector2(_xAxisBound.y, _yAxisBound.y);
        Vector2 corner4 = new Vector2(_xAxisBound.y, _yAxisBound.x);

        // Transform the corners from local to world space.
        corner1 = LocalToWorld().MultiplyPoint3x4(corner1);
        corner2 = LocalToWorld().MultiplyPoint3x4(corner2);
        corner3 = LocalToWorld().MultiplyPoint3x4(corner3);
        corner4 = LocalToWorld().MultiplyPoint3x4(corner4);

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maximumX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minimumY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maximumY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        // Create vector2 of new extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);

        if (debugMode)
        {
            // Debug drawing data for extents in world space.
            Vector2 drawCorner1 = new Vector2(_xAxisBound.x, _yAxisBound.x);
            Vector2 drawCorner2 = new Vector2(_xAxisBound.x, _yAxisBound.y);
            Vector2 drawCorner3 = new Vector2(_xAxisBound.y, _yAxisBound.y);
            Vector2 drawCorner4 = new Vector2(_xAxisBound.y, _yAxisBound.x);

            // Draw the extents in world space.
            Debug.DrawLine(drawCorner1, drawCorner2, Color.magenta);
            Debug.DrawLine(drawCorner2, drawCorner3, Color.magenta);
            Debug.DrawLine(drawCorner3, drawCorner4, Color.magenta);
            Debug.DrawLine(drawCorner4, drawCorner1, Color.magenta);
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

        if (debugMode)
        {
            //// Debug drawing data for extents in world space.
            //Vector2 drawCorner1 = new Vector2(_xAxisBound.x, _yAxisBound.x);
            //Vector2 drawCorner2 = new Vector2(_xAxisBound.x, _yAxisBound.y);
            //Vector2 drawCorner3 = new Vector2(_xAxisBound.y, _yAxisBound.y);
            //Vector2 drawCorner4 = new Vector2(_xAxisBound.y, _yAxisBound.x);

            //// Draw the extents in world space.
            //Debug.DrawLine(drawCorner1, drawCorner2, Color.white);
            //Debug.DrawLine(drawCorner2, drawCorner3, Color.white);
            //Debug.DrawLine(drawCorner3, drawCorner4, Color.white);
            //Debug.DrawLine(drawCorner4, drawCorner1, Color.white);
        }

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
public class ObjectBoxEditor : Editor
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

        //DrawVertices(xAxisBound, yAxisBound, Color.green);
        

        //// Create a color.
        //Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        //// Change gizmo drawing color.
        //Handles.color = purple;

        //// Change the gizmo drawing matrix.
        //Handles.matrix = boxHull.LocalToWorld();

        //// Draw a cube to represent the collider on this object.
        //Handles.DrawWireCube(Vector3.zero, boxHull.BoundingBox);
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
