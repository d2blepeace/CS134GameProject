using UnityEngine;

/// Projectile fired by EnemyAI that can be reflected back via the player parry.
/// 
///   - "shooter" is immutable, set once on Fire() so the projectile always
///         ignores its original launcher's collider, even after reflection
///   - "Owner" stays mutable, retained through reflection so the reflected
///         projectile can aim-lock toward the enemy's current world position
///   - "damage" is a float throughout the pipeline to avoid silent casting bugs

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
        if (parry.IsParryActive && !reflected)
        {
            Reflect(other.transform);
            // play parrySuccessSFX
            parry.PlayParrySuccessSfx();
        }
    }
   /// Collision-based damage and destruction logic.
    ///   1. Reflected projectile hits player-> ignored 
    ///   2. Un-reflected projectile hits its own shooter -> ignored 
    ///   3. Reflected projectile hits an enemy -> deals damage, destroys self
    ///   4. failed to parried projectile hits the player -> deals damage to player, destroys self
    ///   5. Hits anything else (walls, floor) -> destroys self
    private void OnCollisionEnter(Collision collision) 
    {
        if (hasHit) return;

        // Case 1: Reflected projectile should not damage the player
        if (collision.gameObject.CompareTag("Player") && reflected)
        {
            return;
        }

        hasHit = true;
        
        // Case 2: Skip collision with the original shooter before reflection
        if (collision.transform == shooter && !reflected) return;

        // Case 3: Reflected projectile damages an enemy
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy != null && reflected)
        {
            enemy.TakeDamage((int)damage);
            Destroy(gameObject);
            return;
        }

        // Case 4: failed to parried projectile hits the player
        if (collision.gameObject.CompareTag("Player") && !reflected)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(1);

            Destroy(gameObject);
            return;
        }

        // Case 5: Hit environment or other objects
        Destroy(gameObject);
    }
}
