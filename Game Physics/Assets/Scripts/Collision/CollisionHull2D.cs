using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]

// Abstract prevent anything from instantiating it.
public abstract class CollisionHull2D : MonoBehaviour
{
    // TODO: Comment for lab 5.
    public class Collision
    {
        public struct Contact
        {
            Vector2 position;
            Vector2 normal;
            float restitution;
            // float? collisionDepth;
        }
        // The 2 hulls which are colliding.
        public CollisionHull2D a = null;
        public CollisionHull2D b = null;

        // All the contact involved in the collision.
        public Contact[] contact = new Contact[4];

        // Number of contacts involved in the collision.
        public int contactCount = 0;

        // Was there a collision.
        public bool status = false;

        // Closing velocity of the collision.
        public float closingVelocity;

        // Calculate the closing velocity of the 2 colliding objects.
        public void CalculateClosingVelocity()
        {
            // Calculate the difference in velocities.
            Vector2 velocityDifference = a.Particle.Velocity - b.Particle.Velocity;
            
            // Get the opposite values of the difference.
            velocityDifference *= -1.0f;
            
            // Get the difference in positions.
            Vector2 positionDifference = a.Particle.Position - b.Particle.Position;
            
            // Normalize the difference in positions.
            positionDifference.Normalize();

            // Find the scalar(dot) product of the difference in velocities and difference in positions normalized.
            closingVelocity = Vector2.Dot(velocityDifference, positionDifference);

            return;
        }
    }
    // END TODO

    // Different types of collision hulls.
    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }

    // Constructor.
    protected CollisionHull2D(CollisionHullType2D type)
    {
        // Set the type to the given type.
        Type = type;

        return;
    }

    void Awake()
    {
        // Assign the Particle2D component.
        Particle = gameObject.GetComponent<Particle2D>();

        return;
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {		
        return false;
    }

    /********************************
    **  Abstract Collision Checks  **
    ********************************/

    // Check collision of this vs circle.
    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c);
    // Check collision of this vs AABB.
    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c);
    // Check collision of this vs OBB.
    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c);

    // Particle accessor.
    public Particle2D Particle { get; set; }

    // Accessor for collision hull type.
    public CollisionHullType2D Type { get; }

    //protected CollisionHullType2D Type { set; }
}