using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    [SerializeField]
    private List <GameObject> pins = new List<GameObject>();
    private int pinLength = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //OnMouseDown();
        Controls();

        if (pinLength == 0)
        {
            Debug.Log("Empty");
        }
    }

    private void Controls()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            foreach (GameObject pin in  pins)
            {
                pins.Remove(pin);
            }
        }
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
