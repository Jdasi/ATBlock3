using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float speed = 6;
    [SerializeField] float turn_speed = 150;
    [SerializeField] float strafe_speed_modifier = 0.75f;
    [SerializeField] float back_speed_modifier = 0.75f;
    [SerializeField] float sprint_speed_modifier = 1.5f;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;

    private float horizontal;
    private float vertical;

    private bool strafing;
    private bool sprinting;

    private float modified_speed
    {
        get
        {
            return speed * (1 + (sprinting ? sprint_speed_modifier : 0));
        }
    }

    private Vector3 prev_pos;
    private float current_speed;


    void Start()
    {

    }


    void Update()
    {
        current_speed = (prev_pos - transform.position).magnitude;

        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        strafing = Input.GetMouseButton(1);
        sprinting = Input.GetKey(KeyCode.LeftShift);

        prev_pos = transform.position;
    }


    void FixedUpdate()
    {
        HandleMovement();
    }


    void HandleMovement()
    {
        Vector3 move = Vector3.zero;

        if (horizontal != 0)
        {
            if (strafing)
            {
                horizontal *= strafe_speed_modifier;
                move += transform.right * modified_speed * horizontal;
            }
            else
            {
                transform.Rotate(0, horizontal * turn_speed, 0);
            }
        }

        if (vertical != 0)
        {
            if (vertical < 0)
                vertical *= back_speed_modifier;

            move += transform.forward * modified_speed * vertical;
        }

        rigid_body.MovePosition(transform.position + move);
    }

}
