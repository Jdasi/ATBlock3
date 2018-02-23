using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Enemy Blob")]
public class EnemyBlob : ScriptableObject
{
    public int max_health { get { return max_health_; } }
    public float move_speed { get { return move_speed_; } }
    public float engage_range { get { return engage_range_; } }
    public float attack_delay { get { return attack_delay_; } }

    [Header("General Stats")]
    [SerializeField] int max_health_ = 30;
    [SerializeField] float move_speed_ = 3;

    [Header("Combat Stats")]
    [SerializeField] float engage_range_ = 5;
    [SerializeField] float attack_delay_ = 1;
    [SerializeField] GameObject attack_prefab_;


    public void Attack(Vector3 _pos, Vector3 _forward)
    {
        if (attack_prefab_ == null)
            return;

        var clone = Instantiate(attack_prefab_, _pos + _forward, Quaternion.LookRotation(_forward));
        var projectile = clone.GetComponent<Projectile>();

        if (projectile != null)
            projectile.Init(_pos);
    }

}
