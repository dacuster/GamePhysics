using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    // Camera height
    float camHeight;

    // Camera width
    float camWidth;
   
    // Start is called before the first frame update
    public void Start()
    {
        // Get camera height
        camHeight = Camera.main.orthographicSize * 2 + 5;

        // Get camera width
        camWidth = camHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Get player gameobject
        GameObject player = GameObject.Find("Player");

        // Set player position
        Vector3 playerPos = player.transform.position;

        // loop through asteroid list
        for (int i = 0; i < GameController.instance.asteroidList.Count; i++)
        {

            Vector3 asteroidPos = (GameController.instance.asteroidList[i].transform.position);
           
            // Check if asteroid pos x > camera Width
            if (asteroidPos.x > camWidth)
            {
                GameController.instance.asteroidList[i].GetComponent<Particle2D>().Position = new Vector2((camWidth * -1) + 2.0f, asteroidPos.y);
            }

            // Check if asteroid pos y > camera height
            else if (asteroidPos.y > camHeight)
            {
                GameController.instance.asteroidList[i].GetComponent<Particle2D>().Position = new Vector2(asteroidPos.x, (camHeight * -1) + 2.0f);
            }

            // Check if asteroid pos x < -camera width
            else if (asteroidPos.x < -camWidth)
            {
                GameController.instance.asteroidList[i].GetComponent<Particle2D>().Position = new Vector2((camWidth * 1), asteroidPos.y);
            }

            // Check if asteroid pos y < -camera height
            else if (asteroidPos.y < -camHeight)
            {
                GameController.instance.asteroidList[i].GetComponent<Particle2D>().Position = new Vector2(asteroidPos.x, (camHeight * 1));
            }
        }

        // Check if player pos x > camera Width
        if (player.GetComponent<Particle2D>().Position.x > camWidth)
        {
            player.GetComponent<Particle2D>().Position = new Vector2((camWidth * -1) + 2.0f, playerPos.y);

        }

        // Check if player pos y > camera height
        else if (player.GetComponent<Particle2D>().Position.y > camHeight)
        {
            player.GetComponent<Particle2D>().Position = new Vector2(playerPos.x, (camHeight * -1) + 2.0f);
        }

        // Check if player pos x < -camera width
        else if (player.GetComponent<Particle2D>().Position.x < -camWidth)
        {
            player.GetComponent<Particle2D>().Position = new Vector2((camWidth * 1), playerPos.y);

        }

        // Check if player pos y < -camera height
        else if (player.GetComponent<Particle2D>().Position.y < -camHeight)
        {
            player.GetComponent<Particle2D>().Position = new Vector2(playerPos.x, (camHeight * 1));
        }
    }
}
