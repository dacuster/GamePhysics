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

        Vector3 circlePosition = particle.position;

        Vector3 otherPosition = other.particle.position;
        Quaternion otherRotation = Quaternion.Euler(0.0f, 0.0f, other.particle.rotation);


        Matrix4x4 otherTranslateMatrix = Matrix4x4.Translate(otherPosition);
        Matrix4x4 otherRotationMatrix = Matrix4x4.Rotate(otherRotation);
        Matrix4x4 otherRotationInverseMatrix = otherRotationMatrix.inverse;

        // Works
        Matrix4x4 matrix = otherTranslateMatrix * otherRotationInverseMatrix;

        // Works
        Vector3 difference = circlePosition - otherPosition;

        // Works
        circlePosition = matrix.MultiplyPoint3x4(difference);

        // Works
        float nearestX = Mathf.Clamp(circlePosition.x, other.leftBound, other.rightBound);
        float nearestY = Mathf.Clamp(circlePosition.y, other.bottomBound, other.topBound);

        float deltaX = circlePosition.x - nearestX;
        float deltaY = circlePosition.y - nearestY;




        // Get the position and create a translation matrix.
        Vector3 position = other.particle.position;
        Matrix4x4 translateMatrix = Matrix4x4.Translate(position);

        // Get negative rotation. Create a rotation matrix and invert it.
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, -other.particle.rotation);
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);
        Matrix4x4 rotationInverseMatrix = rotationMatrix.inverse;

        Vector3 nearestPoint = new Vector3(nearestX, nearestY, 0.0f);

        // Create the model view matrix.
        //Matrix4x4 matrixModel = translateMatrix.inverse * rotationMatrix.inverse;
        //Matrix4x4 matrixModel = translateMatrix * rotationMatrix;
        //Matrix4x4 matrixModel = translateMatrix.inverse * rotationMatrix;
        Matrix4x4 matrixModel = translateMatrix * rotationMatrix.inverse;

        nearestPoint = matrixModel.MultiplyPoint3x4(nearestPoint);






        //Vector2 xRotation = new Vector2(Mathf.Cos(-other.particle.rotation), Mathf.Sin(-other.particle.rotation));
        //Vector2 yRotation = new Vector2(-Mathf.Sin(-other.particle.rotation), Mathf.Cos(-other.particle.rotation));
        //nearestPoint.x *= xRotation.x;
        //nearestPoint.y *= yRotation.y;
        ////deltaPoint = otherPosition - deltaPoint;
        //Quaternion inverseRotation = Quaternion.Euler(0.0f, 0.0f, other.particle.rotation);
        //Matrix4x4 inverseRotationMatrix = Matrix4x4.Rotate(inverseRotation);
        //Matrix4x4 matrixDelta = inverseRotationMatrix.inverse;
        //nearestPoint = matrixDelta.MultiplyVector(nearestPoint);








        // Debug drawing.
        Vector2 startOriginal = new Vector2(nearestX, nearestY);
        Vector2 startTransform = new Vector2(nearestPoint.x, nearestPoint.y);
        Vector2 end = new Vector2(circlePosition.x, circlePosition.y);

        Debug.DrawLine(startOriginal, end, Color.white);
        Debug.DrawLine(startTransform, end, Color.red);

        return (deltaX * deltaX + deltaY * deltaY) <= (radius * radius);
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
