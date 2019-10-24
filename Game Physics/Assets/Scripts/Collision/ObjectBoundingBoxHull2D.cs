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

    // Bounding axis for max extents.
    private Vector2 xAxisMaxBound;
    private Vector2 yAxisMaxBound;

    private void FixedUpdate()
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

        //// This OBB max extents.
        //Vector2 _xAxisBoundThis = X_AxisBound;
        //Vector2 _yAxisBoundThis = Y_AxisBound;

        //// Transform the entents to OBB local space.
        //other.CalculateBoundingBoxLocal(ref _xAxisBoundThis, ref _yAxisBoundThis);

        //// Other OBB max extents.
        //Vector2 _xAxisBoundOther = other.X_AxisBound;
        //Vector2 _yAxisBoundOther = other.Y_AxisBound;

        //// Transform the entents to this OBB world space.
        //CalculateBoundingBoxLocal(ref _xAxisBoundOther, ref _yAxisBoundOther);

        //// Check if the extents for the AABB are colliding with the extents for the OBB in OBB world space.
        //if (_xAxisBoundOther.x <= _xAxisBoundThis.y && _xAxisBoundOther.y >= _xAxisBoundThis.x && _yAxisBoundOther.y >= _yAxisBoundThis.x && _yAxisBoundOther.x <= _yAxisBoundThis.y)
        //{
        //    Matrix4x4 otherWorldSpace = other.WorldToLocal();

        //    _xAxisBoundOther = otherWorldSpace.MultiplyPoint3x4(other.X_AxisBound);
        //    _yAxisBoundOther = otherWorldSpace.MultiplyPoint3x4(other.Y_AxisBound);
        //    _xAxisBoundThis = WorldToLocal().MultiplyPoint3x4(X_AxisBound);
        //    _yAxisBoundThis = WorldToLocal().MultiplyPoint3x4(Y_AxisBound);

        //    // Check if the extents for the AABB in OBB local space are colliding with the extents for the OBB in OBB world space.
        //    if (_xAxisBoundOther.x <= _xAxisBoundThis.y && _xAxisBoundOther.y >= _xAxisBoundThis.x && _yAxisBoundOther.y >= _yAxisBoundThis.x && _yAxisBoundOther.x <= _yAxisBoundThis.y)
        //    {
        //        // Collision.
        //        return true;
        //    }
        //}


        // Transform the extents to OBB world space.
        other.CalculateBoundingBoxWorld();

        // OBB max extents
        Vector2 xAxisBoundOther = other.X_AxisMaxBound;
        Vector2 yAxisBoundOther = other.Y_AxisMaxBound;

        // Check if the extents for the AABB are colliding with the extents for the OBB in OBB world space.
        if (X_AxisBound.x <= xAxisBoundOther.y && X_AxisBound.y >= xAxisBoundOther.x && Y_AxisBound.y >= yAxisBoundOther.x && Y_AxisBound.x <= yAxisBoundOther.y)
        {
            CalculateBoundingBoxWorld();

            // AABB max extents
            Vector2 _xAxisBoundThis = X_AxisMaxBound;
            Vector2 _yAxisBoundThis = Y_AxisMaxBound;

            // Transform the entents to OBB local space.
            other.CalculateBoundingBoxWorld(ref _xAxisBoundThis, ref _yAxisBoundThis);

            // Check if the extents for the AABB in OBB local space are colliding with the extents for the OBB in OBB world space.
            if (_xAxisBoundThis.x <= other.X_AxisBound.y && _xAxisBoundThis.y >= other.X_AxisBound.x && _yAxisBoundThis.y >= other.Y_AxisBound.x && _yAxisBoundThis.x <= other.Y_AxisBound.y)
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
        Particle2D particle = GetComponent<Particle2D>();
        xAxisBound.x = particle.Position.x - BoundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + BoundingBox.x;
        yAxisBound.x = particle.Position.y - BoundingBox.y * 0.5f;
        yAxisBound.y = yAxisBound.x + BoundingBox.y;

        return;
    }

    public void CalculateBoundingBoxAxisWorld(Particle2D _particle, ref Vector2 _xAxisBound, ref Vector2 _yAxisBound)
    {
        // Get the particle for use when game is not running.
        Particle2D particle = GetComponent<Particle2D>();
        
        // Get the position of the particle.
        Vector2 position = _particle.Position;
        
        // Place the particle position in world space.
        WorldToLocal().MultiplyPoint3x4(position);

        // Calculate the bounds at the world position.
        _xAxisBound.x = position.x - BoundingBox.x * 0.5f;
        _xAxisBound.y = _xAxisBound.x + BoundingBox.x;
        _yAxisBound.x = position.y - BoundingBox.y * 0.5f;
        _yAxisBound.y = _yAxisBound.x + BoundingBox.y;

        // Debug drawing data for extents in world space.
        Vector2 drawCorner1 = new Vector2(_xAxisBound.x, _yAxisBound.x);
        Vector2 drawCorner2 = new Vector2(_xAxisBound.x, _yAxisBound.y);
        Vector2 drawCorner3 = new Vector2(_xAxisBound.y, _yAxisBound.y);
        Vector2 drawCorner4 = new Vector2(_xAxisBound.y, _yAxisBound.x);
        
        // Draw the extents in world space.
        Debug.DrawLine(drawCorner1, drawCorner2, Color.white);
        Debug.DrawLine(drawCorner2, drawCorner3, Color.white);
        Debug.DrawLine(drawCorner3, drawCorner4, Color.white);
        Debug.DrawLine(drawCorner4, drawCorner1, Color.white);


        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxLocal()
    {
        // Rotate bound corners.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Get the bounds position for each box corner.
        Vector2 corner1 = new Vector2(X_AxisBound.x, Y_AxisBound.x);
        Vector2 corner2 = new Vector2(X_AxisBound.x, Y_AxisBound.y);
        Vector2 corner3 = new Vector2(X_AxisBound.y, Y_AxisBound.y);
        Vector2 corner4 = new Vector2(X_AxisBound.y, Y_AxisBound.x);

        // Transform the corners from local to world space.
        corner1 = WorldToLocal().MultiplyPoint3x4(corner1);
        corner2 = WorldToLocal().MultiplyPoint3x4(corner2);
        corner3 = WorldToLocal().MultiplyPoint3x4(corner3);
        corner4 = WorldToLocal().MultiplyPoint3x4(corner4);

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maximumX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minimumY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maximumY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        // Create vector2 of new extents.
        xAxisMaxBound = new Vector2(minimumX, maximumX);
        yAxisMaxBound = new Vector2(minimumY, maximumY);

        // Debug drawing data for extents in world space.
        Vector2 drawCorner1 = new Vector2(X_AxisMaxBound.x, Y_AxisMaxBound.x);
        Vector2 drawCorner2 = new Vector2(X_AxisMaxBound.x, Y_AxisMaxBound.y);
        Vector2 drawCorner3 = new Vector2(X_AxisMaxBound.y, Y_AxisMaxBound.y);
        Vector2 drawCorner4 = new Vector2(X_AxisMaxBound.y, Y_AxisMaxBound.x);

        // Draw the extents in world space.
        Debug.DrawLine(drawCorner1, drawCorner2, Color.red);
        Debug.DrawLine(drawCorner2, drawCorner3, Color.red);
        Debug.DrawLine(drawCorner3, drawCorner4, Color.red);
        Debug.DrawLine(drawCorner4, drawCorner1, Color.red);

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
        corner1 = WorldToLocalOther().MultiplyPoint3x4(corner1);
        corner2 = WorldToLocalOther().MultiplyPoint3x4(corner2);
        corner3 = WorldToLocalOther().MultiplyPoint3x4(corner3);
        corner4 = WorldToLocalOther().MultiplyPoint3x4(corner4);

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maximumX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minimumY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maximumY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        // Set new max extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);

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

        return;
    }

    // Calculate the bounding box limits in local to world space.
    public void CalculateBoundingBoxWorld()
    {
        // Rotate bound corners.
        // Find min/max extents for x and y.
        // return min and max extents into ref variables.

        // Get the bounds position for each box corner.
        Vector2 corner1 = new Vector2(X_AxisBound.x, Y_AxisBound.x);
        Vector2 corner2 = new Vector2(X_AxisBound.x, Y_AxisBound.y);
        Vector2 corner3 = new Vector2(X_AxisBound.y, Y_AxisBound.y);
        Vector2 corner4 = new Vector2(X_AxisBound.y, Y_AxisBound.x);

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
        xAxisMaxBound = new Vector2(minimumX, maximumX);
        yAxisMaxBound = new Vector2(minimumY, maximumY);

        // Debug drawing data for extents in world space.
        Vector2 drawCorner1 = new Vector2(X_AxisMaxBound.x, Y_AxisMaxBound.x);
        Vector2 drawCorner2 = new Vector2(X_AxisMaxBound.x, Y_AxisMaxBound.y);
        Vector2 drawCorner3 = new Vector2(X_AxisMaxBound.y, Y_AxisMaxBound.y);
        Vector2 drawCorner4 = new Vector2(X_AxisMaxBound.y, Y_AxisMaxBound.x);

        // Draw the extents in world space.
        Debug.DrawLine(drawCorner1, drawCorner2, Color.red);
        Debug.DrawLine(drawCorner2, drawCorner3, Color.red);
        Debug.DrawLine(drawCorner3, drawCorner4, Color.red);
        Debug.DrawLine(drawCorner4, drawCorner1, Color.red);

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
        corner1 = LocalToWorldOther().MultiplyPoint3x4(corner1);
        corner2 = LocalToWorldOther().MultiplyPoint3x4(corner2);
        corner3 = LocalToWorldOther().MultiplyPoint3x4(corner3);
        corner4 = LocalToWorldOther().MultiplyPoint3x4(corner4);

        // Determine the minimum and maximum extents for each axis.
        float minimumX = Mathf.Min(corner1.x, corner2.x, corner3.x, corner4.x);
        float maximumX = Mathf.Max(corner1.x, corner2.x, corner3.x, corner4.x);
        float minimumY = Mathf.Min(corner1.y, corner2.y, corner3.y, corner4.y);
        float maximumY = Mathf.Max(corner1.y, corner2.y, corner3.y, corner4.y);

        // Create vector2 of new extents.
        _xAxisBound = new Vector2(minimumX, maximumX);
        _yAxisBound = new Vector2(minimumY, maximumY);

        // Debug drawing data for extents in world space.
        Vector2 drawCorner1 = new Vector2(_xAxisBound.x, _yAxisBound.x);
        Vector2 drawCorner2 = new Vector2(_xAxisBound.x, _yAxisBound.y);
        Vector2 drawCorner3 = new Vector2(_xAxisBound.y, _yAxisBound.y);
        Vector2 drawCorner4 = new Vector2(_xAxisBound.y, _yAxisBound.x);

        if (name == "OBB")
        {
            // Draw the extents in world space.
            Debug.DrawLine(drawCorner1, drawCorner2, Color.blue);
            Debug.DrawLine(drawCorner2, drawCorner3, Color.blue);
            Debug.DrawLine(drawCorner3, drawCorner4, Color.blue);
            Debug.DrawLine(drawCorner4, drawCorner1, Color.blue);
        }
        else
        {
            // Draw the extents in world space.
            Debug.DrawLine(drawCorner1, drawCorner2, Color.white);
            Debug.DrawLine(drawCorner2, drawCorner3, Color.white);
            Debug.DrawLine(drawCorner3, drawCorner4, Color.white);
            Debug.DrawLine(drawCorner4, drawCorner1, Color.white);
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

    // Accessor for the bounding box x-axis.
    public Vector2 X_AxisMaxBound
    {
        get
        {
            return xAxisMaxBound;
        }
    }

    // Accessor for the bounding box y-axis.
    public Vector2 Y_AxisMaxBound
    {
        get
        {
            return yAxisMaxBound;
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

    // Get the local to world transformation.
    public Matrix4x4 LocalToWorldOther()
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
        Matrix4x4 model = translateInverse * rotateInverse * translate;

        return model;
    }

    // Get the world to local transformation.
    public Matrix4x4 WorldToLocalOther()
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
        Matrix4x4 model = translateInverse * rotate * translate;

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
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
