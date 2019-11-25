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

        }

        if (Input.GetKey(KeyCode.DownArrow))
        {

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            
        }

        if (Input.GetKey(KeyCode.R))
        {

        }

        if(Input.GetKeyDown(KeyCode.E))
        {
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }
}
