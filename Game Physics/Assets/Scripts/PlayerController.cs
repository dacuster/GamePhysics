using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Particle2D particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerControls()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            particle.AddForce(particle.CalculateDirection());
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            particle.AddForce(particle.CalculateDirection() * -1);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            particle.ApplyTorque(new Vector2(0, 1), new Vector2(-1.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            particle.ApplyTorque(new Vector2(0, 1), new Vector2(1.0f, 0.0f));
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //particle.angularVelocity = 0;
            //angularAcceleration = 0;
            //acceleration = new Vector2(0.0f, 0.0f);
            //velocity = new Vector2(0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.E))
        {
            SpawnProjectile.instance.Fire();
        }
    }
}
