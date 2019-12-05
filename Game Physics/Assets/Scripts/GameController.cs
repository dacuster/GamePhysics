using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public Text pins;
    public Text shotPower;

    
    private void FixedUpdate()
    {
        float power = PlayerController.instance.shotPower;

        Vector2 upperLeft = new Vector2(0.0f, Screen.height);
        float maxPower = upperLeft.magnitude;

        power = (power / maxPower) * 100.0f;

        pins.text = "Pins: " + BowlingPinManager.instance.openPinsList.Count;
        shotPower.text = "Shot Power: " + power;
        
    }

}
