﻿using System.Collections;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] float move_speed = 20.0f;
    [SerializeField] float scroll_speed = 50.0f;
    [SerializeField] float drag_speed = 4.0f;
    [SerializeField] float shift_modifier = 3.0f;

    [Space]
    [SerializeField] float min_zoom = 1;
    [SerializeField] float max_zoom = 100;

    private float current_modifier = 1.0f;
    private float original_zoom;

    private Vector3 last_reset_pos;
    private Vector3 old_cam_pos;
    private Vector3 drag_origin;


    public void ResetCamera()
    {
        JHelper.main_camera.transform.position = last_reset_pos;
        JHelper.main_camera.orthographicSize = original_zoom;
    }


    public void ResetCamera(Vector2 pos)
    {
        last_reset_pos = pos;
        last_reset_pos.z = JHelper.main_camera.transform.position.z;

        ResetCamera();
    }


	void Start()
    {
        original_zoom = JHelper.main_camera.orthographicSize;
	}


	void Update()
    {
        HandleSpeedModifier();
        //HandleKeyboardMovement();
        HandleMouseMovement();
        HandleZoom();
	}


    /// <summary>
    /// Camera move and scroll speed is based on shift key press.
    /// </summary>
    void HandleSpeedModifier()
    {
        current_modifier = Input.GetButton("CameraSpeedModifier") ? shift_modifier : 1.0f;
    }


    void HandleKeyboardMovement()
    {
        Vector3 temp = JHelper.main_camera.transform.position;

        temp.x += Input.GetAxis("Horizontal") * move_speed * Time.deltaTime * current_modifier;
        temp.y += Input.GetAxis("Vertical") * move_speed * Time.deltaTime * current_modifier;

        JHelper.main_camera.transform.position = temp;
    }


    /// <summary>
    /// Allows dragging of the camera with RMB.
    /// </summary>
    void HandleMouseMovement()
    {
        float scaled_speed = drag_speed * JHelper.main_camera.orthographicSize;

        if (Input.GetMouseButtonDown(1))
        {
            old_cam_pos = JHelper.main_camera.transform.position;
            drag_origin = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 drag_difference = JHelper.main_camera.ScreenToViewportPoint(Input.mousePosition - drag_origin);
            Vector3 move = new Vector3(drag_difference.x * scaled_speed, drag_difference.y * (scaled_speed / 1.5f), 0);

            Camera.main.transform.position = old_cam_pos - move;
        }
    }


    /// <summary>
    /// Allows zooming of the camera with mousewheel.
    /// </summary>
    void HandleZoom()
    {
        JHelper.main_camera.orthographicSize -= Input.GetAxis("MouseScroll") * scroll_speed * Time.deltaTime * current_modifier * JHelper.main_camera.orthographicSize;
        JHelper.main_camera.orthographicSize = Mathf.Clamp(JHelper.main_camera.orthographicSize, min_zoom, max_zoom);
    }

}
