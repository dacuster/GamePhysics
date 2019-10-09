using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Particle2D particle;
    private float projectileLife = 0.75f;
    private float lifeTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        particle.Velocity = particle.CalculateDirection();
        particle.AddForce(particle.CalculateDirection() * 10.0f);
        
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= projectileLife)
        {
            Destroy(gameObject);
        }
        
    }
}
