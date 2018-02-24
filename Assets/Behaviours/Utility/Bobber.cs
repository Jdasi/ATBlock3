using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] float strength = 5;
    [SerializeField] float speed = 2;

    private float original_y;


    void Awake()
    {
        original_y = transform.localPosition.y;
    }


    void Update()
    {
        float y = original_y + Mathf.Sin(Time.time * speed) * strength;
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }

}
