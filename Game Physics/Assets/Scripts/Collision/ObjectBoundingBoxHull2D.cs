using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public Vector2 boundingBox;
    public Vector2 xAxisBound;
    public Vector2 yAxisBound;
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
                if (TestCollisionVsCircle(hull as CircleCollisionHull2D))
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                    Debug.Log("Collision");
                }
            }
            else if (hull.Type == CollisionHullType2D.hull_aabb)
            {
                if (TestCollisionVsAABB(hull as AxisAlignedBoundingBoxHull2D))
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                    Debug.Log("Collision");
                }
            }
            else if (hull.Type == CollisionHullType2D.hull_obb)
            {
                if (TestCollisionVsOBB(hull as ObjectBoundingBoxHull2D))
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                    Debug.Log("Collision");
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
        xAxisBound.x = particle.position.x - boundingBox.x * 0.5f;
        xAxisBound.y = xAxisBound.x + boundingBox.x;
        yAxisBound.x = particle.position.y - boundingBox.y * 0.5f;
        yAxisBound.y = yAxisBound.x + boundingBox.y;

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



    public Vector2 LocalToWorldAxisX()
    {

        return Vector2.zero;
    }
}
