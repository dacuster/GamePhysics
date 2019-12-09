using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    public static BowlingPinManager instance = null;
    
    public List<GameObject> openPinsList = new List<GameObject>();
    public List<GameObject> closedPinsList = new List<GameObject>();
    int moved = 0;
    public int pinLength = 10;

    public void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // Place pins in a closed list for later removal. Objects cannot be removed from a list while iterating through it.
        foreach (GameObject pin in openPinsList)
        {
            if(pin.GetComponent<Particle3D>().Velocity != Vector3.zero)
            {
                closedPinsList.Add(pin);
            }
        }

        // Remove all the pins in the open list that were placed in the closed list.
        foreach (GameObject pin in closedPinsList)
        {
            openPinsList.Remove(pin);
        }

        // Clear the closed list.
        closedPinsList.Clear();

        if (openPinsList.Count == 0)
        {
            GameController.instance.WinState();
        }

        if(PlayerController.instance.hasShot == true)
        {
            StartCoroutine(Wait());
        }
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        if (openPinsList.Count > 0)
        {
            GameController.instance.LoseState();
        }
    }
}
