using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public GameObject asteroid;
    public GameObject explosion;
    public GameObject gameCanvas;
    public GameObject deadCanvas;
    public GameObject winCanvas;
    public List<GameObject> asteroidList;
    public Vector2 spawnValue;
    public int asteroidCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public Text scoretext;
    public Text lifeText;
    float score = 0.0f;
    public int lives = 3;
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
        gameCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        scoretext.text = "Score: " + score;
        lifeText.text = "Lives: " + lives;

        if(score >= 1000)
        {
            gameCanvas.SetActive(false);
            winCanvas.SetActive(true);
        }
    }

    public void IncreaseScore()
    {
        score += 50;
    }

    public void PlayerHit()
    {
        lives -= 1;
        if (lives <= 0)
        {
            Dead();
            gameCanvas.SetActive(false);
            deadCanvas.SetActive(true);
        }
    }

    public void PlayExplosion()
    {
        Instantiate(explosion, GameObject.Find("Player").transform);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene_Game");
    }

    public void Dead()
    {
        Particle2D particle = GameObject.Find("Player").GetComponent<Particle2D>();
        particle.AngularVelocity = 0;
        particle.AngularAcceleration = 0;
        particle.Acceleration = Vector2.zero;
        particle.Velocity = Vector2.zero;
        PlayExplosion();
        Destroy(GameObject.Find("Player"));
        // Player explosion
        
    }
    
    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(spawnWait);
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
