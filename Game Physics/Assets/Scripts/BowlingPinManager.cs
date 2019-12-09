using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPinManager : MonoBehaviour
{
    public static BowlingPinManager instance = null;
    
    public List <GameObject> openPinsList = new List<GameObject>();

    int moved = 0;
    Vector3 newPosition;
    public int pinLength = 10;


    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject pin in openPinsList)
        {
            Quaternion pinRotation = pin.GetComponent<Particle3D>().Rotation.GetUnityQuaternion();
            Quaternion knockedOverRotation = new Quaternion(0,0,0,1);
            float pinAngle = Quaternion.Angle(pinRotation, knockedOverRotation);

            Vector3 force = new Vector3(0, 1, 0);
            Vector3 momentArm = new Vector3(0, 1, 1);
            //pin.GetComponent<Particle3D>().ApplyTorque(momentArm, force);

            //Vector3 pinPosistion = pin.GetComponent<Particle3D>().Position;
            

            //if (pinPosistion.x != newPosition.x || pinPosistion.y != newPosition.y || pinPosistion.z != newPosition.z)
            //{
            //    Debug.Log("MOVED");
            //}

            if (pinAngle >= 90 || pinAngle <= -90)
            {
                Debug.Log("Knocked Over");
                openPinsList.Remove(pin);
            }
        }

        if(Input.GetKey(KeyCode.W))
        {
            GameController.instance.WinState();
        }

        if (Input.GetKey(KeyCode.L))
        {
            GameController.instance.LoseState();
        }

        if (openPinsList.Count == 0)
        {
            Debug.Log("Win");
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
            Debug.Log(openPinsList.Count);
            Debug.Log("LOSE");
        }
    }
}
