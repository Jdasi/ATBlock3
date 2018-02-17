using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{

    public void TriggerState()
    {
        this.enabled = true;
    }

}
