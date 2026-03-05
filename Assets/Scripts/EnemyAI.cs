using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Enemy AI will patroling an area, if player is near, attack
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;

     [Header("Layers")]
    public LayerMask indicateGround, indicatePlayer;

    [Tooltip("Where the projectile spawns from (child transform). If null, uses enemy position + up.")]
    public Transform shootPoint;

    [Tooltip("Projectile prefab that has a Projectile.cs script on it.")]
    public GameObject projectilePrefab;

    [Header("Stats")]
    public float health;

    [Header("Ranges")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    // Patroling
    public Vector3 patrolPoint;
    bool patrolPointSet;
    public float patrolPointRange;

    // Attack
    public float timeBetweenAttack = 1.25f;
    bool alreadyAttacked;

    private void Awake()
    {
        // Enemy will find Player tag
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, indicatePlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, indicatePlayer);

        // At default: player is not in sight AND attack range, do Patroling()
        if (!playerInSightRange && !playerInAttackRange) Patroling();

        // If player is insight but not inattack range, Chase()
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

        //if player is in sight AND in range of attack, do attack()
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    // Patroling handling
    private void Patroling()
    {
        if (!patrolPointSet) SearchPatrolPoint();

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
        }

        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;

        //If aptrol point reach, search new one to patrol
        if (distanceToPatrolPoint.magnitude < 1f)
        {
            patrolPointSet = false;
        }
    }
    void SearchPatrolPoint()
    {
        // calculate random patrol point in range
        float ZPatrolPoint = Random.Range(-patrolPointRange, patrolPointRange);
        float XPatrolPoint = Random.Range(-patrolPointRange, patrolPointRange);

        patrolPoint = new Vector3(transform.position.x + XPatrolPoint, transform.position.y, transform.position.z + ZPatrolPoint);

        //Indicate where is ground to prevent enemy fall from map
        if (Physics.Raycast(patrolPoint, -transform.up, 2f, indicateGround))
        {
            patrolPointSet = true;
        }
    }   

    // Chase Player
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    // Attack Player
    private void AttackPlayer()
    {
        //Enemy will stop to attack
        agent.SetDestination(transform.position);

        // Face the player directly
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(lookDir);

        if (alreadyAttacked) return;

        // Spawn position
        Vector3 spawnPos = (shootPoint != null) ? shootPoint.position : (transform.position + Vector3.up * 1f);

        // Aim direction toward player
        Vector3 dir = (player.position - spawnPos).normalized;

        // Instantiate projectile
        GameObject go = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(dir));

        Projectile proj = go.GetComponent<Projectile>();
        // If already done an attack, reset the attack
        if (proj != null)
        {
            proj.Fire(dir, transform);                  // owner = this enemy
        }
        else
        {
            // Fallback if you haven't added Projectile.cs yet:
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = dir * 14f;
        }

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttack);
    }

    // Reset Attack
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //Take damage 
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
