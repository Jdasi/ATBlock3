using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float speed = 10;
    [SerializeField] float noclip_speed = 15;
    [SerializeField] float strafe_speed_modifier = 1;
    [SerializeField] float back_speed_modifier = 1;
    [SerializeField] float sprint_speed_modifier = 1.5f;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] Transform perspective_transform;
    [SerializeField] Transform torso_transform;

    private float horizontal;
    private float vertical;

    public bool noclip;

    private bool sprinting;
    

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            noclip = !noclip;

        horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * CurrentSpeed() * strafe_speed_modifier;
        vertical = Input.GetAxis("Vertical") * Time.deltaTime * CurrentSpeed();

        sprinting = Input.GetKey(KeyCode.LeftShift);

        if (vertical < 0)
            vertical *= back_speed_modifier;
    }


    void FixedUpdate()
    {
        if (perspective_transform == null)
        {
            transform.position += new Vector3(horizontal, 0, vertical);
        }
        else
        {
            Vector3 move = new Vector3();

            if (noclip)
            {
                move = (horizontal * perspective_transform.transform.right) +
                       (vertical * perspective_transform.forward);
            }
            else
            {
                move = (horizontal * perspective_transform.transform.right) +
                    (vertical * torso_transform.forward);
            }

            if (horizontal != 0 && vertical != 0)
                move *= strafe_speed_modifier;

            rigid_body.MovePosition(rigid_body.position + move);
        }
    }


    float CurrentSpeed()
    {
        return (noclip ? noclip_speed : speed) * (1 + (sprinting ? sprint_speed_modifier : 0));
    }

}
