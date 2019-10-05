using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AxisAlignedBoundingBoxHull2D : CollisionHull2D
{
    public Vector2 boundingBox;
    public Vector2 xAxisBound;
    public Vector2 yAxisBound;


    private void FixedUpdate()
    {
        // Calculate the bounding box edges.
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

    public AxisAlignedBoundingBoxHull2D() : base(CollisionHullType2D.hull_aabb) { }

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // see circle
        return other.TestCollisionVsAABB(this);
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        // for each dimension, max extent of A >= min extent of B
        // 1. .....

        if (xAxisBound.x <= other.xAxisBound.y && xAxisBound.y >= other.xAxisBound.x && yAxisBound.y >= other.yAxisBound.x && yAxisBound.x <= other.yAxisBound.y)
        {

            return true;
        }


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again
        // 1. .....

        if (xAxisBound.x <= other.xAxisBound.y && xAxisBound.y >= other.xAxisBound.x && yAxisBound.y >= other.yAxisBound.x && yAxisBound.x <= other.yAxisBound.y)
        {

            return true;
        }

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

[CustomEditor(typeof(AxisAlignedBoundingBoxHull2D))]
public class AxisBoxEditor : Editor
{
    private void OnSceneGUI()
    {
        AxisAlignedBoundingBoxHull2D boxHull = (AxisAlignedBoundingBoxHull2D)target;

        Color purple = CreateColor(112.0f, 0.0f, 255.0f);
        Handles.color = purple;
        Handles.DrawWireCube(boxHull.particle.position, boxHull.boundingBox);
    }

    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
