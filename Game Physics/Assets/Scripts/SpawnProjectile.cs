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
        Particle2D particle = projectile.GetComponent<Particle2D>();
        particle.Position = projectileSpawn.position;
        particle.Rotation = projectileSpawn.rotation.eulerAngles.z;
    }

    void Update()
    {
        
    }
}
