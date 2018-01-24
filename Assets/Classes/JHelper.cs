using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JHelper
{
    public static int CalculateIndex(int _x, int _y, int _size_x)
    {
        return (_y * _size_x) + _x;
    }


    public static bool ValidIndex(int _index, int _array_size)
    {
        return _index >= 0 && _index < _array_size;
    }


    public static Texture2D LoadPNG(string _file_path)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(_file_path))
        {
            fileData = File.ReadAllBytes(_file_path);
            tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            tex.LoadImage(fileData);
        }

        return tex;
    }


    public static void SetLayerRecursive(GameObject _obj, int _layer)
    {
        _obj.layer = _layer;

        foreach (Transform child in _obj.transform)
            SetLayerRecursive(child.gameObject, _layer);
    }


    public static Camera main_camera
    {
        get
        {
            if (main_camera_ == null || Camera.current != main_camera_)
                main_camera_ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            return main_camera_;
        }
    }


    private static Camera main_camera_;

}
