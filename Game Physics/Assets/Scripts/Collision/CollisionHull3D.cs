using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// This class requires Particle3D to prevent null references.
[RequireComponent(typeof(Particle3D))]

// Abstract prevent anything from instantiating it.
public abstract class CollisionHull3D : MonoBehaviour
{
    // Debug mode for drawing in the scene view.
    [SerializeField]
    protected bool debugMode = false;

    private bool collided = false;

    // Video tutorial circle collision handler. https://www.youtube.com/watch?v=LPzyNOHY3A4
    // TODO: Comment for lab 5.
    // Collision data.
    public class Collision
    {
        // Point of contact in collision.
        public struct Contact
        {
            // Position of the contact.
            public Vector3 position;
            // Contact normal.
            public Vector3 normal;
            // Coefficient of restitution of the contact.
            public float restitution;
        }

        // The 2 hulls which are colliding.
        private CollisionHull3D a = null;
        private CollisionHull3D b = null;

        // All the contacts involved in the collision.
        private Contact[] contacts = new Contact[4];

        // Number of contacts involved in the collision.
        private int contactCount = 0;

        // Was there a collision.
        private bool status = false;

        // Closing velocity of the collision.
        private Vector3 closingVelocity = Vector3.zero;

        // Calculate the closing velocity of the 2 colliding objects for the given contact normal.
        public float CalculateClosingVelocity()
        {
            // Calculate the difference in velocities.
            Vector3 velocityDifference = A.Particle.Velocity - B.Particle.Velocity;

            // Get the opposite values of the difference.
            //velocityDifference *= -1.0f;

            // Get the difference in positions.
            Vector3 positionDifference = A.Particle.Position - B.Particle.Position;

            // Normalize the difference in positions.
            positionDifference.Normalize();

            // Find the scalar(dot) product of the difference in velocities and difference in positions normalized.
            return Vector3.Dot(velocityDifference, positionDifference);
        }

        // Create a contact and add it to the contacts list.
        public void AddContact(Vector3 _position, Vector3 _normal, float _restitution, int _id)
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
            //Particle3D particleA = A.Particle;
            //Particle3D particleB = B.Particle;
            //Vector2 newVelocity = particleA.Velocity;

            //for (int currentContact = 0; currentContact < contactCount; currentContact++)
            //{
            //    // TODO: Closing velocity is overlap of both objects?
            //    ClosingVelocity = CalculateClosingVelocity(Contacts[currentContact].normal);
            //    Debug.DrawRay(Contacts[currentContact].position, Contacts[currentContact].position + Contacts[currentContact].normal, Color.yellow);
            //    newVelocity = ClosingVelocity.normalized * Contacts[currentContact].restitution;

            //    //particleA.AddForce(ClosingVelocity.normalized * Contacts[currentContact].restitution);
            //    //particleB.AddForce(ClosingVelocity * Contacts[currentContact].restitution);
            //}

            //particleA.Velocity = -newVelocity;
            ////particleB.Velocity = -newVelocity;
            for (int currentContact = 0; currentContact < ContactCount; currentContact++)
            {
                Contact contact = Contacts[currentContact];

                float separatingVelocity = CalculateClosingVelocity();

                // Stationary or separating contact.
                if (separatingVelocity > 0)
                {
                    return;
                }

                // Calculate new separating velocity.
                float newSeparatingVelocity = -separatingVelocity * contact.restitution;

                float deltaVelocity = newSeparatingVelocity - separatingVelocity;

                float totalInverseMass = A.Particle.MassInverse + B.Particle.MassInverse;

                if (totalInverseMass <= 0)
                {
                    return;
                }

                float impulse = deltaVelocity / totalInverseMass;

                // reflection formula
                // r = v - 2(v dot n^)n^

                Vector3 impulsePerInverseMass = contact.normal * impulse;

                A.Particle.Velocity += impulsePerInverseMass * A.Particle.MassInverse;

                B.Particle.Velocity += impulsePerInverseMass * -B.Particle.MassInverse;
            }


            return;
        }

        // Collision hull a accessor.
        public CollisionHull3D A { get => a; set => a = value; }

        // Collision hull b accessor.
        public CollisionHull3D B { get => b; set => b = value; }

        // Collision status accessor.
        public bool Status { get => status; set => status = value; }

        // Collsion contact count accessor.
        public int ContactCount { get => contactCount; set => contactCount = value; }

        // Contacts of the collision.
        public Contact[] Contacts { get => contacts; set => contacts = value; }

