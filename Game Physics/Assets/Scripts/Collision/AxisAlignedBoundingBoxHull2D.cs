using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AxisAlignedBoundingBoxHull2D : CollisionHull2D
{
    public Vector2 boundingBox;

    private void FixedUpdate()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;

        foreach (CollisionHull2D hull in GameObject.FindObjectsOfType<CollisionHull2D>())
        {
            if (hull == this)
            {
                break;
            }

            if (hull.Type == CollisionHullType2D.hull_circle)
            {
                TestCollisionVsCircle(hull as CircleCollisionHull2D);
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
                TestCollisionVsOBB(hull as ObjectBoundingBoxHull2D);
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


        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // same as above twice
        // first, test AABB vs max extents of OBB
        // then, multiply by OBB inverse matrix, do test again
        // 1. .....


        return false;
    }

    private void CalculateBoundingBox()
    {
        // Calculate width and height of bounding box.
        //width = transform.localScale.x;
        //height = transform.localScale.y;

        //// Calculate positions of each bounding box wall.
        //left = particle.position.x - width / 2;
        //right = left + width;
        //bottom = particle.position.y - height / 2;
        //top = bottom + height;

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
