using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Particle2D))]
public class Asteroid : MonoBehaviour
{
    // Asteroid lives.
    private int lives = 1;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        return;
    }

    // Initialize the asteroid.
    private void Initialize()
    {
        // Get the particle.
        Particle2D particle = GetComponent<Particle2D>();

        // Set the initial velocity to be random.
        particle.Velocity = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));

        return;
    }

    // Decrease a life.
    public void DecrementLife()
    {
        Lives--;

        if (Lives == 0)
        {
            Destroy(gameObject);
        }

        return;
    }


    // Lives accessor.
    public int Lives { get => lives; set => lives = value; }
}
