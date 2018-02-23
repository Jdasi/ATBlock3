using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] int damage = 10;
    [SerializeField] float speed = 10;
    [SerializeField] float knockback = 5;

    [Space]
    [SerializeField] float destroy_delay = 1.5f;
    [SerializeField] GameObject explosion_prefab;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] ParticleSystem particle_system;
    [SerializeField] Light torch_light;

    private bool hit_something;
    private Vector3 origin;
    private Vector3 explode_normal;


    public void Explode()
    {
        if (hit_something)
            return;

        hit_something = true;
        torch_light.enabled = false;

        Instantiate(explosion_prefab, transform.position + (explode_normal * 0.5f), Quaternion.identity);

        particle_system.Stop();
    }


    void Start()
    {
        origin = transform.position;
        Invoke("Explode", destroy_delay);
    }


    void Update()
    {
        if (hit_something && !particle_system.IsAlive())
        {
            Destroy(this.gameObject);
            return;
        }
    }


    void FixedUpdate()
    {
        HandleMovement();
    }


    void OnTriggerEnter(Collider _other)
    {
        if (hit_something)
            return;

        explode_normal = (origin - transform.position).normalized;

        var life = _other.GetComponent<LifeForce>();
        if (life != null)
        {
            life.Damage(damage);
        }

        var rb = _other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce((-explode_normal + Vector3.up) * knockback, ForceMode.Impulse);
        }

        Explode();
    }


    void HandleMovement()
    {
        if (hit_something)
            return;

        Vector3 step = transform.forward * speed * Time.fixedDeltaTime;
        rigid_body.MovePosition(transform.position + step);
    }

}
