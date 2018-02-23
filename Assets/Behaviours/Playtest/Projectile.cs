using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 origin { get; protected set; }


    public void Init(Vector3 _origin)
    {
        origin = _origin;
    }

}
