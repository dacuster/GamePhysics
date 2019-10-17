using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Particle2D particle;
    private float projectileLife = 1.5f;
    private float lifeTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        particle.Velocity = particle.CalculateDirection() * 20.0f;
        particle.AddForce(particle.CalculateDirection());
        
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= projectileLife)
        {
            Destroy(gameObject);
        }
        
    }
}
