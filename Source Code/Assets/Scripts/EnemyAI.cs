using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    //Attacking
    public float timeBtwAttacks;
    bool alreadyAttacked;

    public int damage = 10;

    //States
    public float attackRange;
    public bool playerInARange;

    private void Awake() 
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() 
    {
        playerInARange = Physics.CheckSphere(this.transform.position, attackRange, 8);
        
        if (playerInARange) 
            AttackPlayer();
        else    
            ChasePlayer();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private void AttackPlayer()
    {
        //stop enemy from moving
        agent.SetDestination(this.transform.position);

        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            player.GetComponent<PlayerScript>().UpdateHealth(-damage);
            //call anim

            Debug.Log("attk");
            alreadyAttacked = true; 
            Invoke(nameof(ResetAttack), timeBtwAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}

