using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, DungeonInteractable
{
    [SerializeField] float delay_before_close = 3;
    [SerializeField] float move_speed = 10;
    [SerializeField] float slide_height = 1.8f;

    private float close_timestamp;
    private bool is_open;

    private Vector3 origin;
    private float y_target;


    public void Activate()
    {
        Open();
    }


    void Awake()
    {
        origin = transform.position;
        y_target = 0;
    }


    void Update()
    {
        if (is_open && Time.time >= close_timestamp)
        {
            Close();
        }

        HandleLerp();
    }


    void Open()
    {
        if (is_open)
            return;

        is_open = true;
        close_timestamp = Time.time + delay_before_close;
        y_target = slide_height;
    }


    void Close()
    {
        is_open = false;
        y_target = 0;
    }


    void HandleLerp()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(origin.x, y_target, origin.z),
            move_speed * Time.deltaTime);
    }

}
