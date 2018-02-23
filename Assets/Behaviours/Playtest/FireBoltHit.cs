using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoltHit : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float light_fade_rate = 3;

    [Header("References")]
    [SerializeField] ParticleSystem particle_system;
    [SerializeField] Light point_light;


    void Start()
    {

    }

    
    void Update()
    {
        point_light.range -= light_fade_rate * Time.deltaTime;

        if (point_light.range <= 0 && !particle_system.IsAlive())
        {
            Destroy(this.gameObject);
            return;
        }
    }

}
