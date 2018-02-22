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

    [Header("Interaction Parameters")]
    [SerializeField] float interaction_distance = 2;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] Transform eyes_transform;

    private PlayerInventory inventory;

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


    void Awake()
    {
        inventory = this.gameObject.AddComponent<PlayerInventory>();
    }


    void Update()
    {
        current_speed = (prev_pos - transform.position).magnitude;

        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        strafing = Input.GetMouseButton(1);
        sprinting = Input.GetKey(KeyCode.LeftShift);

        prev_pos = transform.position;

        HandleInteraction();
    }


    void FixedUpdate()
    {
        HandleMovement();
    }


    void HandleInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(eyes_transform.position, eyes_transform.forward,
            out hit, interaction_distance, ~LayerMask.NameToLayer("Player")))
        {
            return;
        }

        var i = hit.collider.GetComponentInParent<DungeonInteractable>();
        if (i != null)
        {
            i.Activate();
        }
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
