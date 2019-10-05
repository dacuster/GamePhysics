using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public Vector2 boundingBox;
    //public float leftBound = 0.0f;
    //public float rightBound = 0.0f;
    //public float bottomBound = 0.0f;
    //public float topBound = 0.0f;
    public Vector2 bottomLeftCorner = Vector2.zero;
    public Vector2 topLeftCorner = Vector2.zero;
    public Vector2 bottomRightCorner = Vector2.zero;
    public Vector2 topRightCorner = Vector2.zero;
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
        float rotation = particle.rotation;
        Vector2 position = particle.position;

        Vector2 xRotation = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
        Vector2 yRotation = new Vector2(-Mathf.Sin(rotation), Mathf.Cos(rotation));

        xRotation *= boundingBox.x * 0.5f;
        yRotation *= boundingBox.y * 0.5f;

        bottomLeftCorner = position - xRotation - yRotation;
        bottomRightCorner = position + xRotation - yRotation;
        topRightCorner = position + xRotation + yRotation;
        topLeftCorner = position - xRotation + yRotation;

        //leftBound = particle.position.x - boundingBox.x / 2.0f;
        //rightBound = leftBound + boundingBox.x;
        //bottomBound = particle.position.y - boundingBox.y / 2.0f;
        //topBound = bottomBound + boundingBox.y;

        //Vector3 bottomLineLeft = new Vector3(leftBound, bottomBound);
        //Vector3 bottomLineRight = new Vector3(rightBound, bottomBound);
        //Vector3 topLineLeft = new Vector3(leftBound, topBound);
        //Vector3 topLineRight = new Vector3(rightBound, topBound);
        //Vector3 leftLineTop = new Vector3(leftBound, topBound);
        //Vector3 leftLineBottom = new Vector3(leftBound, bottomBound);
        //Vector3 rightLineTop = new Vector3(rightBound, topBound);
        //Vector3 rightLineBottom = new Vector3(rightBound, bottomBound);

        // Draw bottom line.
        Debug.DrawLine(bottomLeftCorner, bottomRightCorner, Color.green);
        // Draw top line
        Debug.DrawLine(topLeftCorner, topRightCorner, Color.green);
        // Draw left line.
        Debug.DrawLine(bottomLeftCorner, topLeftCorner, Color.green);
        // Draw right line
        Debug.DrawLine(bottomRightCorner, topRightCorner, Color.green);

        return;
    }
}

[CustomEditor(typeof(ObjectBoundingBoxHull2D))]
public class ObjectBoxEditor : Editor
{
    private void OnSceneGUI()
    {
        ObjectBoundingBoxHull2D boxHull = (ObjectBoundingBoxHull2D)target;

        Color purple = CreateColor(112.0f, 0.0f, 255.0f);
        Handles.color = purple;
        Handles.DrawWireCube(boxHull.particle.position, boxHull.boundingBox);
    }

    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
