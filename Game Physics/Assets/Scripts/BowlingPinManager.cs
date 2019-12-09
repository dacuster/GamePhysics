using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    public static BowlingPinManager instance = null;
    
    public List <GameObject> openPinsList = new List<GameObject>();
    int moved = 0;
    public int pinLength = 10;


    public void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject pin in openPinsList)
        {
            Vector3 notMoving = Vector3.zero;
            if(pin.GetComponent<Particle3D>().Velocity != notMoving)
            {
                Debug.Log("Moved");
                openPinsList.Remove(pin);
            }
        }

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
        if (openPinsList.Count < 10 && openPinsList.Count != 0)
        {
            GameController.instance.LoseState();
        }
    }
}
