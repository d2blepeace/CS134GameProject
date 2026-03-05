using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Enemy AI will patroling an area, if player is near, attack
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatGround, whatPlayer;

    //States of enemy
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    // Patroling
    public Vector3 patrolPoint;
    bool patrolPointSet;
    public float patrolPointRange;

    // Attack
    public float timeBetweenAttack;
    bool alreadyAttacked;

    private void Awake()
    {
        // Enemy will find Player tag
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {}

    // Patroling handling
    private void Patroling()
    {
        
    }

    // Chase Player
    private void ChasePlayer() {}

    // Attack Player
    private void AttackPlayer() {}
}
