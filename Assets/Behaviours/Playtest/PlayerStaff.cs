using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaff : MonoBehaviour
{
    [SerializeField] float horizontal_sway = 0.5f;
    [SerializeField] float horizontal_time_factor = 1.0f;

    [Space]
    [SerializeField] float vertical_sway = 0.1f;
    [SerializeField] float vertical_time_factor = 0.5f;

    [Space]
    [SerializeField] float sway_speed = 10;

    private Vector3 origin;
    private float sway_target;

    private float sway_timer;
    private float sway_amount;


    public void Sway(float _target)
    {
        sway_target = _target;
    }


    void Awake()
    {
        origin = transform.position;
    }


    void Update()
    {
        sway_timer += Time.deltaTime * sway_speed;
        sway_amount = Mathf.Lerp(sway_amount, sway_target, sway_speed * Time.deltaTime);

        float h_sway = Mathf.Sin(sway_timer * horizontal_time_factor) * horizontal_sway;
        float v_sway = Mathf.Sin(sway_timer * vertical_time_factor) * vertical_sway;

        transform.localPosition = origin + (new Vector3(h_sway, v_sway) * sway_amount);
    }

}
