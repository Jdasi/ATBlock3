using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] bool ignore_y;


    void LateUpdate()
    {
        if (ignore_y)
        {
            Vector3 c = JHelper.main_camera.transform.position;
            transform.LookAt(new Vector3(c.x, transform.position.y, c.z));
        }
        else
        {
            transform.LookAt(JHelper.main_camera.transform.position);
        }
    }

}
