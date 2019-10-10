using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;
    Particle2D particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
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
            particle.GetComponent<Particle2D>().AngularVelocity = 0;
            particle.GetComponent<Particle2D>().AngularAcceleration = 0;
            particle.GetComponent<Particle2D>().Acceleration =  Vector2.zero;
            particle.GetComponent<Particle2D>().Velocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.E))
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
