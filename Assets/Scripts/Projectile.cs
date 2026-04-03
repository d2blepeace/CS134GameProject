// Projectile must:
// Fired forward when being fired
// Can be parried by player, it will reflect back to enemy
// Damage enemies when reflected

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour 
{
    private bool hasHit = false;
    [Header("Projectile Setting")]
    [SerializeField] private float speed = 14f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 1f;

    private Rigidbody rb;
    
    // Stored who fired this projectile to skip self-collision on launch
    private Transform shooter;
    // Stored the Owner of projectile to points at enemy after being parried
    public Transform Owner
    {
        get;
        private set;
    }

    //Check if projectile has been parried and reflected yet
    private bool reflected = false;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start() 
    {
        // Destroy after some time so it less the work on the scene
        Destroy(gameObject, lifetime);
    }

    //Call when EnemyAI shoot projectile
    public void Fire(Vector3 direction, Transform owner)
    {
        Owner = owner;
        shooter = owner;        //lock in original shooter    
        reflected = false;
        rb.velocity = direction.normalized * speed;
    }

    // called when parry zone trigger detects this projectile 
    public void Reflect(Transform relfectSource)
    {
        reflected = true;

        if (Owner != null )
        {
            //aim directly at enemy's current global position
            Vector3 aimDir = (Owner.position - transform.position).normalized;
            rb.velocity = aimDir * speed;
        }
        else
        {
            // simply reflect backward in case this one is buggy
            rb.velocity = -rb.velocity.normalized * speed;
        }
    }

    // Parry zone is a trigger collider child on player, enabled by PlayerParry
    private void OnTriggerEnter(Collider other)
    {
        // Look for PlayerParry on collider on its parent
        PlayerParry  parry = other.GetComponentInParent<PlayerParry>();
        if (parry == null) return;

        // Only reflect projectile back if parry window is currently active
        if (parry.IsParryActive)
        {
            Reflect(other.transform);
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (hasHit) return;
        hasHit = true;

        // Let the projectile collide freely to enemy after being parried
        if (collision.transform == shooter && !reflected) return;

        // Reflected projectile hits enemy
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy != null && reflected)
        {
            enemy.TakeDamage((int)damage);
            Destroy(gameObject);
            return;
        }

        // Projectile hits player before parried
        if (collision.gameObject.CompareTag("Player") && !reflected)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(1);

            Destroy(gameObject);
            return;
        }

        // Destroy on hitting other object
        Destroy(gameObject);
    }
}
