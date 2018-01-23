using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour
{
    public static float pixel_to_units = 1;
    public static float scale = 1;

    public Vector2 native_resolution = new Vector2(160, 144);


    void Awake()
    {
        Camera cam = GetComponent<Camera>();

        if (cam.orthographic)
        {
            int dir = Screen.height;
            float res = native_resolution.y;

            scale = dir / res;
            pixel_to_units *= scale;

            cam.orthographicSize = (dir / 2) / pixel_to_units;
        }
    }

}
