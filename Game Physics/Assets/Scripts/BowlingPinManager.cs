using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pins;
    private int pinLength = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Controls();

        //foreach (GameObject pin in pins)
        //{
        //    Destroy(pin);
        //    pinLength--;
        //}

        if (pinLength == 0)
        {
            Debug.Log("Empty");
        }
    }

    private void Controls()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Destroy(gameObject);
            Debug.Log("Left Mouse");
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Right Mouse");
        }
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
