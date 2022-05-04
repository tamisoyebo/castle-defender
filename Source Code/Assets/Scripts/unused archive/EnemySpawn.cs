using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject spawnEnemy; 
    public float spawnRate = 1.0f; 

    private float time = 1; 

    // XZ Parameters

    public float xmin;
    public float xmax;
    public float zmin; 
    public float zmax;

    // XY Generated
    private float xgen = 0.0f;
    private float zgen = 0.0f;

    void Start()
    {
        time = spawnRate; 
    }

    void Update()
    {
        time -= Time.deltaTime; 
        if (time <= 0.0f)
        {
            time = spawnRate; 

            RandPos(); // Generate Random Position on XZ Axis
            Instantiate(spawnEnemy, new Vector3(xgen, 5.0f, zgen), Quaternion.identity);
        }
    }

    void RandPos()
    {
        xgen = Random.Range(xmin, xmax);
        zgen = Random.Range(zmin, zmax);
    }
}