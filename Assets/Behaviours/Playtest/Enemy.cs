using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float move_speed;

    [Header("References")]
    [SerializeField] FadableGraphic damage_fade;

    private const float update_radius = 200;

    private PlayerController player;
    private bool active;


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

    }


    void Update()
    {
        if (player == null)
            player = GameObject.FindObjectOfType<PlayerController>();

        active = (transform.position - player.transform.position).sqrMagnitude <= update_radius;
        if (!active)
            return;

        // TODO: enemy logic ..
    }


    void OnDrawGizmos()
    {
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3.up * 1.5f), 0.2f);
    }

}
