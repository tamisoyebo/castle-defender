using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
   //public Vector3[] spawnPoints;
    public GameObject[] enemies;

    public int level = 0;
    public float[] waveTime = new float[3]{90.0f, 120.0f, 180.0f};
    public float[] spawnTime = new float[3]{9.0f, 6.0f, 4.5f};
    public float tutorialTime = 300.00f;
    public float tutorialSpawn = 20.0f;
    public float timer;
    public float spawnLaunchSpeed = 50f;

    // XZ Parameters
    /*
    public float xmin;
    public float xmax;
    public float zmin; 
    public float zmax;
    */

    // XY Generated 
    // private float xgen = 0.0f;
    // private float zgen = 0.0f;
    // Spawn Pts Generation
    public GameObject spawnPt0 = null; 
    public GameObject spawnPt1 = null;  
    public GameObject spawnPt2 = null; 

    private int chosenSpawn = 0; 
    private Transform ranCoord; 

    bool spawning = true;
    public bool paused;
    public UIManager manager;

    public bool delete;

    public bool transition;

    void Awake() 
    {
        
        paused = manager.paused;

        if (SceneManager.GetActiveScene().buildIndex == 2) timer = tutorialTime;
        else timer = waveTime[level];
        
        transition = false;
        delete = false;
    }

    void Start() 
    {
        Debug.Log(timer);

        if (SceneManager.GetActiveScene().buildIndex == 2) InvokeRepeating("RandPos" , 0f, tutorialSpawn);
        else InvokeRepeating("RandPos" , 0f, spawnTime[level]);
        
    }

    void Update() 
    {            
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            transition = true;
            ClearEnemies();

            Debug.Log("stawp spawning");          
        }
    }
    void RandPos() // Chooses which position to spawn, different positions have different functions so this code generates the position and calls the different functions (normal spawn vs launch spawn)
    {
        GameObject[] allPrefabs = new GameObject[3] {spawnPt0, spawnPt1, spawnPt2}; 
        chosenSpawn = Random.Range(0, allPrefabs.Length);

        ranCoord = allPrefabs[chosenSpawn].GetComponent<Transform>();

        SpawnEnemy();

        Debug.Log("chosen spawn" + chosenSpawn);

        // xgen = Random.Range(xmin, xmax);
        // zgen = Random.Range(zmin, zmax);
    }
    void SpawnEnemy()
    {
        Debug.Log("normal spawn");
        Instantiate(enemies[Random.Range(0, enemies.Length)], ranCoord);
        if (!spawning)
        {
            CancelInvoke("RandPos");
        }
    }

    GameObject[] FindInLayer(int layer)
    {
        GameObject[] enemies = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var enemyList = new List<GameObject>();
        for (var i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].layer == layer)
            {
                enemyList.Add(enemies[i]);
            }
        }

        if (enemyList.Count == 0)
        {
            return null;
        }

        return enemyList.ToArray();

    }

    public void ClearEnemies()
    {
        delete = true;
        GameObject[] enemies = FindInLayer(6);
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            } 
        }
}

}
