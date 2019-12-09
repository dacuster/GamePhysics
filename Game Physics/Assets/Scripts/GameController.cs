using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public Text pins;
    public Text shotPower;
    public GameObject Lose;
    public GameObject Win;
    public GameObject start;

    public bool gameStarted = false;
    public void Awake()
    {
        instance = this;
    }

    public void Start()
    { 
        Lose.SetActive(false);
        Win.SetActive(false);
        Title();
    }
    private void FixedUpdate()
    {
        float power = PlayerController.instance.shotPower;

        Vector2 upperLeft = new Vector2(0.0f, Screen.height);
        float maxPower = upperLeft.magnitude;

        power = (power / maxPower) * 100.0f;

        pins.text = "Pins: " + BowlingPinManager.instance.openPinsList.Count;
        shotPower.text = "Shot Power: " + power;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene_Game");
    }

    public void LoseState()
    {
        Lose.SetActive(true);
    }

    public void WinState()
    {
        Win.SetActive(true);
    }

    public void Title()
    {
        pins.enabled = false;
        shotPower.enabled = false;
        start.SetActive(true);
    }

    public void EndTitle()
    {
        gameStarted = true;
        pins.enabled = true;
        shotPower.enabled = true;
        start.SetActive(false);
    }
}
