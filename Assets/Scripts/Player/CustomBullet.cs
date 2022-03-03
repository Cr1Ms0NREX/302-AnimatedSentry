using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    //Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;

    //Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    public void Start()
    {
        Setup();
    }

    public void Update()
    {
        //When to explode
        if (collisions > maxCollisions)
        {
            Explode();
        }

        //Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            Explode();
        }
    }
    private void Explode()
    {
        //Instantiate exlosion 
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get component of enemy and call Take Damage
            //enemies[i].GetComponent<EnemyHealth>().TakeDamage(explosionDamage);
            //Just an example!
            ///enemies[i].GetComponent<ShootAI>().TakeDamage(explosionDamage);
        }
        //Add a little delay, just to make sure everything works fine
        Invoke("Delay", .005f);
        //Delay();
    }
    private void BulletCollision(Collider2D collision)
    {
        if (collision.tag != "Bullet")
        {
            gameObject.transform.parent = collision.gameObject.transform;
            //Destroy();
            GetComponent<CircleCollider2D>().enabled = false;
        }

        if (collision.tag == "Enemy")
        {
            var healthComponent = collision.GetComponent<HealthSystem>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(40); 
            }
        }
    }
    private void Delay()
    {
        Destroy(gameObject);
        Destroy(explosion);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }
    private void Setup()
    {
        //Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
