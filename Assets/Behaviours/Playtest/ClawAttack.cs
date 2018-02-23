using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : Projectile
{
    [SerializeField] int damage;
    [SerializeField] float radius;
    [SerializeField] float knockback = 5;
    [SerializeField] LayerMask hit_layers;


    public void Start()
    {
        DamageAllInSphere();
        Destroy(this.gameObject);
    }


    void DamageAllInSphere()
    {
        var hits = Physics.OverlapSphere(transform.position, radius, hit_layers);
        foreach (var hit in hits)
        {
            var life = hit.GetComponent<LifeForce>();
            if (life != null)
            {
                life.Damage(damage);
            }

            var rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 normal = (hit.transform.position - origin).normalized;
                rb.AddForce((normal + Vector3.up) * knockback, ForceMode.Impulse);
            }
        }
    }

}
