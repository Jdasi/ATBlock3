using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int top { get; private set; }
    public int bottom { get; private set; }
    public int left { get; private set; }
    public int right { get; private set; }

    private int x;
    private int y;

    private int width;
    private int height;


    public Room(int _x, int _y, int _width, int _height, IMapManager _imap_manager)
    {
        x = _x;
        y = _y;

        width = _width;
        height = _height;

        PaintRoom(_imap_manager);
    }


    void PaintRoom(IMapManager _imap_manager)
    {
        for (int row = y; row < y + height; ++row)
        {
            for (int col = x; col < x + width; ++col)
            {
                int index = JHelper.CalculateIndex(col, row, _imap_manager.map_columns);
                _imap_manager.Paint(index, TerrainType.GRASS, false);
            }
        }
    }

}
