using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public static SpawnProjectile instance = null;
    public GameObject projectilePrefab;
    Transform projectileSpawn;
    GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        projectileSpawn = transform.Find("SpawnPoint");
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Update is called once per frame
    

    public void Fire()
    {
        projectile = Instantiate(projectilePrefab, projectileSpawn.position , projectileSpawn.rotation);
        
        projectile.GetComponent<Particle2D>().position = projectileSpawn.position;
        Debug.Log("Send");
    }

    void Update()
    {
        
    }
}
