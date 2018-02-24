using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitPortal : MonoBehaviour
{
    public UnityEvent portal_entered_events;


    void OnTriggerEnter(Collider _other)
    {
        if (!_other.CompareTag("Player"))
            return;

        PlayerController player = _other.GetComponent<PlayerController>();
        if (!player.life.IsAlive())
            return;

        portal_entered_events.Invoke();
    }

}
