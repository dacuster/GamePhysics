using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public Vector2 boundingBox;
    public Vector2 xAxisBound;
    public Vector2 yAxisBound;
    public float leftBound = 0.0f;
    public float rightBound = 0.0f;
    public float bottomBound = 0.0f;
    public float topBound = 0.0f;
    private void FixedUpdate()
    {
        CalculateBoundingBox();
        GetComponent<MeshRenderer>().material.color = Color.white;

        foreach (CollisionHull2D hull in GameObject.FindObjectsOfType<CollisionHull2D>())
        {
            if (hull == this)
            {
                break;
            }

            if (hull.Type == CollisionHullType2D.hull_circle)
            {
                //if (TestCollisionVsCircle(hull as CircleCollisionHull2D))
                //{
                //    GetComponent<MeshRenderer>().material.color = Color.red;
                //    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                //}
            }
            else if (hull.Type == CollisionHullType2D.hull_aabb)
            {
                if (TestCollisionVsAABB(hull as AxisAlignedBoundingBoxHull2D))
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
            else if (hull.Type == CollisionHullType2D.hull_obb)
            {
                if (TestCollisionVsOBB(hull as ObjectBoundingBoxHull2D))
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }
    public ObjectBoundingBoxHull2D() : base(CollisionHullType2D.hull_obb) { }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // see circle
        return other.TestCollisionVsOBB(this);
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        // see AABB
        return other.TestCollisionVsOBB(this);
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // AABB-OBB part 2 twice
        // 1. .....


        return false;
    }
    private void CalculateBoundingBox()
    {
        // Create the edges based on the current position and the bounding box dimensions.
        leftBound = particle.position.x - boundingBox.x * 0.5f;
        rightBound = leftBound + boundingBox.x;
        bottomBound = particle.position.y - boundingBox.y * 0.5f;
        topBound = bottomBound + boundingBox.y;



        //// Used for drawing the edges in debug.
        //Vector3 bottomLineLeft = new Vector3(leftBound, bottomBound);
        //Vector3 bottomLineRight = new Vector3(rightBound, bottomBound);
        //Vector3 topLineLeft = new Vector3(leftBound, topBound);
        //Vector3 topLineRight = new Vector3(rightBound, topBound);
        //Vector3 leftLineTop = new Vector3(leftBound, topBound);
        //Vector3 leftLineBottom = new Vector3(leftBound, bottomBound);
        //Vector3 rightLineTop = new Vector3(rightBound, topBound);
        //Vector3 rightLineBottom = new Vector3(rightBound, bottomBound);

        //// Draw bottom line.
        //Debug.DrawLine(bottomLineLeft, bottomLineRight, Color.green);
        //// Draw top line
        //Debug.DrawLine(topLineLeft, topLineRight, Color.green);
        //// Draw left line.
        //Debug.DrawLine(leftLineTop, leftLineBottom, Color.green);
        //// Draw right line
        //Debug.DrawLine(rightLineTop, rightLineBottom, Color.green);

        return;
    }
}

[CustomEditor(typeof(ObjectBoundingBoxHull2D))]
public class ObjectBoxEditor : Editor
{
    private void OnSceneGUI()
    {
        // Get the hull associated with this object.
        ObjectBoundingBoxHull2D boxHull = (ObjectBoundingBoxHull2D)target;

        // Get the position and create a translation matrix.
        Vector3 position = boxHull.particle.position;
        Matrix4x4 translateMatrix = Matrix4x4.Translate(position);

        // Get negative rotation. Create a rotation matrix and invert it.
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, boxHull.particle.rotation);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);
        Matrix4x4 rotationInverseMatrix = rotationMatrix.inverse;

        // Create the model view matrix.
        Matrix4x4 matrix = translateMatrix * rotationMatrix * translateMatrix.inverse;

        // Muliply position by inverse model view matrix to position and rotate properly.
        position = matrix.inverse.MultiplyPoint3x4(position);

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        Handles.color = purple;
        // Change the gizmo drawing matrix.
        Handles.matrix = matrix;
        // Draw the cube.
        Handles.DrawWireCube(position, boxHull.boundingBox);
    }

    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
