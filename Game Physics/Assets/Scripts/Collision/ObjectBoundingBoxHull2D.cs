using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{
    public Vector2 boundingBox;
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
