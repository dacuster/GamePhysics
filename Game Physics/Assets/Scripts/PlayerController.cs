using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;
    Particle2D particle;
    public float shotPower = 0.0f;
    public Plane lane = new Plane(Vector3.up, Vector3.zero);

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Particle2D>();
    }

    private void FixedUpdate()
    {
        PlayerControls();
        DrawLine();
    }

    public void PlayerControls()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
           float width = Screen.width * 0.5f;
           float height = 0.0f;

           Vector2 mousePosition = Input.mousePosition;

           Vector2 direction = mousePosition - new Vector2(width, height);

           float distance = direction.magnitude;

           shotPower = distance;

           GetComponent<Particle3D>().AddForce(new Vector3(direction.x, 0, direction.y) * 5.44311f);











            //var rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

            //float enter;

            //if(lane.Raycast(rayCast, out enter))
            //{
            //    // Set hitPoint using raycast
            //    Vector3 hitPoint = rayCast.GetPoint(enter);

            //    // Calsulate mouseDirection
            //    Vector3 mouseDirection = hitPoint - GetComponent<Particle3D>().Position;

            //    // Normalize mouseDirection
            //    mouseDirection = mouseDirection.normalized;

            //    Vector3 mousePos = Input.mousePosition;
            //    mousePos.z = 15.0f;
            //    mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //    Vector3 particlePosition = GetComponent<Particle3D>().Position;

            //    particlePosition.z = 15.0f;

            //    shotPower = mousePos - particlePosition;

            //    shotPower *= shotPower.magnitude * 5.0f;
            //    //shotPower = shotPower.normalized;

            //    // Add for based off mouse direction
            //    GetComponent<Particle3D>().AddForce(new Vector3(shotPower.x, 0, shotPower.y));
            //}

        }
    }

    private void DrawLine()
    {
        //Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 15;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Ray ray = Camera.main.ScreenPointToRay(mousePos);

        //Vector3 mousePos = Vector3.zero;
        //if (hasHit)
        //{
        //    mousePos = hit.point;
        //    //mousePos.z = 0;
        //}

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, GetComponent<Particle3D>().Position);
        lineRenderer.SetPosition(1, mousePos);// ray.GetPoint(15.0f));
    }
}
