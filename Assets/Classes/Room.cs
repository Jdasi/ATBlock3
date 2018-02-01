using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int top { get; private set; }
    public int bottom { get; private set; }
    public int left { get; private set; }
    public int right { get; private set; }

    public int x { get; private set; }
    public int y { get; private set; }

    public int width { get; private set; }
    public int height { get; private set; }


    public Room(int _x, int _y, int _width, int _height)
    {
        x = _x;
        y = _y;

        width = _width;
        height = _height;

        top = _y;
        bottom = _y + _height;

        left = _x;
        right = _x + _width;
    }

}
