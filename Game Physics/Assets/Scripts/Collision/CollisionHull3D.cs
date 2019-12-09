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

    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float restitutionCoefficient = 0.0f;

    // Video tutorial circle collision handler. https://www.youtube.com/watch?v=LPzyNOHY3A4
    // TODO: Comment for lab 5.
    // Collision data.
    public class Collision
    {
        // Point of contact in collision.
        public struct Contact
        {
            //dos line endings
            // Position of the contact.
            public Vector3 position;
            // Contact normal.
            public Vector3 normal;
            // Coefficient of restitution of the contact.
            public float restitution;
            // Penetration amount.
            public float penetration;
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

        // Calculate the separating speed of the 2 colliding objects for the given contact normal.
        public float CalculateSeparatingSpeed(Vector3 normal)
        {
            // Calculate the difference in velocities.
            Vector3 velocityDifference = A.Particle.Velocity - B.Particle.Velocity;

            // Find the scalar(dot) product of the difference in velocities and the contact normal.
            return Vector3.Dot(velocityDifference, normal);
        }

        // Create a contact and add it to the contacts list.
        public void AddContact(Vector3 _position, Vector3 _normal, float _restitution, float _penetration, int _id)
        {
            // Create a new contact.
            Contact contact;

            // Set the position of the contact.
            contact.position = _position;

            // Set the normal of the contact.
            contact.normal = _normal;

            // Set the restitution of the contact.
            contact.restitution = _restitution;

            // Set the penetration of the contact.
            contact.penetration = _penetration;

            // Add this contact to the list of contacts based on the current contact id.
            Contacts[_id] = contact;

            return;
        }

        public void ResolveCollision()
        {
            // Loop through each point of contact in the collision.
            for (int currentContact = 0; currentContact < ContactCount; currentContact++)
            {
                // Get the current point of contact.
                Contact contact = Contacts[currentContact];

                // Calculate the separating velocity
                float separatingSpeed = CalculateSeparatingSpeed(contact.normal);

                // Stationary or separating contact.
                if (separatingSpeed > 0)
                {
                    return;
                }

                // Calculate closing speed.
                float closingSpeed = -separatingSpeed * contact.restitution;

                // Calculate the change in speed.
                float deltaSpeed = closingSpeed - separatingSpeed;

                // Calculate sum of inverse masses of both hulls.
                float totalInverseMass = A.Particle.MassInverse + B.Particle.MassInverse;

                // Can't have negative or zero mass.
                if (totalInverseMass <= 0)
                {
                    return;
                }

                // Calculate linear impulse.
                float impulse = deltaSpeed / totalInverseMass;

                // Calculate impulse per inverse mass about the contact normal.
                Vector3 impulsePerInverseMass = contact.normal * impulse;

                // Calculate displacement for each particle.
                Vector3 displacementA = A.Particle.Velocity.normalized * contact.penetration * -0.5f;
                Vector3 displacementB = B.Particle.Velocity.normalized * contact.penetration * -0.5f;

                // Dynamic collision response.
                if (A.Type == CollisionType.dynamicCollision)
                {
                    // Apply displacement in opposite direction.
                    A.Particle.Position += displacementA;

                    // Collision against static object.
                    if (B.Type == CollisionType.staticCollision)
                    {
                        // Apply displacement in opposite direction again since the other object isn't moving.
                        A.Particle.Position += displacementA;

                        if (B.Collider == ColliderType.ground)
                        {
                            A.Particle.NormalActive = true;
                        }
                        else if (B.Collider == ColliderType.wall)
                        {

                        }
                        else
                        {
                            // Calculate reflection velocity along normal.
                            impulsePerInverseMass = A.Particle.Velocity - 2.0f * Vector3.Dot(A.Particle.Velocity, contact.normal) * contact.normal;

                            // Scale reflection velocity normalized by impulse.
                            impulsePerInverseMass = impulsePerInverseMass.normalized * impulse;

                            // Directly change the velocity to be the reflection since we are "bouncing" off the collision.
                            A.Particle.Velocity = impulsePerInverseMass * A.Particle.MassInverse;
                        }
                    }
                    else
                    {
                        //A.Particle.AddForce(impulsePerInverseMass);
                        A.Particle.Velocity += impulsePerInverseMass * A.Particle.MassInverse;
                    }
                }

                // Dynamic collision response.
                if (B.Type == CollisionType.dynamicCollision)
                {
                    // Apply displacement in opposite direction.
                    B.Particle.Position += displacementB;

                    // Collision against static object.
                    if (A.Type == CollisionType.staticCollision)
                    {
                        // Apply displacement in opposite direction again since the other object isn't moving.
                        B.Particle.Position += displacementB;

                        // reflection formula    r = v - 2(v dot n^)n^
                        // Calculate reflection velocity along normal.
                        impulsePerInverseMass = B.Particle.Velocity - 2.0f * Vector3.Dot(B.Particle.Velocity, contact.normal) * contact.normal;

                        // Scale reflection velocity normalized by impulse.
                        impulsePerInverseMass = impulsePerInverseMass.normalized * impulse;

                        // Directly change the velocity to be the reflection since we are "bouncing" off the collision.
                        B.Particle.Velocity = impulsePerInverseMass * B.Particle.MassInverse;
                    }
                    else
                    {
                        //B.Particle.AddForce(-impulsePerInverseMass);
                        B.Particle.Velocity += impulsePerInverseMass * -A.Particle.MassInverse;
                    }
                }




                //// Rotation Attempt
                //Vector3 contactRelativeA = contact.position - A.Particle.Position;
                //Vector3 contactRelativeB = contact.position - B.Particle.Position;

                //Vector3 torquePerUnitImpulseA = Vector3.Cross(contactRelativeA, contact.normal);
                //Vector3 torquePerUnitImpulseB = Vector3.Cross(contactRelativeB, contact.normal);

                //Vector3 rotationPerUnitImpulseA = A.Particle.InertiaInv.MultiplyPoint3x4(torquePerUnitImpulseA);
                //Vector3 rotationPerUnitImpulseB = B.Particle.InertiaInv.MultiplyPoint3x4(torquePerUnitImpulseB);

                //Vector3 velocityPerUnitImpulseA = Vector3.Cross(rotationPerUnitImpulseA, contactRelativeA);
                //Vector3 velocityPerUnitImpulseB = Vector3.Cross(rotationPerUnitImpulseB, contactRelativeB);

                //float deltaAngularVelocity = Vector3.Dot(velocityPerUnitImpulseA, contact.normal);

                //deltaAngularVelocity += Vector3.Dot(velocityPerUnitImpulseB, contact.normal);

                ////Matrix4x4 contactBasisTranspose = CalculateContactBasis(contact.normal).transpose;

                ////velocityPerUnitImpulseA = contactBasisTranspose.MultiplyPoint3x4(velocityPerUnitImpulseA);
                ////velocityPerUnitImpulseB = contactBasisTranspose.MultiplyPoint3x4(velocityPerUnitImpulseB);

                //Vector3 angularVelocityA = Vector3.Cross(A.Particle.AngularVelocity, contactRelativeA) + A.Particle.AngularVelocity;
                //Vector3 angularVelocityB = Vector3.Cross(B.Particle.AngularVelocity, contactRelativeB) + B.Particle.AngularVelocity;


                //A.Particle.ApplyTorque(contact.position, velocityPerUnitImpulseA);
                ////B.Particle.ApplyTorque(contact.position, velocityPerUnitImpulseB);
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
    }
    // END TODO

    // Different types of collision hulls.
    public enum CollisionHullType3D
    {
        hull_circle,
        hull_aabb,
        hull_obb,
        hull_cylinder
    }

    // Different layers for collision.
    public enum CollisionLayer
    {
        player,
        enemy,
        projectile
    }

    public enum CollisionType
    {
        dynamicCollision,
        staticCollision
    }

    public enum ColliderType
    {
        other,
        ground,
        wall
    }

    // Constructor.
    protected CollisionHull3D(CollisionHullType3D type)
    {
        // Set the type to the given type.
        HullType = type;

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

					//if (hull.Layer == CollisionLayer.projectile && this.Layer == CollisionLayer.player)
					//{
					//	Debug.Log("lane");
					//	this.GetComponent<Particle3D>().NormalActive = true;
					//	Vector3 velocity = GetComponent<Particle3D>().Velocity;
					//	velocity.y = 0.0f;
					//	GetComponent<Particle3D>().Velocity = velocity;
					//}
					//else if (hull.Layer == CollisionLayer.player && this.Layer == CollisionLayer.projectile)
					//{
					//	Debug.Log("ball");
					//	hull.GetComponent<Particle3D>().NormalActive = true;
					//	Vector3 velocity = hull.GetComponent<Particle3D>().Velocity;
					//	velocity.y = 0.0f;
					//	hull.GetComponent<Particle3D>().Velocity = velocity;
					//}
					//else
					//{
					//	Debug.Log("other");
					//	collision.ResolveCollision();
					//}

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
        if (right.HullType == CollisionHullType3D.hull_circle)
        {
            // Test for collision with a circle and collect collision data.
            return left.TestCollisionVsCircle(right as CircleCollisionHull3D, ref collision);
        }
        // Other hull is an AABB.
        else if (right.HullType == CollisionHullType3D.hull_aabb)
        {
            // Test for collision with an AABB and collect collision data.
            return left.TestCollisionVsAABB(right as AxisAlignedBoundingBoxHull3D, ref collision);
        }
        // Other hull is an OBB.
        else if (right.HullType == CollisionHullType3D.hull_obb)
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
    public CollisionHullType3D HullType { get; }

    // Type of collision.
    [SerializeField]
    private CollisionType type;

    public CollisionType Type { get { return type; } set { type = value; } }

    // Layer of collision.
    [SerializeField]
    private CollisionLayer layer;

    public CollisionLayer Layer { get { return layer; } set { layer = value; } }

    // Collider types.
    [SerializeField]
    private ColliderType colliderType;
    public ColliderType Collider {  get { return colliderType; } set { colliderType = value; } }
}


[CustomEditor(typeof(CollisionHull3D))]
public class CollisionEditor3D : Editor
{
    public void OnSceneGUI()
    {
        // Get the circle hull attached to this script.
        //CollisionHull3D hull = (CollisionHull3D)target;

        // Get the particle component since it isn't loaded until runtime. (For use in scene editor at all times.)
        //Particle3D particle = hull.GetComponent<Particle3D>();

        // Create a color.
        //Color purple = CreateColor(112.0f, 0.0f, 255.0f);

        // Change gizmo drawing color.
        //Handles.color = purple;

        //if (hull.collision.Status)
        //{
        //    float angle = Mathf.Atan2(hull.collision.ClosingVelocity.y, hull.collision.ClosingVelocity.x);
        //    Quaternion rot = new Quaternion();
        //    rot.eulerAngles = new Vector3(0.0f, 0.0f, angle);
        //    Handles.ArrowHandleCap(0, hull.collision.A.Particle.Position, rot, 2.0f, EventType.Repaint);
        //}
    }

    // Create colors from 0-255 values.
    private Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}