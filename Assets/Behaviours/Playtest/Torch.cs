using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Torch : MonoBehaviour
{
    [SerializeField] float original_intensity = 1;
    [SerializeField] float flicker_variance = 0.1f;

    private Light light;


    void Awake()
    {
        light = GetComponent<Light>();
    }


    void Update()
    {
        light.intensity = Random.Range(
            original_intensity - (original_intensity * flicker_variance),
            original_intensity + (original_intensity * flicker_variance));
    }

}
