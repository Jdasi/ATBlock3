using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyBlob eblob;
    [SerializeField] LayerMask blocking_layers;

    [Header("References")]
    [SerializeField] FadableGraphic damage_fade;
    [SerializeField] Transform body_transform;
    [SerializeField] Rigidbody rigid_body;

    private const float detection_interval = 1f;
    private const float update_radius = 200;

    private bool active;

    private bool detected_player;
    private float dist_to_player;

    private bool can_attack { get { return Time.time >= next_attack_timestamp; } }
    private float next_attack_timestamp;


    public void Kill()
    {
        Destroy(this.gameObject);
    }


    public void Damage()
    {
        damage_fade.FadeOut(0.3f);
    }


    void Awake()
    {
        InvokeRepeating("TestPlayerDetection", 0, detection_interval);
    }


    void Update()
    {
        active = (transform.position - GameManager.scene.player.transform.position).sqrMagnitude <= update_radius;
        if (!active)
            return;

        HandleDetection();
        HandleAttack();
    }


    void FixedUpdate()
    {
        HandleMovement();
    }


    void OnDrawGizmos()
    {
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3.up * 1.5f), 0.2f);
    }


    void HandleDetection()
    {
        if (detected_player)
        {
            dist_to_player = Vector3.Distance(transform.position, GameManager.scene.player.transform.position);
        }
    }


    void HandleMovement()
    {
        if (!detected_player || dist_to_player <= eblob.engage_range)
            return;

        Vector3 dir = (GameManager.scene.player.transform.position - transform.position).normalized;
        rigid_body.MovePosition(transform.position + (dir * eblob.move_speed * Time.fixedDeltaTime));
    }


    void HandleAttack()
    {
        if (!detected_player || !can_attack || dist_to_player > eblob.engage_range)
            return;

        next_attack_timestamp = Time.time + eblob.attack_delay;
        eblob.Attack(body_transform.position, body_transform.forward);
    }


    void TestPlayerDetection()
    {
        if (!active)
            return;

        RaycastHit hit;
        if (Physics.Raycast(body_transform.position, body_transform.forward,
            out hit, 100, blocking_layers))
        {
            detected_player = hit.collider.CompareTag("Player");
        }
        else
        {
            detected_player = false;
        }
    }

}
