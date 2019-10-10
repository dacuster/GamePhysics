﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public GameObject asteroid;
    public List<GameObject> asteroidList;
    public Vector2 spawnValue;
    public int asteroidCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public Text scoretext;
    public Text lifeText;
    float score = 0.0f;
    int lives = 3;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        scoretext.text = "Score: " + score;
        lifeText.text = "Lives: " + lives;

        if(lives <= 0)
        {
            Debug.Log("Dead");
        }

    }

    public void IncreaseScore()
    {
        score += 50;
    }

    public void PlayerHit()
    {
        lives -= 1;
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(spawnWait);
        while (true)
        {
            for (int i = 0; i < asteroidCount; ++i)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(-spawnValue.x, spawnValue.x), Random.Range(-spawnValue.y, spawnValue.y));
                Quaternion spawnRotation = Quaternion.identity;
                GameObject newAsteroid = Instantiate(asteroid, spawnPosition, spawnRotation);
                newAsteroid.GetComponent<Particle2D>().Position = spawnPosition;
                asteroidList.Add(newAsteroid);
                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);
        }
        
      
    }

}
