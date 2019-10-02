using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract prevent anything from instantiating it.
public abstract class CollisionHull2D : MonoBehaviour
{
	public struct Contact
	{
		Vector2 point;
		Vector2 normal;
		float restitution;
	}
			
	public CollisionHull2D a = null, b = null;
	public Contact[] contact = new Contact[4];
	public int contactCount = 0;
	public bool status = false;
			
	public Vector2 closingVelocity;
		
	
    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }

    public CollisionHullType2D Type { get; }


    protected CollisionHull2D(CollisionHullType2D _type)
    {
        Type = _type;
    }

    protected Particle2D particle;


    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }


    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {
		// Change return to collision. Check status if it actually collided.
		
        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleCollisionHull2D other, ref Collision c);
    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c);
    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c);
}
