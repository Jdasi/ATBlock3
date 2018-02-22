using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float speed = 10;
    [SerializeField] float sprint_speed_modifier = 1.5f;

    [Header("Looking Parameters")]
    [SerializeField] float horizontal_look_sensitivity = 150.0f;    // Sensitivity of the mouse X axis.
    [SerializeField] float vertical_look_sensitivity = 100.0f;      // Sensitivity of the mouse Y axis.
    [SerializeField] bool y_flipped;                                // Should the mouse Y axis be flipped?

    [Header("References")]
    [SerializeField] Transform affected_transform;

    private float pan_horizontal;
    private float pan_vertical;

    private bool mouse_locked;
    private bool sprinting;

    private float current_speed
    {
        get
        {
            return speed * (1 + (sprinting ? sprint_speed_modifier : 0));
        }
    }


    void Start()
    {

    }
	

    void Update()
    {
        HandleMouseLookToggle();

        if (mouse_locked)
            HandleMouseLook();

        Cursor.visible = !mouse_locked;
        Cursor.lockState = mouse_locked ? CursorLockMode.Locked : CursorLockMode.None;

        HandleMovement();
    }


    void HandleMouseLookToggle()
    {
        mouse_locked = Input.GetMouseButton(1);
    }


    void HandleMouseLook()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        pan_horizontal +=  horizontal * Time.deltaTime * horizontal_look_sensitivity;
        pan_vertical -= (!y_flipped ? vertical : -vertical) * Time.deltaTime * vertical_look_sensitivity;

        pan_vertical = Mathf.Clamp(pan_vertical, -90, 90);

        affected_transform.rotation = Quaternion.Euler(pan_vertical, pan_horizontal, 0);
    }


    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime;

        sprinting = Input.GetKey(KeyCode.LeftShift);

        Vector3 move = (horizontal * affected_transform.transform.right) +
                       (vertical * affected_transform.forward);

        affected_transform.position += move * current_speed;
    }


    void OnDestroy()
    {
        if (mouse_locked)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
