using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    // Moving
    public NavMeshAgent Enemy;
    public GameObject Player;

    public float stopDist; 

    // Attacking
    public GameObject PlayerBody; 
    
    public float timeBtwAttacks = 0.5f;
    public bool alreadyAttacked = false;
    public int damage = 10; // TEMP?

    void Start()
    {
        int avoiding = Random.Range(0, 99);
        Enemy.avoidancePriority = avoiding;
        Player = GameObject.FindWithTag("Player");
        
        
        // playerHealth = PlayerBody.GetComponent<PlayerScript>().health;
        // PlayerScript playerScript = GameObject.Find("PlayerBody").GetComponent<PlayerScript>();

    }

    // Update is called once per frame
    void Update()
    {

        // Movement Script

        float dist = Vector3.Distance(Enemy.transform.position, Player.transform.position);
        if (dist <= stopDist)
        {
            StopEnemy();
            AttackPlayer();
        }
        else 
        {
            MoveTowards();
        }
                
    }

    void MoveTowards()
    {
        Enemy.isStopped = false;
        Enemy.SetDestination(Player.transform.position);
    }
    void StopEnemy()
    {
        Enemy.isStopped = true; 
        Enemy.SetDestination(Enemy.transform.position);
    }
    void AttackPlayer()
    {
        
        if (!alreadyAttacked)
        {
            Player.GetComponent<PlayerScript>().UpdateHealth(-damage);
            //call anim

            alreadyAttacked = true; 
            Invoke(nameof(ResetAttack), timeBtwAttacks);
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
