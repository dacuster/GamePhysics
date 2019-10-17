using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    // Camera height
    float camHeight;

    // Camera width
    float camWidth;

    // The particle.
    Particle2D particle;
   
    // Start is called before the first frame update
    public void Start()
    {
        // Get camera height
        camHeight = Camera.main.orthographicSize * 2 + 5;

        // Get camera width
        camWidth = camHeight * Camera.main.aspect;

        particle = GetComponent<Particle2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = particle.Position;

        if (position.x > camWidth)
        {
            position += Vector2.left * camWidth * 2.0f;
            position.x += 2.0f;
        }
        else if (position.x < -camWidth)
        {
            position += Vector2.right * camWidth * 2.0f;
            position.x -= 2.0f;
        }

        if (position.y > camHeight)
        {
            position += Vector2.down * camHeight * 2.0f;
            position.y += 2.0f;

        }
        else if (position.y < -camHeight)
        {
            position += Vector2.up * camHeight * 2.0f;
            position.y -= 2.0f;
        }

        GetComponent<Particle2D>().Position = position;
    }
}
