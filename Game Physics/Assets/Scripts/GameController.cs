using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject asteroid;
    public Vector2 spawnValue;
    public int asteroidCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(spawnWait);
        while (true)
        {
            for (int i = 0; i < asteroidCount; ++i)
            {
                Debug.Log(i);
                Vector2 spawnPosition = new Vector2(Random.Range(-spawnValue.x, spawnValue.x), Random.Range(-spawnValue.y, spawnValue.y));
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(asteroid, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);
        }
        
      
    }

}
