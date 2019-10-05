using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CircleCollisionHull2D : CollisionHull2D
{
    public CircleCollisionHull2D() : base(CollisionHullType2D.hull_circle) { }

    // Apply range as positive value.
    [Range(0.0f, 100.0f)]
    public float radius;

    private void Start()
    {
        //particle = GetComponent<Particle2D>();
    }

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
                if (TestCollisionVsCircle(hull as CircleCollisionHull2D))
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    hull.GetComponent<MeshRenderer>().material.color = Color.red;
                }
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

    public override bool TestCollisionVsCircle(CircleCollisionHull2D other)
    {
        // collision passes if distance between centers <= sum of radii
        // optimized collision passes if (distance between centers) squared <= (sum of radii) squared
        // 1. get the centers locations
        // 2. difference between centers
        // 3. distance squared = dot(diff, diff)         (squared magnitued - built into unity)
        // 4. sum of radii
        // 5. square sum
        // 6. DO THE TEST: distSq <= sumSq

        Vector2 differenceCenters = particle.position - other.particle.position;

        float distanceSquared = differenceCenters.sqrMagnitude;

        float radiiSum = radius + other.radius;

        float radiiSumSquared = radiiSum * radiiSum;

        return distanceSquared <= radiiSumSquared;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        // Calculate closest point by clamping circle centers on each dimension
        // passes if closest point vs circle passes
        // 1. Clamp circle centers to AABB.
        // 2. Test distance to circle radius with AABB point found from clamping.
        // 1. Get position of other hull.
        // 2. Get bounding box of other hull.
        // 3. Calculate bounding box esges.
        // 4. Find nearest point to circle on bounding box perimeter.
        // 5. Calculate difference between bounding box point and circle edge.
        // 6. Test difference with radius. (Return true of less than radius)


        float nearestX = Mathf.Clamp(particle.position.x, other.leftBound, other.rightBound);
        float nearestY = Mathf.Clamp(particle.position.y, other.bottomBound, other.topBound);

        float deltaX = particle.position.x - nearestX;
        float deltaY = particle.position.y - nearestY;

        Vector2 start = new Vector2(nearestX, nearestY);
        Vector2 end = new Vector2(particle.position.x, particle.position.y);

        Debug.DrawLine(start, end, Color.red);

        return (deltaX * deltaX + deltaY * deltaY) <= (radius * radius);
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        // same as above, but first...
        // transform circle position by multiplying by box world matrix inverse
        // 1. .....

        //Vector3 circlePosition = particle.position;
        //Vector3 circleRotation = new Vector3(0.0f, 0.0f, particle.rotation);
        //Matrix4x4 inverseOther = other.transform.worldToLocalMatrix;
        //inverseOther = inverseOther.inverse;
        //particle.rotation = inverseOther.rotation.eulerAngles.z;
        //inverseOther.transpose.MultiplyPoint3x4(particle.position);
        //inverseOther.MultiplyPoint(circlePosition);
        //particle.position = circleRotation;



        Vector3 circlePosition = particle.position;
        Vector3 circleScale = particle.transform.localScale;
        Quaternion circleRotation = Quaternion.Euler(0.0f, 0.0f, particle.rotation);

        Vector3 otherPosition = other.particle.position;
        Vector3 otherScale = other.particle.transform.localScale;
        Quaternion otherRotation = Quaternion.Euler(0.0f, 0.0f, other.particle.rotation);

        Matrix4x4 otherMatrix = Matrix4x4.TRS(otherPosition, otherRotation, otherScale);
        //otherMatrix = otherMatrix.inverse;
        Matrix4x4 circleMatrix = Matrix4x4.TRS(circlePosition, circleRotation, circleScale);
        //Matrix4x4 otherTranslateMatrix = Matrix4x4.Translate(otherPosition);
        //Matrix4x4 otherRotationMatrix = Matrix4x4.Rotate(otherRotation);
        //Matrix4x4 otherTranslateInverseMatrix = otherTranslateMatrix.inverse;
        //Matrix4x4 otherRotationInverseMatrix = otherRotationMatrix.inverse;

        //Matrix4x4 circleTranslateMatrix = Matrix4x4.Translate(circlePosition);
        //Matrix4x4 circleRotationMatrix = Matrix4x4.Rotate(circleRotation);
        //Matrix4x4 circleTranslateInverseMatrix = circleTranslateMatrix.inverse;
        //Matrix4x4 circleRotationInverseMatrix = circleRotationMatrix.inverse;


        //circleMatrix *= circleTranslateInverseMatrix;
        //circleMatrix *= otherTranslateInverseMatrix;
        //circleMatrix *= otherRotationMatrix;
        //circleMatrix *= otherTranslateMatrix;
        //circleMatrix *= circleTranslateMatrix;




        //circleMatrix *= otherRotationMatrix;
        //circleMatrix *= otherTranslateMatrix;
        //circleMatrix *= otherRotationInverseMatrix;
        //circleMatrix *= otherTranslateInverseMatrix;



        //circleTranslateMatrix *= otherTranslateMatrix;
        //circleRotationMatrix *= otherRotationMatrix;

        //circleMatrix *= otherMatrix;

        //circlePosition = circleMatrix.GetColumn(3);
        //Vector4 column = otherMatrix.GetColumn(3);
        //Vector3 columnToVector3 = new Vector3(column.x, column.y, column.z);
        //circlePosition = circlePosition + columnToVector3;

        //circlePosition = otherMatrix.MultiplyPoint3x4(circlePosition);
        //otherMatrix = otherMatrix.inverse;


        //Vector2 left = new Vector2(other.leftBound, 0.0f);
        //Vector2 right = new Vector2(other.rightBound, 0.0f);
        //Vector2 top = new Vector2(0.0f, other.topBound);
        //Vector2 bottom = new Vector2(0.0f, other.bottomBound);




        //left = otherMatrix.MultiplyPoint3x4(left);
        //right = otherMatrix.MultiplyPoint3x4(right);
        //top = otherMatrix.MultiplyPoint3x4(top);
        //bottom = otherMatrix.MultiplyPoint3x4(bottom);



        float nearestX = Mathf.Clamp(circlePosition.x, other.leftBound, other.rightBound);
        float nearestY = Mathf.Clamp(circlePosition.y, other.bottomBound, other.topBound);
        float nearestX = 0.0f;
        float nearestY = 0.0f;

        float deltaX = circlePosition.x - nearestX;
        float deltaY = circlePosition.y - nearestY;

        Vector2 start = new Vector2(nearestX, nearestY);
        Vector2 end = new Vector2(circlePosition.x, circlePosition.y);

        Debug.DrawLine(start, end, Color.red);

        return (deltaX * deltaX + deltaY * deltaY) <= (radius * radius);
        //= otherMatrix.rotation.eulerAngles.z;








        return false;
    }

}
[CustomEditor(typeof(CircleCollisionHull2D))]
public class CircleEditor : Editor
{
    private void OnSceneGUI()
    {
        CircleCollisionHull2D circleHull = (CircleCollisionHull2D)target;

        Handles.color = new Color(112.0f / 255.0f, 0, 255.0f / 255.0f);
        Handles.DrawWireDisc(circleHull.particle.position, Vector3.back, circleHull.radius);
    }
}
