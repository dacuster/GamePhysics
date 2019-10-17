using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;
    Particle2D particle;
    bool movingRight = false;
    bool movingLeft = false;
    float maxVelocity = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();

        particle.Velocity = Vector3.ClampMagnitude(particle.Velocity, maxVelocity);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerControls()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            particle.AddForce(particle.CalculateDirection() * 4);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            particle.AddForce(particle.CalculateDirection() * -4);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            particle.ApplyTorque(new Vector2(0, 1), new Vector2(-5.0f, 0.0f));
            movingRight = false;
            movingLeft = true;
            if(movingLeft == true)
            {
                particle.ApplyTorque(new Vector2(0, 1), new Vector2(-10.0f, 0.0f));
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            particle.ApplyTorque(new Vector2(0, 1), new Vector2(5.0f, 0.0f));

            movingRight = true;
            movingLeft = false;
            if (movingRight == true)
            {
                particle.ApplyTorque(new Vector2(0, 1), new Vector2(10.0f, 0.0f));
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            particle.GetComponent<Particle2D>().AngularVelocity = 0;
            particle.GetComponent<Particle2D>().AngularAcceleration = 0;
            particle.GetComponent<Particle2D>().Acceleration =  Vector2.zero;
            particle.GetComponent<Particle2D>().Velocity = Vector2.zero;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            GameController.instance.PlayExplosion();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnProjectile.instance.Fire();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameController.instance.IncreaseScore();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameController.instance.PlayerHit();
        }
    }
}
