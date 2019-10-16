using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class requires Particle2D to prevent null references.
[RequireComponent(typeof(Particle2D))]

// Abstract prevent anything from instantiating it.
public abstract class CollisionHull2D : MonoBehaviour
{
    // Video tutorial circle collision handler. https://www.youtube.com/watch?v=LPzyNOHY3A4
    // TODO: Comment for lab 5.
    // Collision data.
    public class Collision
    {
        // Point of contact in collision.
        public struct Contact
        {
            // Position of the contact.
            public Vector2 position;
            // Contact normal.
            public Vector2 normal;
            // Coefficient of restitution of the contact.
            public float restitution;
        }

        // The 2 hulls which are colliding.
        private CollisionHull2D a = null;
        private CollisionHull2D b = null;

        // All the contacts involved in the collision.
        private Contact[] contacts = new Contact[4];

        // Number of contacts involved in the collision.
        private int contactCount = 0;

        // Was there a collision.
        private bool status = false;

        // Closing velocity of the collision.
        private Vector2 closingVelocity;

        // Calculate the closing velocity of the 2 colliding objects for the given contact normal.
        public Vector2 CalculateClosingVelocity(Vector2 contactNormal)
        {
            // Calculate the difference in velocities.
            Vector2 velocityDifference = A.Particle.Velocity - B.Particle.Velocity;
            
            // Get the opposite values of the difference.
            velocityDifference *= -1.0f;
            
            // Get the difference in positions.
            Vector2 positionDifference = A.Particle.Position - B.Particle.Position;
            
            // Normalize the difference in positions.
            positionDifference.Normalize();

            // Find the scalar(dot) product of the difference in velocities and difference in positions normalized.
            return Vector2.Dot(velocityDifference, positionDifference) * contactNormal;
        }

        // Create a contact and add it to the contacts list.
        public void AddContact(Vector2 _position, Vector2 _normal, float _restitution, int _id)
        {
            // Create a new contact.
            Contact contact;

            // Set the position of the contact.
            contact.position = _position;

            // Set the normal of the contact.
            contact.normal = _normal;

            // Set the restitution of the contact.
            contact.restitution = _restitution;

            // Add this contact to the list of contacts based on the current contact id.
            Contacts[_id] = contact;

            return;
        }

        public void ResolveCollision()
        {
            Particle2D particleA = A.Particle;
            Particle2D particleB = B.Particle;
            Vector2 newVelocity = Vector2.zero;

            for (int currentContact = 0; currentContact < contactCount; currentContact++)
            {
                // TODO: Closing velocity is overlap of both objects?
                CalculateClosingVelocity(Contacts[currentContact].normal);
                newVelocity = 0.5f * -closingVelocity * Contacts[currentContact].restitution;
            }

            particleA.Velocity = newVelocity;

            return;
        }

        // Collision hull a accessor.
        public CollisionHull2D A { get => a; set => a = value; }

        // Collision hull b accessor.
        public CollisionHull2D B { get => b; set => b = value; }

        // Collision status accessor.
        public bool Status { get => status; set => status = value; }

        // Collsion contact count accessor.
        public int ContactCount { get => contactCount; set => contactCount = value; }

        // Contacts of the collision.
        public Contact[] Contacts { get => contacts; set => contacts = value; }
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

    // Update for physics.
    private void FixedUpdate()
    {
        // Iterate through every CollisionHull2D in the game.
        foreach (CollisionHull2D hull in GameObject.FindObjectsOfType<CollisionHull2D>())
        {
            // Ignore collisions with self.
            if (hull == this)
            {
                continue;
            }

            // Create new potential collision.
            Collision collision = new Collision();

            // Check for collision and collect collision data.
            if (TestCollision(this, hull, ref collision))
            {
                // Resolve the collision.
                collision.ResolveCollision();
            }
        }

        return;
    }

    // Test collision with given hulls and collect collision data.
    public static bool TestCollision(CollisionHull2D left, CollisionHull2D right, ref Collision collision)
    {
        // Other hull is a circle.
        if (right.Type == CollisionHullType2D.hull_circle)
        {
            // Test for collision with a circle and collect collision data.
            return left.TestCollisionVsCircle(right as CircleCollisionHull2D, ref collision);
        }
        // Other hull is an AABB.
        else if (right.Type == CollisionHullType2D.hull_aabb)
        {
            // Test for collision with an AABB and collect collision data.
            return left.TestCollisionVsAABB(right as AxisAlignedBoundingBoxHull2D, ref collision);
        }
        // Other hull is an OBB.
        else if (right.Type == CollisionHullType2D.hull_obb)
        {
            // Test for collision with an OBB and collect collision data.
            return left.TestCollisionVsOBB(right as ObjectBoundingBoxHull2D, ref collision);
        }

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


    /****************
    **  Accessors  ** 
    ****************/

    // Particle accessor.
    public Particle2D Particle { get; set; }

    // Accessor for collision hull type.
    public CollisionHullType2D Type { get; }
}