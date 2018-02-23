using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaff : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] float horizontal_sway = 0.5f;
    [SerializeField] float horizontal_time_factor = 1.0f;

    [Space]
    [SerializeField] float vertical_sway = 0.1f;
    [SerializeField] float vertical_time_factor = 0.5f;

    [Space]
    [SerializeField] float sway_speed = 10;

    [Header("Attack Settings")]
    [SerializeField] float attack_cooldown = 0.4f;
    [SerializeField] GameObject projectile_prefab;
    [SerializeField] Transform shoot_point;

    [Space]
    [SerializeField] AnimationCurve swing_curve;
    [SerializeField] Vector3 swing_rotation = new Vector3(33, 0, 0);
    [SerializeField] float swing_duration = 0.5f;

    [Header("References")]
    [SerializeField] SpriteRenderer staff_sprite;
    [SerializeField] Transform animation_joint;

    private Vector3 origin;
    private float sway_target;

    private float sway_timer;
    private float sway_amount;

    private float swing_timer;
    private float next_shoot_timestamp;

    private bool can_shoot
    {
        get { return Time.time >= next_shoot_timestamp; }
    }


    public void Sway(float _target)
    {
        sway_target = _target;
    }


    public void Shoot()
    {
        if (!can_shoot)
            return;

        swing_timer = 0;
        next_shoot_timestamp = Time.time + attack_cooldown;


        RaycastHit hit;
        bool shot_obstructed = Physics.Raycast(shoot_point.position, shoot_point.forward,
            out hit, 1, ~LayerMask.NameToLayer("Player"));

        var clone = Instantiate(projectile_prefab, shoot_point.position + shoot_point.forward,
            Quaternion.LookRotation(transform.forward));
        var bolt = clone.GetComponent<FireBolt>();

        if (shot_obstructed)
        {
            bolt.Explode(hit.collider);
        }
    }


    void Awake()
    {
        origin = animation_joint.transform.localPosition;
        swing_timer = swing_duration;
    }


    void Update()
    {
        HandleSway();
        HandleSwing();
    }


    void HandleSway()
    {
        sway_timer += Time.deltaTime * sway_speed;
        sway_amount = Mathf.Lerp(sway_amount, sway_target, sway_speed * Time.deltaTime);

        float h_sway = Mathf.Sin(sway_timer * horizontal_time_factor) * horizontal_sway;
        float v_sway = Mathf.Sin(sway_timer * vertical_time_factor) * vertical_sway;

        animation_joint.transform.localPosition = origin + (new Vector3(h_sway, v_sway) * sway_amount);
    }


    void HandleSwing()
    {
        if (swing_timer >= swing_duration)
            return;

        swing_timer += Time.deltaTime;

        float curve_step = swing_curve.Evaluate(swing_timer / swing_duration);
        animation_joint.transform.localRotation = Quaternion.Euler(swing_rotation * curve_step);
    }

}
