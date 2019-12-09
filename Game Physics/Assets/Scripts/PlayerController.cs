using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;
    public float shotPower = 0.0f;
    public Plane lane = new Plane(Vector3.up, Vector3.zero);

    public bool hasShot = false;
    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if(GameController.instance.gameStarted == true)
        {
            if (!hasShot)
            {
                PlayerControls();
                DrawLine();
            }
        }
    }

    public void PlayerControls()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
			Particle3D particle = GetComponent<Particle3D>();
			float width = Screen.width * 0.5f;
			float height = 0.0f;

			Vector2 mousePosition = Input.mousePosition;

			Vector2 direction = mousePosition - new Vector2(width, height);

			float distance = direction.magnitude;
			float radius = GetComponent<CircleCollisionHull3D>().Radius;

			shotPower = distance;

			Vector3 force = new Vector3(direction.x, 0, direction.y) * particle.Mass;

			particle.AddForce(force);
            particle.ApplyTorque(new Vector3(particle.Position.x, particle.Position.y + radius, particle.Position.z), force /100.0f);
            hasShot = true;
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    private void DrawLine()
    {
        //Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 15;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, GetComponent<Particle3D>().Position);
        lineRenderer.SetPosition(1, mousePos);// ray.GetPoint(15.0f));
    }
}
