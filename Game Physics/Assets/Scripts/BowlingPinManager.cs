using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    public static BowlingPinManager instance = null;
    
    public List <GameObject> openPinsList = new List<GameObject>();

    [SerializeField]
    private List<bool> closedPinsList = new List<bool>();
    public int pinLength = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Controls();

        if (openPinsList.Count == 0)
        {
            Debug.Log("Empty");
        }
    }

    private void Controls()
    {
        if(Input.GetKey(KeyCode.D))
        {
            foreach (GameObject pin in openPinsList)
            {
                openPinsList.Remove(pin);
                closedPinsList.Add(pin);
            }
        }
    }
}
