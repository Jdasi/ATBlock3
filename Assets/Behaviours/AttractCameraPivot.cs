using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractCameraPivot : MonoBehaviour
{
    [Header("Mouse Influence")]
    [SerializeField] float horizontal_influence = 0.05f;
    [SerializeField] float vertical_influence = 0.05f;

    private Vector3 start_rot;


    void Awake()
    {
        start_rot = transform.localEulerAngles;
    }


    void Update()
    {
        float x_rot = (Input.mousePosition.x / Screen.width) * horizontal_influence;
        float y_rot = (Input.mousePosition.y / Screen.height) * -vertical_influence;

        transform.localEulerAngles = start_rot + new Vector3(y_rot, x_rot, 0);
    }

}
