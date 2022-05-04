using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public AudioSource source;
    public AudioClip ac;
    ScoreManager manager;
    WaveSpawner spawn;
    public int points = 10;
    
    private void Awake() {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<ScoreManager>();
        spawn = GameObject.FindGameObjectWithTag("manager").GetComponent<WaveSpawner>();
    }
    void OnDestroy() 
    {
        if (!spawn.delete)
        {
            manager.playerScore += points;
            source.PlayOneShot(ac, 0.8f);
        }
          
    }
}
