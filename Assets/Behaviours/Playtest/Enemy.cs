using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int max_health;
    [SerializeField] float move_speed;
    [SerializeField] float detection_radius;

    private const float update_radius = 10;

    private PlayerController player;
    private int health;


    void Awake()
    {
        health = max_health;
    }


    void Update()
    {
        if (player == null)
            player = GameObject.FindObjectOfType<PlayerController>();

        if ((transform.position - player.transform.position).sqrMagnitude > update_radius)
            return;

        // TODO: enemy logic ..
    }

}
