using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Torch : MonoBehaviour
{
    [SerializeField] float flicker_variance = 0.1f;

    private Light torch_light;
    private float original_intensity = 1;


    void OnEnable()
    {
        torch_light.enabled = true;
    }


    void OnDisable()
    {
        torch_light.enabled = false;
    }


    void Awake()
    {
        torch_light = GetComponent<Light>();
        original_intensity = torch_light.intensity;
    }


    void Update()
    {
        torch_light.intensity = Random.Range(
            original_intensity - (original_intensity * flicker_variance),
            original_intensity + (original_intensity * flicker_variance));
    }

}
