using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords
{
    public Coords(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public Coords()
    {
        x = 0;
        y = 0;
    }

    public int x;
    public int y;
}
