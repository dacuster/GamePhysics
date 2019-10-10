using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAsteroid : MonoBehaviour
{
    Particle2D particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
        particle.Velocity = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)) * 1.0f;
        particle.AddForce(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }
}