        // Closing velocity of the collision.
        public Vector3 ClosingVelocity { get => closingVelocity; set => closingVelocity = value; }
    }
    // END TODO

    // Different types of collision hulls.
    public enum CollisionHullType3D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }

    // Different layers for collision.
    public enum CollisionLayer
    {
        player,
        enemy,
        projectile
    }

    // Constructor.
    protected CollisionHull3D(CollisionHullType3D type)
    {
        // Set the type to the given type.
        Type = type;

        return;
    }

    void Awake()
    {
        // Assign the Particle3D component.
        Particle = gameObject.GetComponent<Particle3D>();

        return;
    }

    // Update for physics.
    public void FixedUpdate()
    { 

        // Iterate through every CollisionHull3D in the game.
        foreach (CollisionHull3D hull in GameObject.FindObjectsOfType<CollisionHull3D>())
        {
            // Ignore collisions with self.
            if (hull == this)
            {
                continue;
            }

            // Players can only collide with enemies.
            bool playerCollision = Layer == CollisionLayer.player && hull.Layer == CollisionLayer.enemy;
            // Enemies can collide with anything.
            bool enemyCollision = Layer == CollisionLayer.enemy;
            bool projectileCollision = Layer == CollisionLayer.projectile && hull.Layer == CollisionLayer.enemy;

            // Check if collision can happen.
            if (true || playerCollision || enemyCollision || projectileCollision)
            {
                // Check for collision and collect collision data.
                if (TestCollision(this, hull, ref collision))
                {
                    // Resolve the collision.

					if (hull.Layer == CollisionLayer.projectile && this.Layer == CollisionLayer.player)
					{
						Debug.Log("lane");
						this.GetComponent<Particle3D>().NormalActive = true;
						Vector3 velocity = GetComponent<Particle3D>().Velocity;
						velocity.y = 0.0f;
						GetComponent<Particle3D>().Velocity = velocity;
					}
					else if (hull.Layer == CollisionLayer.player && this.Layer == CollisionLayer.projectile)
					{
						Debug.Log("ball");
						hull.GetComponent<Particle3D>().NormalActive = true;
						Vector3 velocity = hull.GetComponent<Particle3D>().Velocity;
						velocity.y = 0.0f;
						hull.GetComponent<Particle3D>().Velocity = velocity;
					}
					else
					{
						Debug.Log("other");
						collision.ResolveCollision();
					}

                    collided = true;

                    if (playerCollision)
                    {
                        if (hull.gameObject != null)
                        {
                            //hull.GetComponent<Asteroid>().DecrementLife();
                        }
                        if (gameObject != null)
                        {
                            //GameController.instance.PlayerHit();
                        }
                    }
                    else if (projectileCollision)
                    {
                       // GameController.instance.IncreaseScore();

                        if (hull.gameObject != null)
                        {
                            //hull.GetComponent<Asteroid>().DecrementLife();
                        }
                        if (gameObject != null)
                        {
                            //Destroy(gameObject);
                        }
                    }

                }
            }
        }

        if (collided)
        {
            collided = false;
           // GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            //gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }

        return;
    }

    // Test collision with given hulls and collect collision data.
    public static bool TestCollision(CollisionHull3D left, CollisionHull3D right, ref Collision collision)
    {
        // Other hull is a circle.
        if (right.Type == CollisionHullType3D.hull_circle)
        {
            // Test for collision with a circle and collect collision data.
            return left.TestCollisionVsCircle(right as CircleCollisionHull3D, ref collision);
        }
        // Other hull is an AABB.
        else if (right.Type == CollisionHullType3D.hull_aabb)
        {
            // Test for collision with an AABB and collect collision data.
            return left.TestCollisionVsAABB(right as AxisAlignedBoundingBoxHull3D, ref collision);
        }
        // Other hull is an OBB.
        else if (right.Type == CollisionHullType3D.hull_obb)
        {
            // Test for collision with an OBB and collect collision data.
            return left.TestCollisionVsOBB(right as ObjectBoundingBoxHull3D, ref collision);
        }

        return false;
    }


    /********************************
    **  Abstract Collision Checks  **
    ********************************/

    // Check collision of this vs circle.
    public abstract bool TestCollisionVsCircle(CircleCollisionHull3D other, ref Collision c);
    // Check collision of this vs AABB.
    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D other, ref Collision c);
    // Check collision of this vs OBB.
    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull3D other, ref Collision c);


    /****************
    **  Accessors  ** 
    ****************/

    // Particle accessor.
    public Particle3D Particle { get; set; }

    public Collision collision = new Collision();

    // Accessor for collision hull type.
    public CollisionHullType3D Type { get; }

    // Layer of collision.
    [SerializeField]
    private CollisionLayer layer;

    public CollisionLayer Layer { get { return layer; } set { layer = value; } }
}


[CustomEditor(typeof(CollisionHull3D))]
public class CollisionEditor3D : Editor
{
    public void OnSceneGUI()
    {
        // Get the circle hull attached to this script.
        CollisionHull3D hull = (CollisionHull3D)target;

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        Particle3D particle = hull.GetComponent<Particle3D>();

        // Create a color.
        Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        //Handles.color = purple;

        if (hull.collision.Status)
        {
            float angle = Mathf.Atan2(hull.collision.ClosingVelocity.y, hull.collision.ClosingVelocity.x);
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0.0f, 0.0f, angle);
            Handles.ArrowHandleCap(0, hull.collision.A.Particle.Position, rot, 2.0f, EventType.Repaint);
        }
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}