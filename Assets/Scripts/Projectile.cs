// Projectile must:
// Fired forward when being fired
// Can be parried by player, it will reflect back to enemy
// Damage enemies when reflected

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehavior 
{
    [Header("Projectile Setting")]
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;

    private Rigidbody rb;
    // Onwer of projectile in case player can shoot projectile too
    public Transform Owneer {get; private set;}

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

    //Call when enemies shoot projectile
    public void Fire(Vector3 direction, Transform owner)
    {
        Owner = owner;
        reflected = false;
        rb.velocity = direction.normalized * speed;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        // Prevent projectile hitting its owner
        if (collision.transform == Owner) return;

        // If projectile hits enemy after being reflected
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

        if (enemy != null && reflected) {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        //If it hits player before parried successfully
        if(collision.gameObject.CompareTag("Player") && !reflected)
        {
            //Script for player being damage (need work after working on health system)
            Destroy(gameObject);
            return;
        }
        // Destroy on hitting other object: walls, dynamic object
        Destroy(gameObject);
    }
}
